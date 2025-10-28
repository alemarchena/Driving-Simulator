//Juan Martin Romito
using System.Collections.Generic;
using UnityEngine;

public class LucesBajas : Simulator
{
    [SerializeField] private Creadores creadores;
    private bool lucesEncendidas = false;
    [SerializeField] List<GameObject> luces;

    public bool LucesEncendidas {
        get { return lucesEncendidas; } 
    }
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
            TurnLight(LucesEncendidas);
            string mensaje = lucesEncendidas ? "Encendida" : "Apagada";
            MessageDisplaySystem.instance.ShowMessage("Luz baja " + mensaje, 1f, 0f);

        }
    }

    public void TurnLight(bool state)
    {
        foreach (var luz in luces)
        {
            luz.SetActive(state);
        }
    }
}
