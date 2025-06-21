using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody), typeof(RutaWayPoint))]
public class NPCMovement : MonoBehaviour
{
    [SerializeField] RutaWayPoint ruta;
    public float speed = 3f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 0.2f;

    private Rigidbody rb;
    private int currentIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (ruta.Waypoints.Length == 0)
            Debug.LogError("No hay puntos asignados al NPC.");
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
            return;
        }

        // Movimiento hacia adelante
        Vector3 move = flatDirection.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotación suave hacia el punto
        if (flatDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(flatDirection);
            Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothedRotation);
        }
    }
}
