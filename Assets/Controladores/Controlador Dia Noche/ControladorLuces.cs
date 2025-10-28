using System.Collections.Generic;
using UnityEngine;
#if CINEMACHINE
using Cinemachine;
#endif

[DisallowMultipleComponent]
public class LucesDeLaCiudadPro : MonoBehaviour
{
    [Header("Referencias (una de las dos)")]
    public CicloDiaNoche ciclo;
    public Light sol;

    [Header("Luces a controlar")]
    public List<Light> luces = new List<Light>();

    [Header("Curva noche (0..1 del día -> 0..1 noche)")]
    public AnimationCurve curvaIntensidad = new AnimationCurve(
        new Keyframe(0.00f, 1f),
        new Keyframe(0.25f, 0f),
        new Keyframe(0.65f, 0f),
        new Keyframe(0.85f, 1f),
        new Keyframe(1.00f, 1f)
    );

    [Header("Intensidad base")]
    [Min(0f)] public float intensidadMax = 2f;
    [Tooltip("Tiempo de suavizado al ENCENDER (s).")]
    [Min(0.01f)] public float suavizadoEncenderSeg = 0.3f;
    [Tooltip("Tiempo de suavizado al APAGAR (s).")]
    [Min(0.01f)] public float suavizadoApagarSeg = 1.2f;

    [Header("Variación por luminaria")]
    [Tooltip("Variación aleatoria por-luz (±%). 0.15 = ±15%")]
    [Range(0f, 0.5f)] public float variacionIntensidad = 0.15f;
    [Range(0f, 0.5f)] public float variacionRango = 0.10f;

    [Header("Color nocturno (opcional)")]
    public bool controlarColor = false;
    [Tooltip("Color/gradiente en función de fNoche (0..1).")]
    public Gradient colorNoche = new Gradient();
    [Range(0f, 1f)] public float mezclaColor = 1f; // 0 = mantiene color original; 1 = usa colorNoche

    [Header("Rango nocturno (opcional)")]
    public bool controlarRango = false;
    [Tooltip("Rango objetivo cuando es noche (antes de aplicar variación por-luz).")]
    [Min(0f)] public float rangoNoche = 12f;

    [Header("Cookie (Spot/Directional)")]
    public Texture cookie;                 // se aplica si la luz lo soporta
    [Min(0f)] public float cookieSize = 5; // solo Spot

    [Header("Flicker/Parpadeo (opcional)")]
    public bool flicker = false;
    [Tooltip("Amplitud de variación relativa (0.15 = ±15%).")]
    [Range(0f, 0.6f)] public float flickerAmplitud = 0.15f;
    [Tooltip("Velocidad del ruido de flicker.")]
    [Min(0.01f)] public float flickerVel = 1.5f;

    [Header("Cámaras (multi-cámara)")]
    [Tooltip("Agregá las cámaras REALES que renderizan (no las virtuales). Si se deja vacío, usa Camera.main.")]
    public List<Camera> cameras = new List<Camera>();
    [Tooltip("Si hay varias cámaras activas, usa la distancia mínima a cualquiera (más correcto, algo más caro).")]
    public bool unionDeCamaras = true;

    [Header("LOD / Rendimiento")]
    [Tooltip("Más allá de esta distancia se apaga la luz por completo.")]
    [Min(0f)] public float cullDist = 120f;
    [Tooltip("Más allá de esta distancia se desactivan sombras para ahorrar.")]
    [Min(0f)] public float noShadowDist = 60f;
    [Tooltip("Actualizar cada N frames.")]
    [Min(1)] public int framesEntreActualizaciones = 1;

    [Header("Emisión de materiales (opcional)")]
    public List<Renderer> emisores = new List<Renderer>();
    public Color colorEmisionNoche = new Color(1f, 0.9f, 0.7f, 1f);
    [Min(0f)] public float emisionMax = 2f;
    public bool actualizarGI = false;

    // ---------------- Internos ----------------
    struct LuzState
    {
        public Light l;
        public float velInt;          // SmoothDamp intensidad
        public float velRango;        // SmoothDamp rango
        public float multInt;         // variación por-luz (intensidad)
        public float multRango;       // variación por-luz (rango)
        public float seed;            // para flicker
        public float baseRango;
        public Color baseColor;
        public LightShadows baseSombras;
        public bool soportaCookie;
        public LightType tipo;
    }

