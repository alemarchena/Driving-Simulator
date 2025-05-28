using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class ControladorDeCamaras : Simulator
{
    [SerializeField] List<CinemachineCamera> cameras;
    [SerializeField] Creadores creador = Creadores.Alejandro_Marchena;

    private int camaraActual = 0;
    public override void AsignarCreador(Creadores creadores)
    {
        CreadoresSimulator = creadores;
    }

    void Start()
    {
        AsignarComandos();
    }

   
    void Update()
    {
        if (SePresionoLaTecla()) // cambiar cámara
        {
            camaraActual = (camaraActual + 1) % cameras.Count;
            ActivarSoloCamara(camaraActual);
        }
    }

    void ActivarSoloCamara(int index)
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].Priority = (i == index) ? 10 : 0;
        }
    }
}
