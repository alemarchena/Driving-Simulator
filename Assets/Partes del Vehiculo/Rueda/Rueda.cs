using UnityEngine;

public class Rueda : MonoBehaviour
{
    public float coeficienteFriccion;
    [SerializeField] private bool enElAire = true;
    public bool EnElAire {
        get { return enElAire; } 
    }

[SerializeField] private bool tieneCoeficienteFriccion = false;
    public bool TieneCoeficienteFriccion
    {
        get { return tieneCoeficienteFriccion; }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out DeteccionCalle deteccioncalle);
            
        if( deteccioncalle != null)
        {
            coeficienteFriccion = deteccioncalle.FriccionDinamicaMaterial();
            tieneCoeficienteFriccion = true;
        }
        if (other != null)
        {
            enElAire = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            enElAire = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent(out DeteccionCalle deteccioncalle);

        if (deteccioncalle != null)
        {
            tieneCoeficienteFriccion = false;
        }

        if (other != null)
        {
            enElAire = true;
        }
    }
}
