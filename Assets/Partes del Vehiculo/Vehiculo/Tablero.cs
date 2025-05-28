using UnityEngine;

public class Tablero : MonoBehaviour
{
    public static Tablero instance;
    [SerializeField] private Creadores creadores = Creadores.Alejandro_Marchena;

    [Space]
    [Header("Luces del Vehículo")]
    [SerializeField] GameObject balizas;
    [SerializeField] GameObject giroIzquierdo;
    [SerializeField] GameObject giroDerecho;

    [Space]
    [Header("Freno de Mano")]
    [SerializeField] GameObject frenoDeMano;
    [Space]
    [Header("Cambios del Vehículo")]
    [SerializeField] GameObject cambioNeutro;
    [SerializeField] GameObject cambioPrimera;
    [SerializeField] GameObject cambioSegunda;
    [SerializeField] GameObject cambioTercera;
    [SerializeField] GameObject cambioCuarta;
    [SerializeField] GameObject cambioQuinta;
    [SerializeField] GameObject cambioSexta;
    [SerializeField] GameObject cambioReversa;

    [Space]
    [Header("Agujas del Tablero del Vehículo")]
    [SerializeField] GameObject agujaVelocimetro;
    [SerializeField] GameObject agujaRPM;

    private void Awake()
    {
        instance = this;
    }

    public void MostrarFrenoDeMano(bool estadoFrenadoMano)
    {
        frenoDeMano.SetActive(estadoFrenadoMano);
    }
    public void MostrarBalizas(bool estadoLuz)
    {
        balizas.SetActive(estadoLuz);
    }

    public void MostrarGiroIzquierdo(bool estadoLuz)
    {
        giroIzquierdo.SetActive(estadoLuz);
    }

    public void MostrarGiroDerecho(bool estadoLuz)
    {
        giroDerecho.SetActive(estadoLuz);
    }

    public void MostrarMarcha(ParteSubParte.SubParte marcha)
    {
        DeshabilitarMarchas();

        if (marcha == ParteSubParte.SubParte.Neutro)
            cambioNeutro.SetActive(true);
        if (marcha == ParteSubParte.SubParte.Reversa)
            cambioReversa.SetActive(true);
        if (marcha == ParteSubParte.SubParte.Primera)
            cambioPrimera.SetActive(true);
        if (marcha == ParteSubParte.SubParte.Segunda)
            cambioSegunda.SetActive(true);
        if (marcha == ParteSubParte.SubParte.Tercera)
            cambioTercera.SetActive(true);
        if (marcha == ParteSubParte.SubParte.Cuarta)
            cambioCuarta.SetActive(true);
        if (marcha == ParteSubParte.SubParte.Quinta)
            cambioQuinta.SetActive(true);
        if (marcha == ParteSubParte.SubParte.Sexta)
            cambioSexta.SetActive(true);
    }
    private void DeshabilitarMarchas()
    {
        cambioNeutro.SetActive(false);
        cambioPrimera.SetActive(false);
        cambioSegunda.SetActive(false);
        cambioTercera.SetActive(false);
        cambioCuarta.SetActive(false);
        cambioQuinta.SetActive(false);
        cambioSexta.SetActive(false);
        cambioReversa.SetActive(false);
    }

    public void MostrarVelocidad(float angulo)
    {
        agujaVelocimetro.transform.localRotation = Quaternion.Euler(0f, 0f, angulo);
    }

    public void MostrarRPM(float angulo)
    {
        agujaRPM.transform.localRotation = Quaternion.Euler(0f, 0f, angulo);
    }

    public void MostrarRadio(string nombre)
    {
        Debug.Log("Tema de radio " + nombre);
    }


    public void MostrarMensajeAlConductor(string mensaje)
    {

    }
}
