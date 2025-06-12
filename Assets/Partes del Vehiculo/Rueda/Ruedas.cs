using System.Collections.Generic;
using UnityEngine;

public class Ruedas : MonoBehaviour
{
    [SerializeField] List<Rueda> listaRuedas;
    [SerializeField] private float coeficienteFriccion = 0.3f; // ajustable en el Inspector
    [SerializeField] private float coeficienteFriccionActual;

    [SerializeField] private bool enElAire = false;
    [SerializeField] private bool todasRuedasenElAire = false;
    [SerializeField] float empujeEnElAire;
    [SerializeField] float empuje1ruedasEnAire=100;
    [SerializeField] float empuje2ruedasEnAire=200;
    [SerializeField] float empuje3ruedasEnAire=300;
    [SerializeField] float empuje4ruedasEnAire=400;

    public float EmpujeEnElAire { 
        get { return empujeEnElAire; }
    }
    public bool EnElAire
    {
        get { return enElAire; }
    }
    public bool TodasRuedasEnElAire
    {
        get { return todasRuedasenElAire; }
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
        todasRuedasenElAire = false;

        switch (contadorRuedas)
        {
            case 1:
                enElAire = true;
                empujeEnElAire = empuje1ruedasEnAire;
                todasRuedasenElAire = false;

                break;

            case 2:
                enElAire = true;
                empujeEnElAire = empuje2ruedasEnAire;

                break;

            case 3:
                enElAire = true;
                empujeEnElAire = empuje3ruedasEnAire;
                break;

            case 4:
                enElAire = true;
                empujeEnElAire = empuje4ruedasEnAire;
                todasRuedasenElAire = true;
                break;

            default:
                enElAire = false;
                empujeEnElAire = 0f;

                break;
        }



    }
}
