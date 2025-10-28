using UnityEngine;

public class FluidTrainMovement : MonoBehaviour
{
    [Header("Waypoints Configuration")]
    public Transform[] waypoints;

    [Header("Movement Settings")]
    public float speed = 1f;
    public float rotationSpeed = 1f;
    public float bankingAmount = 1f;
    public float turnRadius = 1f;

    [Header("Pivot Settings")]
    public float frontOffset = 2.5f; // Distancia hacia adelante desde el centro del tren

    private int targetIndex = 0;
    private Vector3 lastDirection;

    // Valores finales después de iniciar
    private float finalSpeed = 4f;
    private float finalRotationSpeed = 6f;
    private float finalBankingAmount = 2f;
    private float finalTurnRadius = 3.5f;

    void Start()
    {
        if (waypoints.Length > 0)
            lastDirection = (waypoints[0].position - transform.position).normalized;

        // Cambiar a los valores finales después de 2 segundos
        Invoke(nameof(ApplyFinalSettings), 2f);
    }

    void ApplyFinalSettings()
    {
        speed = finalSpeed;
        rotationSpeed = finalRotationSpeed;
        bankingAmount = finalBankingAmount;
        turnRadius = finalTurnRadius;
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[targetIndex];
        Vector3 rawDirection = (target.position - transform.position).normalized;
        Vector3 direction = Vector3.Lerp(lastDirection, rawDirection, Time.deltaTime * rotationSpeed).normalized;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);

            float angle = Vector3.SignedAngle(lastDirection, direction, Vector3.up);
            float bankAngle = Mathf.Abs(angle) > 1f ? Mathf.Clamp(angle * bankingAmount, -bankingAmount, bankingAmount) : 0f;
            targetRotation *= Quaternion.Euler(0, 0, -bankAngle);

            Vector3 frontPivot = transform.position + transform.forward * frontOffset;
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            float angleDifference = Quaternion.Angle(transform.rotation, newRotation);

            transform.RotateAround(frontPivot, Vector3.up, angleDifference);
            transform.rotation = newRotation;

            lastDirection = direction;
        }

        if (Vector3.Distance(transform.position, target.position) < turnRadius)
        {
            targetIndex = (targetIndex + 1) % waypoints.Length;
        }

        Debug.DrawRay(transform.position, direction * 3f, Color.green);
    }
}
