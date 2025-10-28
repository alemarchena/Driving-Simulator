using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class SpeedRadar : MonoBehaviour
{
    [SerializeField] Infraccion infraccion;

    [Header("Configuración del Radar Direccional")]
    [Tooltip("Límite de velocidad en km/h")]
    public float speedLimit = 60f;
    [Tooltip("Distancia máxima de detección (metros)")]
    public float detectionRange = 50f;
    [Tooltip("Ángulo del cono de detección en grados")]
    public float detectionAngle = 45f;
    [Tooltip("Cantidad de rayos para formar el cono")]
    public int rayCount = 15;

    [Header("Dirección del Radar (editable en el Editor)")]
    [Tooltip("Vector local que indica hacia dónde apunta el radar")]
    public Vector3 direction = Vector3.forward;

    [Header("UI del Radar")]
    [SerializeField] Image fondoDisplay;
    [SerializeField] TextMeshProUGUI speedLabel;

    private bool detected = false;

    private void Start()
    {
        ClearDisplay();
    }

    private void ClearDisplay()
    {
        if (speedLabel != null) speedLabel.text = string.Empty;
        if (fondoDisplay != null) fondoDisplay.color = Color.grey;
    }

    private void Update()
    {
        DetectarVehiculosConRaycastAll();
    }

    private void DetectarVehiculosConRaycastAll()
    {
        bool algoDetectado = false;

        // Dirección base editable
        Vector3 dirBase = transform.TransformDirection(direction.normalized);

        // Dispara múltiples rayos dentro del cono
        for (int i = 0; i < rayCount; i++)
        {
            float angle = -detectionAngle / 2 + (detectionAngle / (rayCount - 1)) * i;
            Quaternion rot = Quaternion.Euler(0, angle, 0);
            Vector3 rayDir = rot * dirBase;

            // Usa RaycastAll: devuelve TODOS los hits en ese rayo
            RaycastHit[] hits = Physics.RaycastAll(transform.position, rayDir, detectionRange);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent(out Vehiculo vehiculo))
                {
                    algoDetectado = true;

                    if (vehiculo.VelocidadActual > speedLimit && !detected)
                    {
                        detected = true;
                        MostrarInfraccion(vehiculo);
                        StartCoroutine(RestoreDetected());
                        break; // no procesar más hits por este frame
                    }
                    else if (!detected)
                    {
                        MostrarVelocidadNormal(vehiculo);
                    }
                }
            }
        }

        if (!algoDetectado && !detected)
        {
            ClearDisplay();
        }
    }

    private void MostrarInfraccion(Vehiculo vehiculo)
    {
        if (speedLabel != null)
            speedLabel.text = vehiculo.VelocidadActual.ToString("F0");

        if (fondoDisplay != null)
            fondoDisplay.color = Color.red;

        string mensaje = $"Falta {infraccion.Datos.TypeInfraccion} {infraccion.Datos.descripcion} — " +
                         $"Pasaste a {vehiculo.VelocidadActual:F0} km/h";

        MessageDisplaySystem.instance.ShowMessage(mensaje, 0.6f, 0f, infraccion.Datos.gestoInfraccion,infraccion.Datos.imagenInfraccion);

        if (infraccion != null)
            Supervisor.Instance.AgregarInfraccion(infraccion);
    }

    private void MostrarVelocidadNormal(Vehiculo vehiculo)
    {
        if (speedLabel != null)
            speedLabel.text = vehiculo.VelocidadActual.ToString("F0");

        if (fondoDisplay != null)
            fondoDisplay.color = Color.green;
    }

    IEnumerator RestoreDetected()
    {
        yield return new WaitForSeconds(5f);
        detected = false;
        ClearDisplay();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 dirBase = transform.TransformDirection(direction.normalized);

        // Rayos del cono
        Gizmos.color = Color.red;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = -detectionAngle / 2 + (detectionAngle / (rayCount - 1)) * i;
            Quaternion rot = Quaternion.Euler(0, angle, 0);
            Vector3 rayDir = rot * dirBase;
            Gizmos.DrawRay(transform.position, rayDir * detectionRange);
        }

        // Dirección base
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, dirBase * detectionRange * 0.5f);
    }
}
