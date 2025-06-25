using System;
using UnityEngine;

public class Balizas : Simulator
{
    [Header("Configuración de balizas")]
    [SerializeField] private Creadores creadores;
    [SerializeField] private float tiempoParpadeo = 1f;

    [Header("Sonidos")]
    [SerializeField] private AudioClip ticSound;
    [SerializeField] private AudioClip tocSound;
    private AudioSource audioSource;

    private bool balizaActiva = false;
    private bool estadoLuz = false;
    private float timer = 0f;

    public bool BalizaActiva => balizaActiva;

    void Start()
    {
        AsignarCreador(creadores);
        AsignarComandos();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("Falta AudioSource en el objeto Balizas.");

        Tablero.instance.MostrarBalizas(estadoLuz);
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
                ReproducirSonido();
            }
        }
    }

    void ReproducirSonido()
    {
        if (audioSource == null) return;

        AudioClip clip = estadoLuz ? ticSound : tocSound;
        audioSource.PlayOneShot(clip);
    }

    public bool PuedeActivarse()
    {
        if (!balizaActiva)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
