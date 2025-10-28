using UnityEngine;

public class DetectorInfraccion : MonoBehaviour
{

    [SerializeField] ActaInfraccion actaInfraccion;

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo != null && actaInfraccion)
        {
            actaInfraccion.CrearActaInfraccion();
            Destroy(actaInfraccion);

            MessageDisplaySystem.instance.ShowMessage("Infraccion : " + actaInfraccion.Infraccion.Datos.nombre, 1f, 0f, actaInfraccion.Infraccion.Datos.imagenInfraccion);
        }
    }
}
