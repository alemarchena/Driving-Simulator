using System.Collections.Generic;
using UnityEngine;

public class ControladorComandos : MonoBehaviour
{
    [SerializeField] Data data;
    public static ControladorComandos instance;

    List<KeyCode> teclasComando = new List<KeyCode>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (teclasComando == null)
        {
            Debug.LogError("Falta asignar TeclasData al vehículo");
            return;
        }
    }

    public List<KeyCode> AsignaTeclas(ParteSubParte parteSubparte)
    {
        if (data == null) {
            Debug.LogError("Falta asignar Data a Controlador de Comandos");
            return null;
        }
        var diccionarioTeclas = data.GetComandos();
      
        string clave = parteSubparte.parte.ToString() + parteSubparte.subParte.ToString();

        if (diccionarioTeclas.TryGetValue(clave, out List<KeyCode> teclasEncontradas))
        {
            teclasComando = new List<KeyCode>(teclasEncontradas); // Copia limpia
        }
        else
        {
            teclasComando = new List<KeyCode>(); // Asegura no tener null
            Debug.LogWarning($"No se encontraron teclas para la parte: {clave}");
        }
        

        return teclasComando;
    }
}