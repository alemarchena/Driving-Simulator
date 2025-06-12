using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class Vehiculo : Simulator
{
    [SerializeField] Creadores creador = Creadores.Alejandro_Marchena;

    [Header("Partes del vehículo")]
    [SerializeField] Motor motor;
    [SerializeField] Freno freno;
    [Range(0f, 1f)]
    [SerializeField] float reduccionFuerzaFrenoMano = 0.3f; // 30% de fuerza cuando el freno de mano está activo

    [SerializeField] Volante volante;
    [SerializeField] FuelTank tanqueCombustible;
    [SerializeField] Ruedas ruedas;
    public static Vehiculo instance;

    [Space]
    [Header("Parametros Vehiculo en el aire")]
    [SerializeField] float masaEnElAire;

    [Space]
    [Header("Giro del vehículo")]
    [SerializeField] private float intensidadGiro = 2.5f; // Cuánto gira al máximo
    [SerializeField] private float limiteVelocidadGiro = 5f; // No girar si es muy lento

    [Space]
    [Header("Giro de las agujas")]
    [SerializeField] private float velocidadMaxima = 200f;
    [SerializeField] private float anguloMin = -20f;
    [SerializeField] private float anguloMax = 200f;


    private float coeficienteFriccionActual;

    private Rigidbody rb;
    private float masaOriginal;
    private float linearDumpingOriginal;

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
        linearDumpingOriginal = rb.linearDamping;

        VerificarSiTieneMotor();
    }

    

    private void Update()
    {
        if (ruedas.EnElAire)
        {
            rb.mass = masaEnElAire;
            rb.linearDamping = 0f;
            return;
        }
        else
        {
            rb.mass = masaOriginal;
            rb.linearDamping = linearDumpingOriginal;
        }

        if (motor.ModificadorDeMasa)
        {
            rb.mass = masaOriginal + masaOriginal * motor.PorcentajeModificadoDeMasa / 100;
            rb.angularDamping = motor.PorcentajeModificadoDeMasa;
        }
        else
        {
            rb.mass = masaOriginal;
        }

        if (ruedas == null)
        {
            Debug.LogError("Falta asignar el objeto Ruedas, la fricción será 0.3");
            coeficienteFriccionActual = 0.3f;
        }
        else
        {
            coeficienteFriccionActual = ruedas.CoeficienteFriccionActual;
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
        if (ruedas.EnElAire)
        {
            rb.AddForce(Vector3.down * ruedas.EmpujeEnElAire, ForceMode.Acceleration);
        }

        if (motor.MotorEncendido && !ruedas.TodasRuedasEnElAire)
        {
            tanqueCombustible.ConsumeFuel(motor.ConsumoCombustible);

            float fuerzaAplicada = motor.FuerzaMotor;

            if (freno.FrenoDeManoActivo)
            {
                fuerzaAplicada *= reduccionFuerzaFrenoMano;
            }

            rb.AddForce(transform.forward * fuerzaAplicada, ForceMode.Force);

            VerificaFrenaje();

            VerificaGiroVolante();

            Tablero.instance.MostrarVelocidad(CalculaAnguloVelocidad());
        }
    }

    private void VerificaFrenaje()
    {
        // Aplicar frenado
        float cantidadFrenado = freno.ObtenerCantidadFrenado();
        float fuerzaFriccion;

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

            fuerzaFriccion = velocidadActual.magnitude * coeficienteFriccionActual;

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

    public void RecibirDanio(float danio)
    { 
        
    }

}

