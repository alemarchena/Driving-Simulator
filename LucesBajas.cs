//Juan Martin Romito
using System;
using UnityEngine;

public class LucesBajas : Simulator
{
    [SerializeField] private Creadores creadores;
    private bool lucesEncendidas = false;

    void Start()
    {
        AsignarCreador(creadores);
        AsignarComandos();
    }

    public override void AsignarCreador(Creadores creador)
    {
        CreadoresSimulator = creador;
    }

    void Update()
    {
        if (SePresionoLaTecla())
        {
            lucesEncendidas = !lucesEncendidas;
            Tablero.instance.MostrarLuzBaja(lucesEncendidas);
        }
    }
}
