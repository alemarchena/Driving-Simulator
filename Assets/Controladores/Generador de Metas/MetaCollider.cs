using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class MetaCollider : MonoBehaviour
{

    [SerializeField] Meta meta;

    public Meta Meta => meta;

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo != null)
        {
            meta.Resolve();
        }
    }
}
