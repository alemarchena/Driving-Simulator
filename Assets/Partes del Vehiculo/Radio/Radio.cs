using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Radio : Simulator
{
    [SerializeField] private Creadores creadores = Creadores.Rada_Lizarraga_Lucas;
    [SerializeField] List<AudioClip> audioClipList;

    private int indice = 0;
    public float pasoVolumen = 0.1f;

    AudioClip anteriorAudioClip=null;
    AudioClip proximoAudioClip=null;

    [SerializeField] ParteSubParte parteSubparteSubirVolumen;
    [SerializeField] List<KeyCode> teclasComandoSubirVolumen = new List<KeyCode>();

    [SerializeField] ParteSubParte parteSubparteBajarVolumen;
    [SerializeField] List<KeyCode> teclasComandoBajarVolumen = new List<KeyCode>();

    bool presionoSubirVolumen = false;
    bool presionoBajarVolumen = false;

    float tiempoAcumulado=0;
    float tiempo=2f;
    private void Start()
    {
        AsignarComandos();
        AsignarCreador(creadores);

        teclasComandoSubirVolumen = 
            ControladorComandos.instance.AsignaTeclas(parteSubparteSubirVolumen);
        teclasComandoBajarVolumen = 
            ControladorComandos.instance.AsignaTeclas(parteSubparteBajarVolumen);
    }

    private void Update()
    {
        if (SePresionoLaTecla())
        {
            Cambiar();
        }

        if(SeMantienePresionadaLaTecla()){
            tiempoAcumulado += Time.deltaTime;
            if(tiempoAcumulado > tiempo)
            {
                tiempoAcumulado = 0;
                ControladorSonidos.Instance.StopAudioSourceWithClip(proximoAudioClip);
            }
        }
        presionoSubirVolumen = false;

        foreach (KeyCode ki in teclasComandoSubirVolumen)
        {
            if (Input.GetKey(ki))
            {
                presionoSubirVolumen = true;
                break;
            }
        }

        if (presionoSubirVolumen)
        { 
            Volumen("subir");
        }

        presionoBajarVolumen = false;

        foreach (KeyCode ki in teclasComandoBajarVolumen)
        {
            if (Input.GetKey(ki))
            {
                presionoBajarVolumen = true;
                break;
            }
        }

        if (presionoBajarVolumen)
        {
            Volumen("bajar");
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


        ControladorSonidos.Instance.PlaySoundGlobal( anteriorAudioClip, proximoAudioClip, 
            ControladorSonidos.ModePlay.play, true,0.3f);
        
        Escuchar();
    }

    private void Volumen(string funcion)
    {
        if (funcion == "bajar")
        {
            ControladorSonidos.Instance.SetVolumen(proximoAudioClip, -pasoVolumen);
        }
        else
        if (funcion == "subir")
        {
            ControladorSonidos.Instance.SetVolumen(proximoAudioClip, pasoVolumen);
        }
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