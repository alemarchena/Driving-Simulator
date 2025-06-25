using System;
using System.Collections.Generic;
using UnityEngine;

public class PalancaDeCambio : Simulator
{
    [SerializeField] private Creadores creadores = Creadores.Corro_Benicio;

    [SerializeField] List<Palanca> palancaComando;

    [SerializeField] ParteSubParte.SubParte marchaActual = ParteSubParte.SubParte.Neutro;
    Embrague embrague;

    [SerializeField] AudioClip clipSinEmbrague;
    [SerializeField] AudioClip clipCambio;
    AudioSource audioSource;
    public ParteSubParte.SubParte MarchaActual
    {
        get { return marchaActual; }
    }
    private void Start()
    {
        AsignarCreador(creadores);
        SearchAndSetComandos();
        ObtenerEmbrague();
        Describir();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("Falta AudioSource en el objeto Palanca de cambio.");
        if(clipCambio == null )
                Debug.LogError("Falta el clip de sonido de Palanca de cambio.");
        if(clipSinEmbrague == null)
            Debug.LogError("Falta el clip de sonido de embrague.");


    }

    private void ObtenerEmbrague()
    {
        embrague = GetComponent<Embrague>();
        if (embrague == null) {
            Debug.LogError("Falta el componente Embrague en la caja de cambios");
        }
    }
    private void SearchAndSetComandos()
    {
        foreach(var cambio in palancaComando)
        {
            //Revisa todas las teclas asignadas a cada parteSubparte
            List<KeyCode> k = ControladorComandos.instance.AsignaTeclas(cambio.palanca);

            //Setea la tecla detectada a cada elemento de parteSubparte
            cambio.SetComando(k);

            //Agrega las teclas traidas del Data a la lista de teclas disponibles 
            foreach(var key in k) teclasComando.Add(key);

        }
    }


    void Update()
    {
        foreach (KeyCode key in teclasComando)
        {
            if (Input.GetKeyDown(key))
            {
                if (embrague.EstaPresionado)
                {
                    CambiarMarcha( DetectarCambioPalanca(key) );
                    break;
                }
                else
                {
                    ReproducirSonido(clipSinEmbrague);
                }
            }
        }
    }
    private ParteSubParte.SubParte DetectarCambioPalanca(KeyCode comando)
    {
        foreach (var cambio in palancaComando)
        { 
            foreach (KeyCode tecla in cambio.teclasComando)
            {
                
                if(tecla == comando) return cambio.palanca.subParte;
            }
        }
        return ParteSubParte.SubParte.Ninguna;
    }

    void CambiarMarcha(ParteSubParte.SubParte nuevaMarcha)
    {
        if (marchaActual != nuevaMarcha)
        {
            ReproducirSonido(clipCambio);
            marchaActual = nuevaMarcha;
            Tablero.instance.MostrarMarcha(marchaActual); //Muestra la marcha en texto
        }
    }

    
    public override void Describir()
    {
    }

    public override void AsignarCreador(Creadores creadores)
    {
        CreadoresSimulator = creadores;
    }

    void ReproducirSonido(AudioClip clip)
    {
        if (audioSource == null) return;

        audioSource.PlayOneShot(clip);
    }
}

[Serializable]
public class Palanca
{
    public ParteSubParte palanca;
    public List<KeyCode> teclasComando = new List<KeyCode>();

    public void SetComando(List<KeyCode> listaComandos)
    {
        teclasComando = listaComandos;
    }
}