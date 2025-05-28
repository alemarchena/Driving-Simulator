using System;
using System.Collections.Generic;
using UnityEngine;
public class Balizas : Simulator
{
    [SerializeField] private Creadores creadores;
    [SerializeField] private float tiempoParpadeo = 1f;
    public bool BalizaActiva => balizaActiva;
    private bool balizaActiva = false;
    private bool estadoLuz = false;
    private float timer = 0f;

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
            balizaActiva = !balizaActiva;

            if (!balizaActiva)
            {
                estadoLuz = false;
                timer = 0f;
                Tablero.instance.MostrarBalizas(estadoLuz);
            }
        }

        if (balizaActiva)
        {
            timer += Time.deltaTime;
            if (timer >= tiempoParpadeo)
            {
                timer = 0f;
                estadoLuz = !estadoLuz;
                Tablero.instance.MostrarBalizas(estadoLuz);
            }
        }
    }
    public bool PuedeActivarse()
    {
        if (!balizaActiva)
        {
            Debug.Log("Se pueden activar las luces de giro");
            return true;
        }
        else
        {
            Debug.Log("No se pueden activar las luces de giro");
            return false;
        }
    }
}