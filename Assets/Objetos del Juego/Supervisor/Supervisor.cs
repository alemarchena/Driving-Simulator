using System.Collections.Generic;
using UnityEngine;

public class Supervisor : MonoBehaviour
{
    public static Supervisor Instance;
    [SerializeField] ControladorInfracciones controladorInfracciones;
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Agrega y guarda una infracción al player
    /// </summary>
    /// <param name="infraccion"></param>
    public void AgregarInfraccion(Infraccion infraccion)
    {
        controladorInfracciones.AgregarInfraccion(infraccion);
    }

    /// <summary>
    /// Devuelve las infracciones que contiene el player actualmente
    /// </summary>
    /// <returns></returns>
    public List<Infraccion> ObtenerInfraccionesDelPlayer()
    {
       return controladorInfracciones.GetInfraccionesDelPlayer();
    }

    /// <summary>
    /// Devuelve la base de datos de tipos de Infracciones y su detalle
    /// </summary>
    /// <returns></returns>
    public List<Infraccion> ObtenerInfracciones()
    {
        return controladorInfracciones.Infracciones;
    }

    /// <summary>
    /// Elimina todas las infracciones del player
    /// </summary>
    public void EliminarInfraccionesDelPlayer()
    {
        controladorInfracciones.LimpiarInfraccionesDelPlayer();
    }
}
