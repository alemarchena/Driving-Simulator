using UnityEngine;

public class Boing : MonoBehaviour
{
    public float speed = 3f;                      // Velocidad de avance
    public float jumpHeight = 1f;                 // Altura del rebote
    public float jumpFrequency = 4f;              // Frecuencia del rebote
    public float maxDistance = 5f;                // Distancia máxima a recorrer
    public Vector3 moveDirection = Vector3.forward; // Dirección de movimiento

    private Vector3 startPos;
    private bool isMoving = true;

    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rbPersonaje;
    [SerializeField] Collider rbCollider;
    [SerializeField] Rigidbody rbTabla;
    void Start()
    {
        startPos = transform.position; // Guardamos posición inicial
        moveDirection = moveDirection.normalized;
    }

    void Update()
    {
        if (!isMoving) return;

        // Calcular cuánto se movió desde el inicio
        float traveled = Vector3.Distance(startPos, transform.position);

        if (traveled >= maxDistance)
        {
            isMoving = false;
            if (animator != null) {
                animator.SetTrigger("Stop");
            }

            if (rbPersonaje != null) {
                rbCollider.isTrigger = false;
                rbPersonaje.useGravity = true;
            }
            if(rbTabla != null)
            {
                rbTabla.useGravity = true;

            }
            return;
        }

        // Avance horizontal
        Vector3 move = moveDirection * speed * Time.deltaTime;
        transform.position += move;

        // Rebote vertical
        float bounce = Mathf.Abs(Mathf.Sin(Time.time * jumpFrequency)) * jumpHeight;
        Vector3 newPos = transform.position;
        newPos.y = startPos.y + bounce;
        transform.position = newPos;
    }
}
