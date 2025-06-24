using UnityEngine;

public class Cono : Obstaculo
{
    ActaInfraccion actaInfraccion;

    private void Start()
    {
        actaInfraccion = GetComponent<ActaInfraccion>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo != null && actaInfraccion)
        {
            actaInfraccion.CrearActaInfraccion();
        }
    }
}