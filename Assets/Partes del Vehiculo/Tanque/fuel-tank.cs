using Unity.VisualScripting;
using UnityEngine;

public class FuelTank : MonoBehaviour
{
    [SerializeField] float maxFuel = 50f; // Capacidad máxima del tanque (litros)
    [SerializeField] float currentFuel;   // Combustible actual

    [SerializeField] float anguloMin=0;
    [SerializeField] float anguloMax = 0;
    
    public FuelTank()
    {
        currentFuel = maxFuel; // Empieza lleno
    }

    // Consumir combustible (por ejemplo, al acelerar)
    public void ConsumeFuel(float amount)
    {
        currentFuel -= amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel); // Asegura que no baje de 0
    }

    // Cargar combustible
    public void Refuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
    }

    // Obtener el porcentaje de combustible restante
    public float GetFuelPercentage()
    {
        return currentFuel / maxFuel;
    }

    // Ver si hay combustible
    public bool HasFuel()
    {
        return currentFuel > 0f;
    }

    private void Update()
    {
        Tablero.instance.MostrarCombustible(CalculaAnguloCombustible());
    }

    private float CalculaAnguloCombustible()
    {
        // Calcula la proporción de velocidad
        float t = currentFuel / maxFuel;

        // Interpola entre 0 (ángulo base) y el ángulo máximo
        float angulo = Mathf.Lerp(anguloMin, anguloMax, t) * -1;

        return angulo;
    }
}
