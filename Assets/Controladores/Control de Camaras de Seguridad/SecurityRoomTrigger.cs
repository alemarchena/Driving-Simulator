using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SecurityRoomTrigger : MonoBehaviour
{
    public SecurityCamSystem system;
    public string playerTag;

    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out Vehiculo vehiculo);
        if (vehiculo != null) { 
        
            if (!system) Debug.LogError("[Trigger] system NO asignado");
            else
            {
                Debug.Log("[Trigger] SetActive(TRUE)");
                system.SetActive(true);
            }
        }
       
    }

    void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo != null && system)
        {
            system.SetActive(false);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,1,0,0.2f);
        var col = GetComponent<Collider>() as BoxCollider;
        if (col)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(col.center, col.size);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(col.center, col.size);
        }
    }
#endif
}
