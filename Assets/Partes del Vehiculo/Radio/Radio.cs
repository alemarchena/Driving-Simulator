using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Radio : Simulator
{
    [SerializeField] List<AudioClip> audioClipList;
    [SerializeField] Creadores creadores;

    private int indice = 0;
    AudioClip anteriorAudioClip=null;
    AudioClip proximoAudioClip=null;

    private void Start()
    {
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
        anteriorAudioClip = audioClipList[indice];
        indice += 1;

        if (indice > audioClipList.Count - 1)
        {
            indice = 0;
            proximoAudioClip = audioClipList[indice];
            
        }
        else
        {
            proximoAudioClip = audioClipList[indice];
        }


        SoundManager.Instance.PlaySoundGlobal( anteriorAudioClip, proximoAudioClip, 
            SoundManager.ModePlay.play, true);
        
        Escuchar();
    }

    
    private void Escuchar()
    {
        Tablero.instance.MostrarRadio(audioClipList[indice].name);
    }

    public override void AsignarCreador(Creadores creador)
    {
        CreadoresSimulator = creador;
    }
}