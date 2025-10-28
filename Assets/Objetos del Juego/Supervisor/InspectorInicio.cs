using UnityEngine;

public class InspectorInicio : MonoBehaviour
{
    [SerializeField] private LucesBajas luces;
    [SerializeField] private CinturonSeguridad cinturon;
    [SerializeField] private Freno freno;
    [SerializeField] private Vehiculo vehiculo;
    [SerializeField] private FuelTank tanqueCombustible;
    [SerializeField] private Motor motor;
    [SerializeField] private bool perdio = false;
    void Update()
    {
        if (vehiculo.VelocidadActual > 10 && !perdio  )
        {
            if(motor != null && motor.MotorEncendido)
            {
                if (luces.LucesEncendidas == false)
                {
                    GameOverManager.instance.GameOver(false, "Las luces están APAGADAS.");
                }
                if (cinturon.CinturonAbrochado == false)
                {
                    GameOverManager.instance.GameOver(false, "El cinturon esta desabrochado.");
                }
                if (freno.FrenoDeManoActivo == true)
                {
                    GameOverManager.instance.GameOver(false, "El freno esta puesto.");
                }
                if (tanqueCombustible.HasFuel == false)
                {
                    GameOverManager.instance.GameOver(false, "Te quedaste sin combustible.");
                }
            }
        }
    }
}
