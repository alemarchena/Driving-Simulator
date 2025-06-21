using UnityEngine;

public class NPCOneWayStop : MonoBehaviour
{
    public Vector2 direction = Vector2.right; // Dirección del movimiento
    public float speed = 3f;                  // Velocidad del cubo
    public float moveDistance = 5f;           // Distancia máxima que se moverá

    private Vector3 startPos;
    private bool isMoving = true;

    void Start()
    {
        // Guardamos la posición inicial del cubo
        startPos = transform.position;
    }

    void Update()
    {
        if (!isMoving) return;

        // Movimiento en la dirección dada
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        // Calculamos la distancia recorrida desde el punto inicial
        float distanceMoved = Vector3.Distance(transform.position, startPos);

        // Si superó o alcanzó la distancia límite, lo detenemos
        if (distanceMoved >= moveDistance)
        {
            isMoving = false;

            // Opcional: fijamos posición exacta al límite (para evitar errores por deltaTime)
            Vector3 stopPos = startPos + (Vector3)(direction.normalized * moveDistance);
            transform.position = stopPos;
        }
    }
}
