using UnityEngine;

public class BarreraController : MonoBehaviour
{
    public Vector3 upPosition;   // Posición cuando la barrera está levantada
    public Vector3 downPosition; // Posición cuando la barrera está bajada
    public float speed = 2f;     // Velocidad de movimiento

    private bool isMovingDown = false;

    void Update()
    {
        // Mover la barrera hacia la posición correspondiente
        if (isMovingDown)
        {
            transform.position = Vector3.MoveTowards(transform.position, downPosition, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, upPosition, speed * Time.deltaTime);
        }
    }

    public void SubirBarrera()
    {
        isMovingDown = false;
    }

    public void BajarBarrera()
    {
        isMovingDown = true;
    }
}
