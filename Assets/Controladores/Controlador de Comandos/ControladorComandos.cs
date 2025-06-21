using System.Collections.Generic;
using UnityEngine;

public class ControladorComandos : MonoBehaviour
{
    [SerializeField] Data data;
    public static ControladorComandos instance;

    List<KeyCode> teclasComando = new List<KeyCode>();

    public enum TeclasEspeciales {MouseDerecho,MouseIzquiero,MouseRueda}

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

    /// <summary>
    /// Devuelve un arreglo de keycode
    /// </summary>
    /// <returns></returns>
  
    public KeyCode TeclaEspecial(TeclasEspeciales tecla)
    {
        if (tecla == TeclasEspeciales.MouseDerecho)
            return KeyCode.Mouse1;
        else if (tecla == TeclasEspeciales.MouseIzquiero)
            return KeyCode.Mouse0;
        else if (tecla == TeclasEspeciales.MouseRueda)
            return KeyCode.Mouse2;
        else return KeyCode.None;
    }

}