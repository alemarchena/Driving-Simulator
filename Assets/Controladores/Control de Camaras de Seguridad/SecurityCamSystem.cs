using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SecurityCamSystem : MonoBehaviour
{
    [Header("Cámaras de seguridad (colócalas por el mapa)")]
    public Camera[] cams;

    [Header("Pantallas (modo manual o automático)")]
    [Tooltip("Si completás 'screens', el sistema usará esas pantallas tal cual.")]
    public RawImage[] screens; // MODO MANUAL (opcional)

    [Header("Modo automático (mural en grid)")]
    [Tooltip("Si activás esto, el sistema creará un RawImage por cámara dentro de gridParent.")]
    public bool autoBuildGrid = false;
    [Tooltip("RectTransform (Canvas World Space o un Panel) que contendrá el grid")]
    public RectTransform gridParent;
    [Tooltip("Cantidad de columnas en el mural")]
    public int columns = 3;
    [Tooltip("Tamaño de cada celda (16:9 recomendado)")]
    public Vector2 cellSize = new Vector2(400, 225);
    public Vector2 spacing = new Vector2(8, 8);

    [Header("Opcional (si hay 1 pantalla)")]
    public bool autoCycle = true;
    [Min(0.5f)] public float cycleSeconds = 3f;

    [Header("RenderTexture")]
    [Tooltip("Resolución base de cada cámara")]
    public Vector2Int renderResolution = new Vector2Int(1024, 576);
    public int depthBuffer = 16;

    // Internos
    RenderTexture[] rts;
    readonly List<RawImage> autoViews = new();
    int current = 0;
    float t = 0f;

    // IMPORTANTES: ahora el Grid va en monitorsRoot (no en gridParent)
    GridLayoutGroup grid;       
    RectTransform monitorsRoot; 

    void Awake()
    {
        if (cams == null || cams.Length == 0)
        {
            Debug.LogWarning("[SecurityCamSystem] No hay cámaras asignadas.");
            enabled = false;
            return;
        }

        // Crear RTs y preparar cámaras
        rts = new RenderTexture[cams.Length];
        for (int i = 0; i < cams.Length; i++)
        {
            if (!cams[i]) continue;

            var rt = new RenderTexture(renderResolution.x, renderResolution.y, depthBuffer, RenderTextureFormat.ARGB32);
            rt.name = $"RT_Cam_{i}";
            rts[i] = rt;

            cams[i].targetTexture = rt;
            cams[i].enabled = false;

            var al = cams[i].GetComponent<AudioListener>();
            if (al) al.enabled = false;
        }

        // Grid automático (seguro, y esta vez en monitorsRoot)
        if (autoBuildGrid)
        {
            if (!gridParent)
            {
                Debug.LogError("[SecurityCamSystem] autoBuildGrid = true pero no asignaste gridParent.");
            }
            else
            {
                // Crear/usar contenedor propio que ocupará TODO el gridParent
                monitorsRoot = gridParent.Find("__Monitors__") as RectTransform;
                if (!monitorsRoot)
                {
                    var go = new GameObject("__Monitors__", typeof(RectTransform));
                    monitorsRoot = go.GetComponent<RectTransform>();
                    monitorsRoot.SetParent(gridParent, false);
                    monitorsRoot.anchorMin = Vector2.zero;
                    monitorsRoot.anchorMax = Vector2.one;
                    monitorsRoot.offsetMin = Vector2.zero;
                    monitorsRoot.offsetMax = Vector2.zero;
                }

                // Asegurar que EL GRID esté en monitorsRoot (NO en gridParent)
                grid = monitorsRoot.GetComponent<GridLayoutGroup>();
                if (!grid) grid = monitorsRoot.gameObject.AddComponent<GridLayoutGroup>();
                grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                grid.constraintCount = Mathf.Max(1, columns);
                grid.cellSize = cellSize;
                grid.spacing = spacing;
                grid.childAlignment = TextAnchor.UpperLeft;
                grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
                grid.startAxis = GridLayoutGroup.Axis.Horizontal;

                // Limpiar SOLO lo nuestro (los hijos de monitorsRoot)
                for (int i = monitorsRoot.childCount - 1; i >= 0; i--)
                    Destroy(monitorsRoot.GetChild(i).gameObject);

                autoViews.Clear();
                for (int i = 0; i < cams.Length; i++)
                {
                    if (!cams[i]) continue;
                    var go = new GameObject($"Monitor_{i}", typeof(RectTransform), typeof(RawImage));
                    go.transform.SetParent(monitorsRoot, false); // hijo DIRECTO del grid
                    var ri = go.GetComponent<RawImage>();
                    // Tamaño explícito por si el layout tarda 1 frame
                    var rt = (RectTransform)ri.transform;
                    rt.sizeDelta = cellSize;
                    ri.texture = null; // se setea al encender
                    autoViews.Add(ri);
                }
            }
        }

        // Arranca apagado (lo prenderá el trigger)
        SetActive(false);
    }

    public void SetActive(bool on)
    {
        // Enciendo/apago cámaras
        foreach (var cam in cams)
            if (cam) cam.enabled = on;

        // Pantallas (auto o manual)
        if (autoBuildGrid && autoViews.Count > 0)
        {
            for (int i = 0; i < cams.Length; i++)
            {
                if (i < autoViews.Count && autoViews[i] && cams[i])
                    autoViews[i].texture = on ? cams[i].targetTexture : null;
            }
            // No necesitamos Update para grid
            enabled = false;
        }
        else if (screens != null && screens.Length > 0)
        {
            if (screens.Length == 1)
            {
                if (screens[0]) screens[0].texture = on ? rts[current] : null;
                t = 0f;
                enabled = on && autoCycle && cams.Length > 1;
            }
            else
            {
                for (int i = 0; i < screens.Length; i++)
                {
                    int camIdx = i % cams.Length;
                    if (screens[i])
                        screens[i].texture = on ? rts[camIdx] : null;
                }
                enabled = false;
            }
        }
        else
        {
            enabled = false;
            if (on)
                Debug.LogWarning("[SecurityCamSystem] SetActive(true) pero no hay pantallas (screens vacío y autoBuildGrid desactivado).");
        }

        // --- Diagnóstico útil ---
        Debug.Log($"[System] on={on} | autoBuildGrid={autoBuildGrid} | screens={(screens != null ? screens.Length : 0)}");
        if (autoBuildGrid)
        {
            Debug.Log($"[System] monitorsRoot={(monitorsRoot ? monitorsRoot.name : "null")} | gridOnRoot={(grid != null)} | views={autoViews.Count}");
        }
    }

    void Update()
    {
        // Ciclo automático solo si hay 1 pantalla manual
        if (!autoCycle || screens == null || screens.Length != 1 || cams.Length <= 1) return;

        t += Time.deltaTime;
        if (t >= cycleSeconds)
        {
            t = 0f;
            current = (current + 1) % rts.Length;
            if (screens[0]) screens[0].texture = rts[current];
        }
    }

    // (Opcional) reconstruir el mural si cambiás cams en runtime
    public void RebuildGrid()
    {
        if (!autoBuildGrid || !monitorsRoot) return;

        for (int i = monitorsRoot.childCount - 1; i >= 0; i--)
            Destroy(monitorsRoot.GetChild(i).gameObject);

        autoViews.Clear();
        for (int i = 0; i < cams.Length; i++)
        {
            if (!cams[i]) continue;
            var go = new GameObject($"Monitor_{i}", typeof(RectTransform), typeof(RawImage));
            go.transform.SetParent(monitorsRoot, false);
            var ri = go.GetComponent<RawImage>();
            ((RectTransform)ri.transform).sizeDelta = cellSize;
            autoViews.Add(ri);
        }
    }

    void OnDestroy()
    {
        if (rts != null)
        {
            for (int i = 0; i < rts.Length; i++)
                if (rts[i]) rts[i].Release();
        }
    }
}
