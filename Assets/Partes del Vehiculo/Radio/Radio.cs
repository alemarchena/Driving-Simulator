using System.Collections.Generic;
using UnityEngine;

public class Radio : Simulator
{
    [SerializeField] List<AudioClip> audioClipList;
    [SerializeField] Creadores creadores;
    private AudioSource AudioSource;

    private int indice = 0;
 

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        AsignarComandos();
        AsignarCreador(creadores);
    }

    private void Update()
    {
        if (SePresionoLaTecla())
        {
            Cambiar();
        }
    }

    public void Cambiar()
    {
        indice += 1;
        if(indice > audioClipList.Count-1) indice = 0;

        AudioSource.clip = audioClipList[indice];
        Escuchar();
    }

    
    private void Escuchar()
    {
        Tablero.instance.MostrarRadio(audioClipList[indice].name);
        AudioSource.Play();
    }

    public override void AsignarCreador(Creadores creador)
    {
        CreadoresSimulator = creador;
    }
}