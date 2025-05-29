//Juan Martin Romito
using System;
using UnityEngine;

public class CinturonSeguridad : Simulator
{
    [SerializeField] private Creadores creadores;
    private bool cinturonAbrochado = false;
    public CinturonAbrochado {
        get { return cinturonAbrochado;}
    }
    [SerializeField] AudioClip clip;

    private float tiempoTranscurrido = 0f;
    [SerializeField]
    float tiempoVerificador = 3f;

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
        cinturonAbrochado = !cinturonAbrochado;
        Tablero.instance.MostrarCinturon(!cinturonAbrochado);
    }
    tiempoTranscurrido += Time.deltaTime;

    if (tiempoTranscurrido > tiempoVerificador) {
        tiempoTranscurrido = 0f;
        //Disparar el sonido   
        Debug.Log("Sonar.."); 
    }
    }

    
}
