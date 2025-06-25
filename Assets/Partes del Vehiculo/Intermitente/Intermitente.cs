using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class Intermitente : MonoBehaviour
{
    [SerializeField] ParteSubParte parteSubparteDerecha;
    List<KeyCode> teclasComandoDerecha = new List<KeyCode>();
    [SerializeField] ParteSubParte parteSubparteIzquierda;
    List<KeyCode> teclasComandoIzquierda = new List<KeyCode>();

    [Space]
    [SerializeField] Balizas balizas;
    [Space]
    [SerializeField] private Color colorLuz;
    [SerializeField] private bool estadoLuzIzq;
    [SerializeField] private bool estadoLuzDer;
    [SerializeField] private Vector3 posicionFrontal;
    [SerializeField] private Vector3 posicionTrasera;
    [SerializeField] private float intervaloParpadeo;
    [SerializeField] private AudioClip sonidoIntermitente;
    private AudioSource audioSource;

    [SerializeField] private float volumenSonido;

    public bool indicadorActivo;
    [SerializeField] private float anguloGiroApagado;

    private Coroutine parpadeoCoroutine; // Referencia al Coroutine actual
    bool sePresionoTeclaIzquierda;
    bool sePresionoTeclaDerecha;
    void Start()
    {
        teclasComandoDerecha = ControladorComandos.instance.AsignaTeclas(parteSubparteDerecha);
        teclasComandoIzquierda = ControladorComandos.instance.AsignaTeclas(parteSubparteIzquierda);

        Tablero.instance.MostrarGiroDerecho(false);
        Tablero.instance.MostrarGiroIzquierdo(false);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("Falta AudioSource en el objeto Balizas.");
        VerificarBalizas();
    }

    private void VerificarBalizas()
    {
        if (balizas == null)
            Debug.LogError("Falta asignar las balizas al Intermitente");
    }

    private void Update()
    {
        if(!balizas.BalizaActiva)
        {
            sePresionoTeclaIzquierda = false;

            foreach (KeyCode ki in teclasComandoIzquierda)
            {
                if (Input.GetKeyDown(ki))
                {
                    sePresionoTeclaIzquierda = true;
                    break;
                }
            }

            if (sePresionoTeclaIzquierda)
            {
                if (estadoLuzIzq)
                {
                    DesactivarLuces();
                }
                else
                {
                    DesactivarLuces(); // Asegurarse de apagar cualquier luz activa
                    ActivarLuzIzq();
                }
            }

            sePresionoTeclaDerecha = false;

            foreach (KeyCode kd in teclasComandoDerecha)
            {
                if (Input.GetKeyDown(kd))
                {
                    sePresionoTeclaDerecha = true;
                    break;
                }
            }

            if (sePresionoTeclaDerecha)
            {
                if (estadoLuzDer)
                {
                    DesactivarLuces();
                }
                else
                {
                    DesactivarLuces(); // Asegurarse de apagar cualquier luz activa
                    ActivarLuzDer();
                }
            }
        }else
        {
            if (estadoLuzIzq == true || estadoLuzDer == true) DesactivarLuces();
        }    

    }

    private void ActivarLuzIzq() //TERMINADO
    {
        if (parpadeoCoroutine != null)
        {
            StopCoroutine(parpadeoCoroutine); // Detener cualquier Coroutine en ejecución
        }
        estadoLuzIzq = true;
        estadoLuzDer = false; // Asegurarse de que la luz derecha esté apagada
        parpadeoCoroutine = StartCoroutine(ParpadeoCoroutine(intervaloParpadeo)); // Iniciar nuevo Coroutine
    }

    private void ActivarLuzDer() //TERMINADO
    {
        if (parpadeoCoroutine != null)
        {
            StopCoroutine(parpadeoCoroutine); // Detener cualquier Coroutine en ejecución
        }
        estadoLuzDer = true;
        estadoLuzIzq = false; // Asegurarse de que la luz izquierda esté apagada
        parpadeoCoroutine = StartCoroutine(ParpadeoCoroutine(intervaloParpadeo)); // Iniciar nuevo Coroutine
    }

    private void DesactivarLuces() //TERMINADO
    {
        Tablero.instance.MostrarGiroIzquierdo(false);
        Tablero.instance.MostrarGiroDerecho(false);

        if (parpadeoCoroutine != null)
        {
            StopCoroutine(parpadeoCoroutine); // Detener cualquier Coroutine en ejecución
            parpadeoCoroutine = null; // Limpiar la referencia
        }
        estadoLuzIzq = false;
        estadoLuzDer = false;

        // Asegurarse de que el sonido se detenga al apagar las luces
    }

    private void Parpadeo(float intervaloParpadeo)
    {
        if (estadoLuzIzq || estadoLuzDer)
        {
            StartCoroutine(ParpadeoCoroutine(intervaloParpadeo));
        }
    }

    private IEnumerator ParpadeoCoroutine(float intervaloParpadeo)
    {
        while (estadoLuzIzq || estadoLuzDer)
        {
            if (estadoLuzIzq)
            {
                Tablero.instance.MostrarGiroIzquierdo(true);
                ReproducirSonido(); // Sonido ON
                yield return new WaitForSeconds(intervaloParpadeo);

                Tablero.instance.MostrarGiroIzquierdo(false);
                ReproducirSonido(); // Sonido OFF
                yield return new WaitForSeconds(intervaloParpadeo);
            }
            else if (estadoLuzDer)
            {
                Tablero.instance.MostrarGiroDerecho(true);
                ReproducirSonido(); // Sonido ON
                yield return new WaitForSeconds(intervaloParpadeo);

                Tablero.instance.MostrarGiroDerecho(false);
                ReproducirSonido(); // Sonido OFF
                yield return new WaitForSeconds(intervaloParpadeo);
            }
        }
    }

    private void VerificarApagadoAuto(float anguloVolante)
    {
        if (anguloVolante >= anguloGiroApagado)
        {
            DesactivarLuces();
            // LOGICA INCOMPLETA, FALTA LO DE VOLVER EL VOLANTE AL CENTRO
        }
    }

    void ReproducirSonido()
    {
        if (audioSource == null) return;

        audioSource.PlayOneShot(sonidoIntermitente);
    }




}