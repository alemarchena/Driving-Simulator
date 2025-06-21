using UnityEngine;

public class NPCZigZag : MonoBehaviour
{
    public float speed = 3f;                   // Velocidad general del movimiento
    public float zigzagAngle = 1f;             // Qué tan pronunciado es el zigzag (valor positivo = más inclinación)
    public float zigzagInterval = 0.5f;        // Cada cuánto tiempo cambia de dirección (en segundos)
    public Vector3 forwardDirection = Vector3.forward; // Dirección principal (ej: adelante en Z)

    private float timer;
    private int direction = 1; // Cambia entre 1 y -1 para alternar izquierda/derecha

    void Update()
    {
        timer += Time.deltaTime;

        // Cambiamos de dirección cada "zigzagInterval" segundos
        if (timer >= zigzagInterval)
        {
            direction *= -1; // Alterna entre 1 y -1
            timer = 0f;
        }

        // Calculamos la dirección en zigzag:
        // - Se mueve hacia adelante (ej: Z) + un poco a la derecha o izquierda (ej: X)
        Vector3 moveDir = forwardDirection.normalized + (Vector3.right * zigzagAngle * direction);
        moveDir.Normalize(); // Asegura que la velocidad sea constante sin importar el ángulo

        // Movimiento
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
    }
}
