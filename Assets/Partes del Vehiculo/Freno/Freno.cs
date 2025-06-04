using System.Collections.Generic;
using UnityEngine;

public class Freno : MonoBehaviour
{

    [Header("Configuración del Freno")]
    [SerializeField] ParteSubParte parteSubparteFreno;
    List<KeyCode> teclasComandoFreno = new List<KeyCode>();
    [SerializeField] ParteSubParte parteSubparteFrenoMano;
    List<KeyCode> teclasComandoFrenoMano = new List<KeyCode>();

    [SerializeField] bool frenado = false; // Estado del freno normal
    [SerializeField] bool frenoDeManoActivo = false; // Estado del freno de mano
    [SerializeField] float decrementoFrenado = 100f;

    private float cantidadFrenado = 0f; // Valor de frenado entre 0 y 1
    [SerializeField] float cantidadFrenadoMaximo = 100f; // Valor de frenado Maximo
    [SerializeField] float sensibiidadFrenado = 100f;

    [Header("Configuración del Desgaste")]
    [SerializeField] float durabilidadFreno = 100f; // Durabilidad inicial del freno
    [SerializeField] float desgastePorUso = 0.1f; // Cuánto se desgasta cada vez que se frena

    [Header("Calentamiento del Freno")]
    [SerializeField] float temperaturaFreno = 0f; // Temperatura inicial
    [SerializeField] float incrementoTemperatura = 10f; // Cuánto aumenta la temperatura al usar el freno
    [SerializeField] float enfriamientoPorSegundo = 5f; // Cuánto disminuye la temperatura por segundo

    [Header("Freno Gradual")]
    public AnimationCurve curvaDeFrenado; // Curva que define la respuesta del freno

    [SerializeField] Creadores creador = Creadores.Acieff_Valentin;

    bool sePresionoTeclaFreno;
    bool sePresionoTeclaFrenoMano;

    public bool FrenoDeManoActivo
    {
        get { return frenoDeManoActivo; }
    }
    private void Start()
    {
    teclasComandoFreno = ControladorComandos.instance.AsignaTeclas(parteSubparteFreno);
    teclasComandoFrenoMano = ControladorComandos.instance.AsignaTeclas(parteSubparteFrenoMano);
    }

    private void Update()
    {
        ControlarFrenoDeMano();
        Frenar();
        MostrarEstado();
    }

    public void Frenar()
    {
        if (frenoDeManoActivo)
        {
            cantidadFrenado = cantidadFrenadoMaximo; // Freno total
            frenado = true;
        }
        else
        {
            sePresionoTeclaFreno = false;

            foreach (KeyCode ki in teclasComandoFreno)
            {
                if (Input.GetKey(ki))
                {
                    sePresionoTeclaFreno = true;
                    break;
                }
            }

            if (sePresionoTeclaFreno)
            {
                // Aplicar frenado gradual 
                float tiempoPresionado = Mathf.Clamp(Time.time, 0f, 1f);

                cantidadFrenado = curvaDeFrenado.Evaluate(tiempoPresionado) * sensibiidadFrenado * (durabilidadFreno / 100f);
            

                cantidadFrenado = Mathf.Clamp(cantidadFrenado, 0f, cantidadFrenadoMaximo);
                frenado = true;

                // Reducir durabilidad y aumentar temperatura
                durabilidadFreno -= desgastePorUso * Time.deltaTime;
                durabilidadFreno = Mathf.Clamp(durabilidadFreno, 0f, 100f);

                temperaturaFreno += incrementoTemperatura * Time.deltaTime;
                temperaturaFreno = Mathf.Clamp(temperaturaFreno, 0f, 100f);
            }
            else
            {
                cantidadFrenado -= Time.deltaTime * decrementoFrenado;
                cantidadFrenado = Mathf.Clamp(cantidadFrenado, 0f, cantidadFrenadoMaximo);
                frenado = false;

                // Enfriar el freno cuando no está en uso
                temperaturaFreno -= enfriamientoPorSegundo * Time.deltaTime;
                temperaturaFreno = Mathf.Clamp(temperaturaFreno, 0f, 100f);
            }
        }
    }

    public void ControlarFrenoDeMano()
    {
        sePresionoTeclaFrenoMano = false;

        foreach (KeyCode ki in teclasComandoFrenoMano)
        {
            if (Input.GetKeyDown(ki))
            {
                sePresionoTeclaFrenoMano = true;
                break;
            }
        }

        if (sePresionoTeclaFrenoMano)
        {
            frenoDeManoActivo = !frenoDeManoActivo; // Cambiar estado
        }
    }

    public void MostrarEstado()
    {
        //string estado = FrenoDeManoActivo ? "Freno de Mano Activado" : (Frenado ? "Frenando" : "Libre");
        Tablero.instance.MostrarFrenoDeMano(frenoDeManoActivo);
        //Debug.Log($"Estado del freno: {estado}, Cantidad de Frenado: {cantidadFrenado:F2}, " +
        //          $"Durabilidad: {durabilidadFreno:F2}%, Temperatura: {temperaturaFreno:F2}°C");
    }

    public float ObtenerCantidadFrenado()
    {
        return cantidadFrenado;
    }

    public void RepararFreno(float cantidad)
    {
        durabilidadFreno += cantidad;
        durabilidadFreno = Mathf.Clamp(durabilidadFreno, 0f, 100f);
        Debug.Log($"Freno reparado. Durabilidad actual: {durabilidadFreno}%");
    }

}
