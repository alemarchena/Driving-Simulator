//Juan Martin Romito
using System;
using UnityEngine;

public class LucesAltas : Simulator
{
    [SerializeField] private Creadores creadores;
    private bool lucesAltasEncendidas = false;


    public bool LucesAltasEncendidas {
        get{ return lucesAltasEncendidas;}
    }
    void Start()
    {
        AsignarCreador(creadores);
        AsignarComandos();
        Tablero.instance.MostrarLuzAlta(lucesAltasEncendidas);
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
            Tablero.instance.MostrarLuzAlta(lucesAltasEncendidas);

            string mensaje = lucesAltasEncendidas ? "Encendida" : "Apagada";
            MessageDisplaySystem.instance.ShowMessage("Luz alta " + mensaje, 1f, 0f);
        }
    }
}
