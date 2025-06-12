using UnityEngine;

public class Rueda : MonoBehaviour
{
    public float coeficienteFriccion;
    [SerializeField] private bool enElAire = false;
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
            enElAire = false;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        other.gameObject.TryGetComponent(out DeteccionCalle deteccioncalle);

        if (deteccioncalle != null)
        {
            tieneCoeficienteFriccion = false;
            enElAire = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent(out DeteccionCalle deteccioncalle);

        if (deteccioncalle != null)
        {
            tieneCoeficienteFriccion = false;
            enElAire = true;
        }

    }
}
