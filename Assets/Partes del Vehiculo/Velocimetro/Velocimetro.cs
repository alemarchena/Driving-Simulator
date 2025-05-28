using UnityEngine;

public class Velocimetro : Simulator
{
    [SerializeField] Creadores creador = Creadores.Alejandro_Marchena;
    [SerializeField] Motor motor;
    [SerializeField] private float anguloMin = -20f;
    [SerializeField] private float anguloMax = 200f;
    private void Start()
    {
        AsignarCreador(creador);
        VerificarSiTieneMotor();
    }
    public override void AsignarCreador(Creadores creadores)
    {
        CreadoresSimulator = creador;
    }
    private void VerificarSiTieneMotor()
    {
        if (motor == null)
        {
            Debug.LogError("Falta asignar el motor al velocímetro");
            return;
        }
    }
    private void FixedUpdate()
    {
        Tablero.instance.MostrarRPM(CalculaAnguloRPM());
    }

    private float CalculaAnguloRPM()
    {
        float rpmActual = motor.RPMactual;
        
        if(rpmActual > 0)
            rpmActual = Mathf.Clamp(rpmActual, rpmActual, motor.RPMmaxima);
  
        float t = rpmActual / motor.RPMmaxima;

        if (t < 0) t *= -1;
        // Interpola entre 0 (ángulo base) y el ángulo máximo
        float angulo = Mathf.Lerp(anguloMin, anguloMax, t) * -1;

        return angulo;
    }
}
