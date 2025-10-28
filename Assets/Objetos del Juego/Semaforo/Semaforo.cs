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

    [Header("Configuración de detección")]
    [Tooltip("Dirección hacia donde el semáforo detecta vehículos (ej: Vector3.forward)")]
    public Vector3 direccionDeteccion = Vector3.forward;

    [Tooltip("Distancia máxima de detección (metros)")]
    public float rangoDeteccion = 15f;

    [Tooltip("Ángulo de detección en grados")]
    public float anguloDeteccion = 30f;

    [Tooltip("Cantidad de rayos en el abanico")]
    public int cantidadRayos = 10;

    [Header("Infracción")]
    [SerializeField] private Infraccion infraccionPorRojo;

    [SerializeField] private EstadoSemaforo estadoActual;
    [SerializeField] private bool infraccionDetectada = false;
    [Header("Punto de detección")]
    [SerializeField] private Transform puntoDeteccion;

    private void Start()
    {
        StartCoroutine(CicloSemaforo());
    }

    private IEnumerator CicloSemaforo()
    {
        while (true)
        {
            // 🔴 Rojo
            SetEstado(EstadoSemaforo.Rojo, duracionRojo);
            yield return CuentaAtras(duracionRojo);

            // 🟢 Verde
            SetEstado(EstadoSemaforo.Verde, duracionVerde);
            yield return CuentaAtras(duracionVerde);

            // 🟡 Amarillo
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

            if (display != null)
                display.text = Mathf.CeilToInt(temporizador).ToString();

            // Mientras está en rojo, verificamos infracciones
            if (estadoActual == EstadoSemaforo.Rojo)
                DetectarCruceEnRojo();

            yield return null;
        }
    }

    private void SetEstado(EstadoSemaforo nuevoEstado, int duracion)
    {
        estadoActual = nuevoEstado;
        DesactivarTodas();
        infraccionDetectada = false; // Reset cada cambio de luz

        switch (nuevoEstado)
        {
            case EstadoSemaforo.Verde:
                lamparaVerde.SetActive(true);
                if (display != null) display.color = Color.green;
                break;

            case EstadoSemaforo.Amarillo:
                lamparaAmarilla.SetActive(true);
                if (display != null) display.color = Color.yellow;
                break;

            case EstadoSemaforo.Rojo:
                lamparaRoja.SetActive(true);
                if (display != null) display.color = Color.red;
                break;
        }
    }

    private void DesactivarTodas()
    {
        lamparaVerde.SetActive(false);
        lamparaAmarilla.SetActive(false);
        lamparaRoja.SetActive(false);
    }

    // 🚨 Detección de cruce en rojo
    private void DetectarCruceEnRojo()
    {
        if (infraccionDetectada || puntoDeteccion == null) return;

        // Dirección principal del semáforo (editable desde el inspector)
        Vector3 dirBase = transform.TransformDirection(direccionDeteccion.normalized);
        Vector3 origen = puntoDeteccion.position;

        for (int i = 0; i < cantidadRayos; i++)
        {
            float angle = -anguloDeteccion / 2 + (anguloDeteccion / (cantidadRayos - 1)) * i;
            Quaternion rot = Quaternion.Euler(0, angle, 0);
            Vector3 rayDir = rot * dirBase;

            // Lanzamos el rayo
            RaycastHit[] hits = Physics.RaycastAll(origen, rayDir, rangoDeteccion);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent(out Vehiculo vehiculo))
                {
                    // Si el vehículo se mueve (VelocidadActual > 1) durante rojo, es infracción
                    if (vehiculo.VelocidadActual > 1f)
                    {
                        infraccionDetectada = true;
                        RegistrarInfraccion(vehiculo);
                        return; // sale al detectar el primero
                    }
                }
            }
        }
    }




    private void RegistrarInfraccion(Vehiculo vehiculo)
    {
        // Mostrar mensaje en pantalla
        string mensaje = $"Infracción — {infraccionPorRojo.Datos.descripcion}";

        MessageDisplaySystem.instance.ShowMessage(mensaje, 0.6f, 0f,infraccionPorRojo.Datos.imagenInfraccion);

        // Registrar en Supervisor
        if (infraccionPorRojo != null)
            Supervisor.Instance.AgregarInfraccion(infraccionPorRojo);
    }

    // 🔍 Gizmos visuales para depurar
    private void OnDrawGizmosSelected()
    {
        if (puntoDeteccion == null) return;

        Vector3 dirBase = transform.TransformDirection(direccionDeteccion.normalized);
        Vector3 origen = puntoDeteccion.position;

        Gizmos.color = Color.red;
        for (int i = 0; i < cantidadRayos; i++)
        {
            float angle = -anguloDeteccion / 2 + (anguloDeteccion / (cantidadRayos - 1)) * i;
            Quaternion rot = Quaternion.Euler(0, angle, 0);
            Vector3 rayDir = rot * dirBase;

            Gizmos.DrawRay(origen, rayDir * rangoDeteccion);
        }

        // Flecha central azul (dirección de detección)
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(origen, dirBase * rangoDeteccion * 0.5f);
    }

}
