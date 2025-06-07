using UnityEngine;

public class Policia : MonoBehaviour
{
    bool policiaAlfrente = false;

    public bool PoliciaAlFrente
    {
        get { return policiaAlfrente; }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo)
        {
            policiaAlfrente = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo)
        {
            policiaAlfrente = false;

        }
    }


}
