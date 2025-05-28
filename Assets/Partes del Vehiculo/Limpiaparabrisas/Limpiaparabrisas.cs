using System.Collections.Generic;
using UnityEngine;

public class Limpiaparabrisas : Simulator
{
    [Header("Configuración")]
    [SerializeField] private float[] velocidadesCiclo = { 200f, 250f, 300f };

    [Header("Estado interno")]
    [SerializeField] private float amplitud = 45f;
    [SerializeField] float anguloinactivo =0f;  
    
    private bool activo = false;
    private float velocidadLimpiado = 0f;
    private int estadoActual = -1;

    private float tiempo = 0f;
    private void Start()
    {
        activo = false;
        velocidadLimpiado = 0f;
        AsignarComandos();
    }

    private void Update()
    {

        if (SePresionoLaTecla())
        {
            CambiarVelocidad();
        }

        if (activo)
        {
            tiempo += Time.deltaTime * velocidadLimpiado;
            //Oscila entre el tiempo y una amplitud desde el ánguloinactivo
            float angulo = -Mathf.PingPong(tiempo, amplitud );

            transform.localRotation = Quaternion.Euler(0f, 0f, angulo);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, anguloinactivo);
        }
    }

    private void CambiarVelocidad()
    {
        estadoActual++;
        if (estadoActual >= velocidadesCiclo.Length)
        {
            activo = false;
            velocidadLimpiado = 0f;
            estadoActual = -1;
            transform.localRotation = Quaternion.identity;
            tiempo = 0f;
        }
        else
        {
            activo = true;
            velocidadLimpiado = velocidadesCiclo[estadoActual];
        }
    }

    public bool EstaActivo() => activo;
    public void SetActivo(bool estado) => activo = estado;
    public float GetVelocidadLimpiado() => velocidadLimpiado;
    public void SetVelocidadLimpiado(float velocidad) => velocidadLimpiado = velocidad;

    public override void Describir()
    {
        Debug.Log("Este objeto simula el funcionamiento de un limpiaparabrisas de automóvil.");
    }

    public override void AsignarCreador(Creadores creadores)
    {
        CreadoresSimulator = creadores;
        Debug.Log($"Creador asignado al Limpiaparabrisas: {creadores}");
    }
}

