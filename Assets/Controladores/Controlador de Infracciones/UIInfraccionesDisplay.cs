using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIInfraccionesDisplay : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Transform content;      
    [SerializeField] private GameObject itemPrefab;  
    [SerializeField] private TextMeshProUGUI textTotalInfracciones;  

    private readonly List<GameObject> itemsInstanciados = new();
    private float total=0f;
    /// <summary>
    /// Limpia la lista visual actual.
    /// </summary>
    public void Limpiar()
    {
        foreach (var go in itemsInstanciados)
            Destroy(go);

        itemsInstanciados.Clear();
    }

    /// <summary>
    /// Muestra las infracciones actuales del jugador.
    /// </summary>
    public void MostrarInfracciones(List<Infraccion> infracciones)
    {
        Limpiar();
        total = 0f;
        foreach (var infr in infracciones)
        {
            GameObject nuevoItem = Instantiate(itemPrefab, content);
            nuevoItem.TryGetComponent(out ItemInfraccion itemInfraccion);

            if (itemInfraccion != null) {
                itemInfraccion.SetTextInfraccion(infr.Datos.nombre, infr.Datos.monto.ToString(), infr.Datos.TypeInfraccion.ToString());
                total += infr.Datos.monto;
            }

            textTotalInfracciones.text = total.ToString();
            itemsInstanciados.Add(nuevoItem);
        }
    }
}
