using UnityEngine;

public abstract class Obstaculo : MonoBehaviour
{
    public string nombre;
    public int danio;

    public virtual void AplicarDaņo(Vehiculo jugador)
    {
        jugador.RecibirDanio(danio);
    }
}