using UnityEngine;

public class TrainPatrol : MonoBehaviour
{
    public Transform[] checkpoints;  // Lista de checkpoints asignados en el Inspector
    public float speed = 5f;

    private int currentCheckpointIndex = 0;
    private int previousCheckpointIndex = -1;
    private Checkpoint[] checkpointScripts;

    void Start()
    {
        // Obtener los scripts Checkpoint para poder cambiar colores
        checkpointScripts = new Checkpoint[checkpoints.Length];
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpointScripts[i] = checkpoints[i].GetComponent<Checkpoint>();
            if (checkpointScripts[i] != null)
                checkpointScripts[i].SetDefaultColor();
        }

        // Al inicio, poner en verde el primer checkpoint (porque el tren empieza ahí)
        if (checkpointScripts.Length > 0 && checkpointScripts[0] != null)
            checkpointScripts[0].SetActiveColor();
    }

    void Update()
    {
        if (checkpoints.Length == 0)
            return; // No hay checkpoints asignados

        Transform target = checkpoints[currentCheckpointIndex];
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // Si el tren llegó al checkpoint actual
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Cambiar colores: el anterior vuelve a blanco
            if (previousCheckpointIndex != -1 && checkpointScripts[previousCheckpointIndex] != null)
            {
                checkpointScripts[previousCheckpointIndex].SetDefaultColor();
            }

            // El checkpoint actual se pone verde
            if (checkpointScripts[currentCheckpointIndex] != null)
            {
                checkpointScripts[currentCheckpointIndex].SetActiveColor();
            }

            // Actualizar índices para siguiente checkpoint
            previousCheckpointIndex = currentCheckpointIndex;
            currentCheckpointIndex++;
            if (currentCheckpointIndex >= checkpoints.Length)
            {
                currentCheckpointIndex = 0; // ciclo infinito
            }
        }
    }
}
