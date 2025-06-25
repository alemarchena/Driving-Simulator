using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody), typeof(RutaWayPoint))]
public class NPCMovement : MonoBehaviour
{
    [SerializeField] RutaWayPoint ruta;
    public float speed = 3f; // velocidad base
    public float rotationSpeed = 5f;
    public float stoppingDistance = 0.2f;

    private Rigidbody rb;
    private int currentIndex = 0;
    private float velocidadActual;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (ruta.Waypoints.Length == 0)
        {
            Debug.LogError("No hay puntos asignados al NPC.");
            return;
        }

        GenerarNuevaVelocidad(); // velocidad inicial aleatoria
    }

    void FixedUpdate()
    {
        if (ruta.Waypoints.Length == 0) return;

        Vector3 targetPos = ruta.Waypoints[currentIndex].position;
        Vector3 direction = (targetPos - transform.position);
        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);

        if (flatDirection.magnitude < stoppingDistance)
        {
            currentIndex = (currentIndex + 1) % ruta.Waypoints.Length;
            GenerarNuevaVelocidad(); // cada vez que llega a un nuevo punto, cambia la velocidad
            return;
        }

        // Movimiento hacia adelante
        Vector3 move = flatDirection.normalized * velocidadActual * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotación suave hacia el punto
        if (flatDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(flatDirection);
            Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothedRotation);
        }
    }

    void GenerarNuevaVelocidad()
    {
        float min = speed * 0.9f;
        float max = speed * 1.1f;
        velocidadActual = Random.Range(min, max);
    }
}
