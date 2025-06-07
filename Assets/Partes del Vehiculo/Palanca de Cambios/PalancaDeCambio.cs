using System;
using System.Collections.Generic;
using UnityEngine;

public class PalancaDeCambio : Simulator
{
    [SerializeField] private Creadores creadores = Creadores.Corro_Benicio;

    [SerializeField] List<Palanca> palancaComando;

    [SerializeField] ParteSubParte.SubParte marchaActual = ParteSubParte.SubParte.Neutro;
    Embrague embrague;
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
                    //hizo un cambio sin presionar el embrague (Ver que hacer...)
                    Debug.Log("No presionaste el embrague");
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