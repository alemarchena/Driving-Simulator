using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SpeedRadar : MonoBehaviour
{

    [Header("Configuración de Radar Direccional")]
    public float speedLimit = 60f;       // km/h
    public float detectionRange = 20f;   // Distancia antes del radar (20 o 50 metros)
    public float detectionAngle = 45f;      // Ángulo del cono de detección

    [Space]
    [SerializeField] Image fondoDisplay;
    [SerializeField] TextMeshProUGUI speedLabel;


    private Dictionary<GameObject, Vector3> entryPositions = new();
    private Dictionary<GameObject, float> entryTimes = new();

    private SphereCollider sphere;

    private void Start()
    {
        ClearDisplay();
    }

    private void ClearDisplay()
    {
        speedLabel.text = string.Empty;
        fondoDisplay.color = Color.grey;
    }
    private void OnValidate()
    {
        sphere = GetComponent<SphereCollider>();
        if (sphere != null)
        {
            sphere.isTrigger = true;
            sphere.radius = detectionRange;
        }
    }
    private void Update()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider col in nearby)
        {
            if (col.TryGetComponent(out Vehiculo vehiculo))
            {
                Vector3 toTarget = (col.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, toTarget);

                if (angleToTarget <= detectionAngle / 2f)
                {
                    if (!entryPositions.ContainsKey(col.gameObject))
                    {
                        entryPositions[col.gameObject] = col.transform.position;
                        entryTimes[col.gameObject] = Time.time;

                        //Debug.Log($"Vehículo detectado dentro del radar: {col.name}");
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (entryPositions.ContainsKey(other.gameObject))
        {
            if (other.gameObject.TryGetComponent(out Vehiculo vehiculo))
            {
                if (vehiculo != null)
                {
                    if (vehiculo.VelocidadActual > speedLimit)
                    {
                        speedLabel.text = vehiculo.VelocidadActual.ToString();
                        fondoDisplay.color = Color.red;
                    }else
                    {
                        fondoDisplay.color = Color.green;
                    }
                }
            }else
            {
                fondoDisplay.color = Color.grey;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (entryPositions.ContainsKey(other.gameObject))
        {
            if (other.gameObject.TryGetComponent(out Vehiculo vehiculo))
            {
                if (vehiculo != null)
                {
                    if (vehiculo.VelocidadActual > speedLimit)
                    {
                        Debug.LogWarning("⚠️ Multa por exceso de velocidad");
                        Debug.LogWarning("⚠️ Velocidad " + vehiculo.VelocidadActual);
                        // Aplica lógica de multa aquí
                    }
                }
                ClearDisplay();
            }

            entryPositions.Remove(other.gameObject);
            entryTimes.Remove(other.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Vector3 forward = transform.forward;

        int segments = 30;
        for (int i = 0; i <= segments; i++)
        {
            float angle = -detectionAngle / 2 + detectionAngle * i / segments;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * forward;
            Gizmos.DrawLine(transform.position, transform.position + direction * detectionRange);
        }

        // También se puede usar una semiesfera base
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }


}
