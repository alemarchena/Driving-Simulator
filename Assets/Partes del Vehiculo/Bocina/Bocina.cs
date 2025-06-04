using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

[RequireComponent(typeof(AudioSource))]
public class Bocina : MonoBehaviour
{

    [SerializeField] ParteSubParte parteSubparteNormal;
    List<KeyCode> teclasComandoNormal = new List<KeyCode>();
    [SerializeField] ParteSubParte parteSubparteSecreto;
    List<KeyCode> teclasComandoSecreta = new List<KeyCode>();

    private enum ModoBocina { Normal, Secreto }

    [Header("Configuración de creador")]
    [SerializeField]
    private Creadores creadores = Creadores.Sabatini_Cialone_Santino;

    [Header("Clips de audio (arrástralos en el Inspector)")]
    [SerializeField]
    private AudioClip clipNormal;
    [SerializeField]
    private AudioClip clipSecreto;

    private ModoBocina modoActual = ModoBocina.Normal;
    bool sePresionoTeclaNormal;
    bool sePresionoTeclaSecreta;
    private void Awake()
    {
        // Asegura creador por defecto
        creadores = Creadores.Sabatini_Cialone_Santino;

    }

    private void Start()
    {
        teclasComandoNormal = ControladorComandos.instance.AsignaTeclas(parteSubparteNormal);
        teclasComandoSecreta = ControladorComandos.instance.AsignaTeclas(parteSubparteSecreto);
    }

    private void Update()
    {

        sePresionoTeclaSecreta = false;

        foreach (KeyCode ki in teclasComandoSecreta)
        {
            if (Input.GetKeyDown(ki))
            {
                sePresionoTeclaSecreta = true;
                break;
            }
        }

        if (sePresionoTeclaSecreta)
        {
            modoActual = modoActual == ModoBocina.Normal
                   ? ModoBocina.Secreto
                   : ModoBocina.Normal;
        }
          

        sePresionoTeclaNormal = false;

        foreach (KeyCode ki in teclasComandoNormal)
        {
            if (Input.GetKey(ki))
            {
                sePresionoTeclaNormal = true;
                break;
            }
        }

        if (sePresionoTeclaNormal)
        {
            TocarBocina();
        }
       
    }

    private void TocarBocina()
    {
        AudioClip clip = (modoActual == ModoBocina.Normal)
            ? clipNormal
            : clipSecreto;

        if (clip == null)
        {
            return;
        }
        SoundManager.Instance.PlaySoundGlobal(clip, ModePlay.playOneShoot, false);
    }
}