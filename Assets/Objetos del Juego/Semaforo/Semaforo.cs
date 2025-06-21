using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Semaforo : MonoBehaviour
{
    [SerializeField] private Text display;
    public enum EstadoSemaforo { Verde, Amarillo, Rojo }

    [Header("Duraciones (segundos)")]
    public int duracionVerde = 5;
    public int duracionAmarillo = 2;
    public int duracionRojo = 5;

    [Header("Objetos Lámpara")]
    [SerializeField] private GameObject lamparaVerde;
    [SerializeField] private GameObject lamparaAmarilla;
    [SerializeField] private GameObject lamparaRoja;

    private EstadoSemaforo estadoActual;

    private void Start()
    {
        // Inicia el ciclo del semáforo en rojo
        StartCoroutine(CicloSemaforo());
    }

    private IEnumerator CicloSemaforo()
    {
        while (true)
        {
            // Rojo
            SetEstado(EstadoSemaforo.Rojo, duracionRojo);
            yield return CuentaAtras(duracionRojo);

            // Verde
            SetEstado(EstadoSemaforo.Verde, duracionVerde);
            yield return CuentaAtras(duracionVerde);

            // Amarillo
            SetEstado(EstadoSemaforo.Amarillo, duracionAmarillo);
            yield return CuentaAtras(duracionAmarillo);
        }
    }

    private IEnumerator CuentaAtras(int duracion)
    {
        float temporizador = duracion;
        while (temporizador > 0f)
        {
            temporizador -= Time.deltaTime;
            if(display!=null)
                display.text = Mathf.CeilToInt(temporizador).ToString();
            yield return null;
        }
    }

    /// <summary>
    /// Ajusta las lámparas según el estado, actualiza el color del texto y activa solo la lámpara correspondiente.
    /// </summary>
    private void SetEstado(EstadoSemaforo nuevoEstado, int duracion)
    {
        estadoActual = nuevoEstado;
        DesactivarTodas();

        switch (nuevoEstado)
        {
            case EstadoSemaforo.Verde:
                lamparaVerde.SetActive(true);
                if (display != null)
                    display.color = Color.green;
                break;
            case EstadoSemaforo.Amarillo:
                lamparaAmarilla.SetActive(true);
                if (display != null)
                    display.color = Color.yellow;
                break;
            case EstadoSemaforo.Rojo:
                lamparaRoja.SetActive(true);
                if (display != null)
                    display.color = Color.red;
                break;
        }
    }

    /// <summary>
    /// Desactiva todas las lámparas.
    /// </summary>
    private void DesactivarTodas()
    {
        lamparaVerde.SetActive(false);
        lamparaAmarilla.SetActive(false);
        lamparaRoja.SetActive(false);
    }
}
