using System.Collections.Generic;
using UnityEngine;


public abstract class Simulator : MonoBehaviour
{
    private Creadores creadorSimulator;

    [SerializeField] protected List<KeyCode> teclasComando = new List<KeyCode>();
    [SerializeField] protected ParteSubParte parteSubparte;

    /// <summary>
    /// Esta propiedad devuelve la lista de creadores posibles del juego
    /// </summary>
    public Creadores CreadoresSimulator
    {
        get { return creadorSimulator; }
        set { creadorSimulator = value; }
    }

    /// <summary>
    /// Permite describir que hace el objeto heredado
    /// </summary>
    public virtual void Describir()
    {
        Debug.Log("Simulator es un simulador de manejo de automóviles.");
    }

    public abstract void AsignarCreador(Creadores creadores);

    /// <summary>
    /// Asigna los comandos almacenados en Data a la variable teclasComando
    /// </summary>
    protected virtual void AsignarComandos()
    {
        teclasComando = ControladorComandos.instance.AsignaTeclas(parteSubparte);
    }
    /// <summary>
    /// Devuelve true si alguna de las teclas correspondiente a Parte-Subparte ha sido presionada
    /// </summary>
    /// <returns></returns>
    protected virtual bool SePresionoLaTecla()
    {
        bool sePresionoTecla = false;
        if(teclasComando.Count>0)
        {
            foreach (KeyCode k in teclasComando)
            {
                if (Input.GetKeyDown(k))
                {
                    sePresionoTecla = true;
                    break;
                }
            }
        }
        return sePresionoTecla;
    }


    /// <summary>
    /// Devuelve true si alguna de las teclas correspondiente a Parte-Subparte se mantiene presionada
    /// </summary>
    /// <returns></returns>
    protected virtual bool SeMantienePresionadaLaTecla()
    {
        bool sePresionoTecla = false;
        if (teclasComando.Count > 0)
        {
            foreach (KeyCode k in teclasComando)
            {
                if (Input.GetKey(k))
                {
                    sePresionoTecla = true;
                    break;
                }
            }
        }
        return sePresionoTecla;
    }
}