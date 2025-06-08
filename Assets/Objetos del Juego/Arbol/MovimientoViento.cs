using UnityEngine;

public class MovimientoViento : MonoBehaviour
{
    public float amplitud = 5f;     // Qué tanto se inclina (grados)
    public float frecuencia = 1f;   // Qué tan rápido oscila (veces por segundo)

    private Quaternion rotacionInicial;

    void Start()
    {
        rotacionInicial = transform.localRotation;
    }

    void Update()
    {
        float angulo = Mathf.Sin(Time.time * frecuencia) * amplitud;
        transform.localRotation = rotacionInicial * Quaternion.Euler(0f, 0f, angulo);
    }
}
