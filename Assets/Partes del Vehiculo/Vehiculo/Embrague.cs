using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Embrague : Simulator
{
    private enum EstadoEmbrague { Presionado, Suelto }

    // Asignamos directamente a Santino Sabatini como creador predeterminado
    [SerializeField] private Creadores creadores = Creadores.Sabatini_Cialone_Santino;

    private EstadoEmbrague estadoActual = EstadoEmbrague.Suelto;

    [SerializeField] private bool estaPresionado;

    public bool EstaPresionado => estaPresionado;

    private void Awake()
    {
        creadores = Creadores.Sabatini_Cialone_Santino;
    }
    private void Start()
    {
        AsignarCreador(creadores);
        Describir();
        teclasComando = ControladorComandos.instance.AsignaTeclas(parteSubparte);
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

    public override void Describir()
    {

    }

    public override void AsignarCreador (Creadores creador)
    {
        CreadoresSimulator = creador;
    }

}