using System.Collections.Generic;
using UnityEngine;

public class Supervisor : MonoBehaviour,IMetaObserver
{
    public static Supervisor Instance;
    [SerializeField] ControladorInfracciones controladorInfracciones;
    [SerializeField] GeneradorMetasAleatorio generadorMetas;

    [SerializeField] private List<Meta> metasSeleccionadas = new List<Meta>();
    [Header("UI")]
    [SerializeField] UIInfraccionesDisplay uiInfraccionesDisplay;

    [SerializeField] float montoMaximoInfracciones;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EliminarInfracciones();
    }
    private void Update()
    {
        if(metasSeleccionadas == null || metasSeleccionadas.Count <= 0)
        {
            metasSeleccionadas = generadorMetas.MetasSeleccionadasRandom();
            AddMetaToObserver();
        }
    }
    private void AddMetaToObserver()
    {
        foreach(var meta in metasSeleccionadas)
        {
            meta.AddObserver(this);
        }
    }
    /// <summary>
    /// Agrega una infracción al jugador solo si aún no la tiene registrada.
    /// </summary>
    public void AgregarInfraccion(Infraccion infraccion)
    {
        if (infraccion == null)
        {
            Debug.LogWarning("⚠️ Intento de agregar una infracción nula al Supervisor.");
            return;
        }

        // Obtener la lista actual de infracciones del player
        List<Infraccion> infraccionesActuales = controladorInfracciones.GetInfracciones();

        // Validar si ya está registrada (comparación por referencia o por tipo)
        bool yaExiste = infraccionesActuales.Exists(i => i == infraccion || i.name == infraccion.name);

        if (yaExiste)
        {
            return;
        }

        controladorInfracciones.AgregarInfraccion(infraccion);
        ActualizarUI();

        if (controladorInfracciones.TotalInfracciones() > montoMaximoInfracciones) {
            GameOverManager.instance.GameOver(false, "Infracciones $ " + controladorInfracciones.TotalInfracciones() + ",Máximo $ : " + montoMaximoInfracciones);
        }
    }


    private void ActualizarUI()
    {
        if (uiInfraccionesDisplay != null)
        {
            uiInfraccionesDisplay.MostrarInfracciones(controladorInfracciones.GetInfracciones());
        }
    }

    /// <summary>
    /// Devuelve las infracciones que contiene el player actualmente
    /// </summary>
    /// <returns></returns>
    public List<Infraccion> ObtenerInfracciones()
    {
       return controladorInfracciones.GetInfracciones();
    }

    public void EliminarInfracciones()
    {
        controladorInfracciones.LimpiarInfracciones();
    }

    public void OnMetaResolved(Meta meta)
    {
        generadorMetas.ActualizarTextoMetas();
        bool todasResueltas = metasSeleccionadas.TrueForAll(m => m.Resolved);
        if (todasResueltas)
        {
            GameOverManager.instance.GameOver(true, "Lo lograste");
        }
    }
}