    readonly List<LuzState> _estados = new();
    readonly List<MaterialPropertyBlock> _mpbs = new();

    // tmp list para cámaras activas
    static readonly List<Camera> _tmpCams = new List<Camera>(8);

    int _frame;

    void Awake()
    {
        // Preparar luces
        _estados.Clear();
        foreach (var l in luces)
        {
            if (!l) continue;

            var s = new LuzState
            {
                l = l,
                velInt = 0f,
                velRango = 0f,
                multInt = 1f + Random.Range(-variacionIntensidad, variacionIntensidad),
                multRango = 1f + Random.Range(-variacionRango, variacionRango),
                seed = Random.value * 1000f,
                baseRango = l.range,
                baseColor = l.color,
                baseSombras = l.shadows,
                tipo = l.type,
                soportaCookie = (l.type == LightType.Spot) || (l.type == LightType.Directional)
            };

            if (cookie && s.soportaCookie)
            {
                l.cookie = cookie;
                if (l.type == LightType.Spot) l.cookieSize = cookieSize;
            }

            _estados.Add(s);
        }

        // Emission: habilitar keyword una vez y preparar MPBs
        _mpbs.Clear();
        foreach (var r in emisores)
        {
            if (!r) continue;
            foreach (var m in r.sharedMaterials)
            {
                if (!m) continue;
                m.EnableKeyword("_EMISSION");
            }
            var mpb = new MaterialPropertyBlock();
            r.GetPropertyBlock(mpb);
            _mpbs.Add(mpb);
        }
    }

    void Update()
    {
        _frame++;
        if (_frame % framesEntreActualizaciones != 0) return;

        // Cálculo del porcentaje del día y factor de noche
        float pct = ObtenerPorcentajeDelDia();
        float fNoche = Mathf.Clamp01(curvaIntensidad.Evaluate(pct)); // 0 día, 1 noche

        // Target global de intensidad
        float baseObjetivo = fNoche * intensidadMax;

        // Cámaras activas
        GetCamsActivas(_tmpCams);

        // Precalcular umbrales al cuadrado
        float noShadowDistSqr = noShadowDist * noShadowDist;
        float cullDistSqr     = cullDist     * cullDist;

        for (int i = 0; i < _estados.Count; i++)
        {
            var s = _estados[i];
            if (!s.l) continue;

            // Distancia vs cámara(s) usando sqrMagnitude (sin sqrt)
            float distSqr = DistSqrARefs(s.l.transform.position, _tmpCams);

            // Sombras LOD
            if (distSqr > noShadowDistSqr)
            {
                if (s.l.shadows != LightShadows.None) s.l.shadows = LightShadows.None;
            }
            else
            {
                if (s.l.shadows != s.baseSombras) s.l.shadows = s.baseSombras;
            }

            // Culling por distancia
            if (distSqr > cullDistSqr)
            {
                if (s.l.enabled) s.l.enabled = false;
                // No actualizamos color/rango si está culleada
                continue;
            }
            else if (!s.l.enabled && baseObjetivo > 0.01f)
            {
                s.l.enabled = true;
            }

            // Objetivo por-luz (intensidad)
            float objetivo = baseObjetivo * s.multInt;

            // Flicker (solo de noche)
            if (flicker && fNoche > 0.001f)
            {
                float ruido = Mathf.PerlinNoise(Time.time * flickerVel, s.seed);
                float factor = 1f + ((ruido - 0.5f) * 2f * flickerAmplitud);
                objetivo *= Mathf.Max(0f, factor);
            }

            // SmoothDamp con tiempos distintos
            bool encendiendo = (objetivo > s.l.intensity);
            float smooth = encendiendo ? suavizadoEncenderSeg : suavizadoApagarSeg;
            s.l.intensity = Mathf.SmoothDamp(s.l.intensity, objetivo, ref s.velInt, Mathf.Max(0.01f, smooth));
            s.l.enabled = (s.l.enabled || s.l.intensity > 0.01f);

            // Color nocturno
            if (controlarColor)
            {
                Color targetColor = Color.Lerp(s.baseColor, colorNoche.Evaluate(fNoche), mezclaColor * fNoche);
                s.l.color = targetColor;
            }

            // Rango nocturno
            if (controlarRango)
            {
                float targetRange = Mathf.Lerp(s.baseRango, rangoNoche * s.multRango, fNoche);
                s.l.range = Mathf.SmoothDamp(s.l.range, targetRange, ref s.velRango, 0.25f);
            }

            _estados[i] = s;
        }

        // Emission de materiales
        if (emisores.Count > 0 && _mpbs.Count == emisores.Count)
        {
            float e = fNoche * emisionMax;
            Color emColor = colorEmisionNoche * Mathf.LinearToGammaSpace(e);

            for (int i = 0; i < emisores.Count; i++)
            {
                var r = emisores[i];
                if (!r) continue;
                var mpb = _mpbs[i];
                mpb.SetColor("_EmissionColor", emColor);
                r.SetPropertyBlock(mpb);
                if (actualizarGI) DynamicGI.SetEmissive(r, emColor);
            }
        }
    }

