using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DeteccionCalle : MonoBehaviour
{
    [SerializeField] string superficieActual;
    private PhysicsMaterial materialFisico;

    public float FriccionDinamicaMaterial()
    {
        return materialFisico.dynamicFriction;
    }
    public float FriccionEstaticaMaterial()
    {
        return materialFisico.staticFriction;
    }
  
    public float ReboteMaterial()
    {
        return materialFisico.bounciness;
    }
    private void Start()
    {
        if (GetComponent<Collider>().material != null)
        {
            materialFisico = GetComponent<Collider>().material;
        }
        else
        {
            Debug.LogWarning("Este objeto no tiene un Physic Material asignado en su Collider.");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        collision.gameObject.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo)
        {
            Debug.Log("Estas en un piso de " + superficieActual + materialFisico.dynamicFriction);
        }
    }
}
