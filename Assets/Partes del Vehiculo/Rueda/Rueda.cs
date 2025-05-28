using UnityEngine;

public class Rueda : Simulator
{
    [SerializeField] GameObject RuedaSana;
    [SerializeField] GameObject RuedaPinchada;

    [SerializeField] Creadores creadores = Creadores.Castro_Corradi_Maximiliano;

    public bool estaPinchada = false;    // Estado actual de la rueda

    private void Start()
    {
        AsignarCreador(creadores);
        AsignarComandos();
    }
    void Update()
    {
        if (SePresionoLaTecla())
        {
            CambiarEstadoRueda();
        }
    }

    void CambiarEstadoRueda()
    {
        estaPinchada = !estaPinchada;

        if (estaPinchada)
        {
            RuedaPinchada.SetActive(true);
            RuedaSana.SetActive(false);
        }
        else
        {
            RuedaPinchada.SetActive(false);
            RuedaSana.SetActive(true);
        }
            
    }

    public override void AsignarCreador(Creadores creadores)
    {
       CreadoresSimulator = creadores;
    }
}

