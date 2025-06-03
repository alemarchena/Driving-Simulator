//Juan Martin Romito
using System;
using UnityEngine;

public class LucesAltas : Simulator
{
    [SerializeField] private Creadores creadores;
    private bool lucesAltasEncendidas = false;

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
            lucesAltasEncendidas = !lucesAltasEncendidas;
            Tablero.instance.MostrarLuzAltaz(lucesAltasEncendidas);
        }
    }
}
