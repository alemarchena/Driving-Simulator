using UnityEditor.SceneManagement;
using UnityEngine;

public class Cono : Obstaculo
{
    ActaInfraccion actaInfraccion;

    private void Start()
    {
        actaInfraccion = GetComponent<ActaInfraccion>();
    }

   
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo != null && actaInfraccion)
        {
            actaInfraccion.CrearActaInfraccion();
            Destroy(actaInfraccion);
            actaInfraccion = null;
        }
    }
}