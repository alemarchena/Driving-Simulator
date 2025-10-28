using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    [Header("Tiempo")]
    [Range(0, 24)] public float horaDelDia = 12f;
    public float duracionDelDia = 60f;

    [Header("Luz Solar")]
    public Light sol;
    public Gradient colorDeLuz;
    public AnimationCurve intensidadDeLuz;
    public Vector3 rotacionExtra = new Vector3(0, 0, 0);

    float velocidad => 24f / duracionDelDia;

    void Update()
    {
        horaDelDia += Time.deltaTime * velocidad;
        if (horaDelDia > 24f) horaDelDia -= 24f;

        ActualizarLuz();
    }

    void ActualizarLuz()
    {
        float porcentaje = horaDelDia / 24f;

        float angulo = (porcentaje * 360f) - 90f;
        sol.transform.rotation = Quaternion.Euler(angulo, rotacionExtra.y, rotacionExtra.z);

        sol.color = colorDeLuz.Evaluate(porcentaje);
        sol.intensity = intensidadDeLuz.Evaluate(porcentaje);

        RenderSettings.ambientLight = sol.color * 0.5f;
    }
}
