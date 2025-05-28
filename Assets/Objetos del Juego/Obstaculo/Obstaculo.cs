using UnityEngine;

public abstract class Obstaculo : MonoBehaviour
{
    public string nombre;
    public int danio;

    public virtual void AplicarDaño(Vehiculo jugador)
    {
        jugador.RecibirDanio(danio);
    }
}