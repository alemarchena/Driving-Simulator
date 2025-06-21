using UnityEngine;

public class RutaWayPoint : MonoBehaviour
{
    [SerializeField] Transform[] waypoints; // Lista de puntos a recorrer

    public Transform[] Waypoints {
        get { 
            return waypoints;
        }
    }
}
