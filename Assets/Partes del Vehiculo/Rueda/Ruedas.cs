using System.Collections.Generic;
using UnityEngine;

public class Ruedas : MonoBehaviour
{
    [SerializeField] List<Rueda> listaRuedas;
    [SerializeField] private float coeficienteFriccion = 0.3f; // ajustable en el Inspector
    [SerializeField] private float coeficienteFriccionActual;

    private bool enElAire = false;

    public bool EnElAire
    {
        get { return enElAire; }
    }
    public float CoeficienteFriccionActual
    {
        get { return coeficienteFriccionActual; }
    }
    private void Start()
    {
        AsignaRuedas();
    }
    private void AsignaRuedas()
    {
        if (listaRuedas.Count <= 0)
        {
            Debug.LogError("Falta asignar las ruedas al vehículo");
            return;
        }
        foreach (Rueda rueda in listaRuedas)
        {
            rueda.coeficienteFriccion = coeficienteFriccionActual;
        }
    }

    private void Update()
    {
        VerificaCoeficienteFriccionRuedas();
        VerificaSiEstaEnElAire();
    }
    private float VerificaCoeficienteFriccionRuedas()
    {
        float totalCoeficiente = coeficienteFriccion;
        int cantidadRuedasConCoeficiente = 0;

        foreach (Rueda rueda in listaRuedas)
        {
            if (rueda.TieneCoeficienteFriccion)
            {
                totalCoeficiente += rueda.coeficienteFriccion;
            }
            else
            {
                totalCoeficiente += coeficienteFriccion;
            }
            cantidadRuedasConCoeficiente += 1;
        }

        totalCoeficiente /= cantidadRuedasConCoeficiente;
        coeficienteFriccionActual = totalCoeficiente;

        return coeficienteFriccionActual;

    }
    public void VerificaSiEstaEnElAire()
    {
        int contadorRuedas=0;

        foreach (Rueda rueda in listaRuedas)
        {
            if (rueda.EnElAire == true)
            {
                contadorRuedas += 1;
            }
        }

        if (contadorRuedas > 2)
        { enElAire = true;
        }else
        { enElAire = false; 
        }
    }
}
