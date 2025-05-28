using System;
using UnityEngine;

public class Motor : Simulator
{
    [SerializeField] Creadores creadores = Creadores.Cruceño_Fratti_Lucas;

    [SerializeField] private float rpmActual = 0f;
    [SerializeField] private float rpmMinima = 0;
    [SerializeField] private float rpmMaxima = 6000f;
    [SerializeField] private float torqueMaximo = 2000f;
    [SerializeField] private float fuerzaMotor = 0;
    [SerializeField] Acelerador acelerador;
    [SerializeField] PalancaDeCambio palanca;
    [Space]
    [SerializeField] private float[] relacionesDeMarcha = { 3.5f, 2.2f, 1.5f, 1.0f, 0.85f, 0.7f }; // Para 6 marchas
    [SerializeField] private float relacionRetroceso = -3.0f;
    private float porcentajeRPM;
    private bool motorEncendido;

    public bool MotorEncendido
    {
        get { return motorEncendido; }
    }
    public float FuerzaMotor
    {
        get { return fuerzaMotor; }
    }
    public float RPMactual
    {
        get { return rpmActual; }
    }
    public float RPMminima
    {
        get { return rpmMinima; }
    }
    public float RPMmaxima
    {
        get { return rpmMaxima; }
    }
    private void Start()
    {
        AsignarCreador(creadores);
        VerificaSiTieneAcelerador();
    }
    private void VerificaSiTieneAcelerador()
    {
        if(acelerador == null)
        {
            Debug.LogWarning("Falta asignar el Acelerador al motor");
        }
    }
    private void VerificaSiTienePalancaDeCambio()
    {
        if (palanca == null)
        {
            Debug.LogWarning("Falta asignar Palanca al motor");
        }
    }
    public override void AsignarCreador(Creadores creadores)
    {
        CreadoresSimulator = creadores;
    }
    private void Update()
    {
        ActualizaFuerzaMotor();
    }

    private float ObtenerRelacionDeMarcha()
    {

        int indice = 0;
        if (palanca.MarchaActual == ParteSubParte.SubParte.Neutro)  return  0;
        if (palanca.MarchaActual == ParteSubParte.SubParte.Reversa) return relacionRetroceso;

        if (palanca.MarchaActual == ParteSubParte.SubParte.Primera) indice  = 1;
        if (palanca.MarchaActual == ParteSubParte.SubParte.Segunda) indice  = 2;
        if (palanca.MarchaActual == ParteSubParte.SubParte.Tercera) indice  = 3;
        if (palanca.MarchaActual == ParteSubParte.SubParte.Cuarta)  indice  = 4;
        if (palanca.MarchaActual == ParteSubParte.SubParte.Quinta)  indice  = 5;
        if (palanca.MarchaActual == ParteSubParte.SubParte.Sexta)   indice  = 6;
        return relacionesDeMarcha[Mathf.Clamp(indice, 0, relacionesDeMarcha.Length - 1)]; ;
    }
    private void ActualizaFuerzaMotor()
    {
        float relacionMarcha = ObtenerRelacionDeMarcha();

        if (relacionMarcha == 0)
        {
            rpmActual = Mathf.Clamp(acelerador.GetAceleracion() * rpmMaxima * 1, rpmMinima, rpmMaxima);
        }else if (relacionMarcha < 0)
        {
            rpmActual = acelerador.GetAceleracion() * rpmMaxima * relacionMarcha;
        }
        else
        {
            //rpmActual = Mathf.Clamp(acelerador.GetAceleracion() * rpmMaxima * relacionMarcha, rpmMinima,rpmMaxima);
            rpmActual = acelerador.GetAceleracion() * rpmMaxima * relacionMarcha;
        }

        porcentajeRPM = rpmActual / rpmMaxima;

        if (relacionMarcha == 0)
            fuerzaMotor = 0;
        else
            fuerzaMotor = torqueMaximo * porcentajeRPM;
    }

    public float GetFuerzaMotor()
    {
        return fuerzaMotor;
    }
}
