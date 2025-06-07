using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Vehiculo : Simulator
{
    [SerializeField] Creadores creador = Creadores.Alejandro_Marchena;

    [Header("Partes del vehículo")]
    [SerializeField] Motor motor;
    [SerializeField] Freno freno;
    [SerializeField] Volante volante;
    [SerializeField] FuelTank tanqueCombustible;

    public static Vehiculo instance;

    [Header("Giro del vehículo")]
    [SerializeField] private float intensidadGiro = 2.5f; // Cuánto gira al máximo
    [SerializeField] private float limiteVelocidadGiro = 5f; // No girar si es muy lento


    [Header("Giro de las agujas")]
    [SerializeField] private float velocidadMaxima = 200f;
    [SerializeField] private float anguloMin = -20f;
    [SerializeField] private float anguloMax = 200f;

    [Header("Entorno")]
    [SerializeField] private float coeficienteFriccion = 0.1f; // ajustable en el Inspector

    private Rigidbody rb;
    private float masaOriginal;

    private float velocidadActual;
    public float VelocidadActual{
        get { return velocidadActual; }    
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        AsignarCreador(creador);
        rb = GetComponent<Rigidbody>();
        masaOriginal = rb.mass;
        VerificarSiTieneMotor();
    }
    private void Update()
    {
        if (motor.ModificadorDeMasa)
        {
            rb.mass = masaOriginal + masaOriginal * motor.PorcentajeModificadoDeMasa / 100;
            rb.angularDamping = motor.PorcentajeModificadoDeMasa;
        }
        else
        {
                rb.mass = masaOriginal;

        }
    }
    private void VerificarSiTieneMotor()
    {
        if(motor == null)
        {
            Debug.LogError("Falta asignar el motor al vehículo");
            return;
        }
    }
    private void FixedUpdate()
    {
        if (motor.MotorEncendido)
        {

            tanqueCombustible.ConsumeFuel(motor.ConsumoCombustible);

            if(!VerificaFrenoMano())
            {
                rb.AddForce(transform.forward * motor.FuerzaMotor, ForceMode.Force);

                VerificaFrenaje();

                VerificaGiroVolante();

                Tablero.instance.MostrarVelocidad(CalculaAnguloVelocidad());
            }
        }

    }

    private bool VerificaFrenoMano()
    {
        if (freno.FrenoDeManoActivo)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            return true;
        }else
            return false;
    }
    private void VerificaFrenaje()
    {
        // Aplicar frenado
        float cantidadFrenado = freno.ObtenerCantidadFrenado();

        if (cantidadFrenado > 0f)
        {
            Vector3 velocidadActual = rb.linearVelocity;
            Vector3 direccionFrenado = -velocidadActual.normalized;

            float magnitudFrenado = velocidadActual.magnitude * cantidadFrenado;

            rb.AddForce(direccionFrenado * magnitudFrenado * rb.mass, ForceMode.Force);
        }
        else
        {
            // Simular resistencia/fricción pasiva cuando no se frena
            Vector3 velocidadActual = rb.linearVelocity;
            Vector3 direccionFriccion = -velocidadActual.normalized;
            float fuerzaFriccion = velocidadActual.magnitude * coeficienteFriccion;

            rb.AddForce(direccionFriccion * fuerzaFriccion * rb.mass, ForceMode.Force);
        }
    }

    private void VerificaGiroVolante()
    {
        Volante.MovimientoVolante movimientoVolante = volante.GiroVolante();

        float velocidadActual = rb.linearVelocity.magnitude;

        if (velocidadActual < limiteVelocidadGiro)
            return; // No girar si está casi detenido

        float direccion = 0f;

        if (movimientoVolante == Volante.MovimientoVolante.Izquierda)
            direccion = -1f;
        else if (movimientoVolante == Volante.MovimientoVolante.Derecha)
            direccion = 1f;

        if (direccion != 0f)
        {
            float giro = direccion * intensidadGiro * Time.fixedDeltaTime;
            Quaternion rotacion = Quaternion.Euler(0f, giro, 0f);
            rb.MoveRotation(rb.rotation * rotacion);
        }
    }

    private float CalculaAnguloVelocidad()
    {
        float velocidad = rb.linearVelocity.magnitude;
        // Limita la velocidad entre 0 y la velocidad máxima
        velocidad = Mathf.Clamp(velocidad, 0f, velocidadMaxima);
        // Calcula la proporción de velocidad
        float t = velocidad / velocidadMaxima;

        // Interpola entre 0 (ángulo base) y el ángulo máximo
        float angulo = Mathf.Lerp(anguloMin, anguloMax, t) * -1;
        
        velocidadActual = Mathf.RoundToInt(Mathf.Abs(velocidad));

        return angulo;
    }

 
    public override void AsignarCreador(Creadores creadores)
    {
        CreadoresSimulator = creadores;
    }

    public void RecibirDanio(float danio) { 
    
    }
}

