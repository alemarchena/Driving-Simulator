using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Embrague : MonoBehaviour
{
    private enum EstadoEmbrague { Presionado, Suelto }

    [SerializeField] private Creadores creador = Creadores.Sabatini_Cialone_Santino;

    private EstadoEmbrague estadoActual = EstadoEmbrague.Suelto;
    [SerializeField] private bool estaPresionado;
    public bool EstaPresionado => estaPresionado;

    [SerializeField] private KeyCode teclaMouse;

   
    private void Awake()
    {
        creador = Creadores.Sabatini_Cialone_Santino;
    }

    private void Start()
    {
        teclaMouse = ControladorComandos.instance.TeclaEspecial(ControladorComandos.TeclasEspeciales.MouseDerecho);
    }
    private bool SeMantienePresionadaLaTecla()
    {
        bool sePresionoTecla = false;
        
        if (Input.GetKey(teclaMouse))
        {
            sePresionoTecla = true;
        }
        
        return sePresionoTecla;
    }

    private void Update()
    {
        if (SeMantienePresionadaLaTecla())
        {
            if (estadoActual != EstadoEmbrague.Presionado)
            {
                estadoActual = EstadoEmbrague.Presionado;
                estaPresionado = true;
            }
        }
        else
        {
            if (estadoActual != EstadoEmbrague.Suelto)
            {
                estadoActual = EstadoEmbrague.Suelto;
                estaPresionado = false;
            }
        }
    }

    public string Describir
    {
        get
        {
            string descripcion = creador.ToString();
            return descripcion;
        }
    }
}