    // -------- Helpers de cámaras --------
    bool CamActiva(Camera c)
    {
        return c && c.enabled && c.gameObject.activeInHierarchy && c.targetDisplay == 0;
    }

    void GetCamsActivas(List<Camera> outList)
    {
        outList.Clear();
        if (cameras != null)
        {
            for (int i = 0; i < cameras.Count; i++)
                if (CamActiva(cameras[i])) outList.Add(cameras[i]);
        }
        if (outList.Count == 0)
        {
            var main = Camera.main;
            if (CamActiva(main)) outList.Add(main);
        }
    }

    Vector3 GetRenderPos(Camera c)
    {
        if (!c) return Vector3.zero;
        #if CINEMACHINE
        var brain = c.GetComponent<CinemachineBrain>();
        if (brain && brain.ActiveVirtualCamera != null)
            return brain.ActiveVirtualCamera.State.FinalPosition;
        #endif
        return c.transform.position;
    }

    float DistSqrARefs(Vector3 point, List<Camera> refs)
    {
        if (refs == null || refs.Count == 0) return float.PositiveInfinity;

        if (!unionDeCamaras)
        {
            // Elegir una cámara representativa (la de mayor depth)
            Camera pick = refs[0];
            for (int i = 1; i < refs.Count; i++)
                if (refs[i].depth > pick.depth) pick = refs[i];
            Vector3 p = GetRenderPos(pick);
            return (point - p).sqrMagnitude;
        }
        else
        {
            // Unión: mínima distancia a cualquiera
            float best = float.PositiveInfinity;
            for (int i = 0; i < refs.Count; i++)
            {
                Vector3 p = GetRenderPos(refs[i]);
                float d = (point - p).sqrMagnitude;
                if (d < best) best = d;
            }
            return best;
        }
    }

    // -------- Día/Noche --------
    float ObtenerPorcentajeDelDia()
    {
        if (ciclo) return Mathf.Repeat(ciclo.horaDelDia / 24f, 1f);

        if (sol)
        {
            // Dirección en la que "pega" la luz (muchos rigs usan -forward)
            Vector3 dirLuz = -sol.transform.forward;
            float altura = Vector3.Dot(dirLuz.normalized, Vector3.up); // -1..1
            // Margen alrededor del horizonte para transiciones suaves
            return Mathf.InverseLerp(-0.1f, 0.2f, altura);
        }

        // Sin referencias -> noche (para que se note)
        return 1f;
    }

    // -------- Utilidades/Debug --------
    [ContextMenu("Reaplicar Cookies")]
    void ReaplicarCookies()
    {
        for (int i = 0; i < _estados.Count; i++)
        {
            var s = _estados[i];
            if (s.l && s.soportaCookie)
            {
                s.l.cookie = cookie;
                if (s.l.type == LightType.Spot) s.l.cookieSize = cookieSize;
            }
        }
    }

    [ContextMenu("Reaplicar Variación")]
    void ReaplicarVariacion()
    {
        for (int i = 0; i < _estados.Count; i++)
        {
            var s = _estados[i];
            s.multInt = 1f + Random.Range(-variacionIntensidad, variacionIntensidad);
            s.multRango = 1f + Random.Range(-variacionRango, variacionRango);
            _estados[i] = s;
        }
    }
}
