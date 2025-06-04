using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Option Data")]
public class Data : ScriptableObject
{
    public List<Control> controles;

    Dictionary<string, List<KeyCode>> diccionarioTeclas = new Dictionary<string, List<KeyCode>>();


    /// <summary>
    /// Devuelve un diccionario compuesto por la clave : parte + subparte y la lista de teclas de la parte-subparte
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, List<KeyCode>> GetComandos()
    {

        foreach (Control entrada in controles)
        {
            string clave = entrada.parteSubparte.parte.ToString() + entrada.parteSubparte.subParte.ToString();

            if (!diccionarioTeclas.ContainsKey(clave))
                diccionarioTeclas[clave] = new List<KeyCode>();

            foreach (KeyCode tecla in entrada.teclas)
            {
                if (!diccionarioTeclas[clave].Contains(tecla)) // evitar duplicados
                {
                    diccionarioTeclas[clave].Add(tecla);
                }
            }
        }

        return diccionarioTeclas;
    }
}