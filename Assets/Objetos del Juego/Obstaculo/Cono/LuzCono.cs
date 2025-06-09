using UnityEngine;

public class LuzCono : MonoBehaviour
{
    [SerializeField] private Light luz;
    [SerializeField] private float intervalo = 2f; // tiempo en segundos entre cambios

    private float temporizador = 0f;

    private void Start()
    {
        if (luz == null)
        {
            Debug.LogError("No se encontró una luz.");
        }
    }

    private void Update()
    {
        if (luz == null) return;

        temporizador += Time.deltaTime;

        if (temporizador >= intervalo)
        {
            luz.enabled = !luz.enabled; // cambia de estado
            temporizador = 0f;
        }
    }
}
