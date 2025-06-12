//Juan Martin Romito
using System;
using UnityEngine;

public class CinturonSeguridad : Simulator
{
    [SerializeField] AudioClip clip;
    [SerializeField] private Creadores creador;
    [SerializeField] float tiempoVerificador = 3f;
    public override void AsignarCreador(Creadores creadores)
    {
        CreadoresSimulator = creador;
    }
    private bool cinturonAbrochado = false;
    public bool CinturonAbrochado 
    {
        get { return cinturonAbrochado;}
    }


private float tiempoTranscurrido = 0f;

void Start()
{
    AsignarCreador(creador);
    AsignarComandos();
    Tablero.instance.MostrarCinturon(cinturonAbrochado);
}

void Update()
{
    if (SePresionoLaTecla())
    {
        cinturonAbrochado = !cinturonAbrochado;
        Tablero.instance.MostrarCinturon(cinturonAbrochado);
    }
    tiempoTranscurrido += Time.deltaTime;

    if (tiempoTranscurrido > tiempoVerificador)
    {
        tiempoTranscurrido = 0f;
        if(clip != null) ControladorSonidos.Instance.PlaySoundGlobal(clip,ControladorSonidos.ModePlay.playOneShoot,true,1f);
    }
}
}
