using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="ControladorInfracciones",menuName = "Scriptable Objects/Infracciones/Controlador", order =1)]
public class ControladorInfracciones : ScriptableObject
{
    private List<Infraccion> infracciones;
    public List<Infraccion> Infracciones => infracciones;

    public void AgregarInfraccion(Infraccion nueva)
    {
        if (nueva == null) return;

        var campo = new SerializedObject(nueva);
        var infraccionField = campo.FindProperty("infraccion");

        var imagen = infraccionField.FindPropertyRelative("imagenInfraccion");
        var gesto = infraccionField.FindPropertyRelative("gestoInfraccion");

        if (imagen.objectReferenceValue == null)
        {
            Debug.LogWarning($"La infracción '{nueva.name}' no tiene la imagen de la infracción y no será agregada.");
            return;
        }
        if (imagen.objectReferenceValue == null || gesto.objectReferenceValue == null)
        {
            Debug.LogWarning($"La infracción '{nueva.name}' no tiene la imagen del gesto y no será agregada.");
            return;
        }
        infracciones.Add(nueva);
    }

    #if UNITY_EDITOR
        public void AgregarManual(Infraccion nueva)
        {
            if (!infracciones.Contains(nueva))
                infracciones.Add(nueva);
        }

        public void EliminarManual(int index)
        {
            if (index >= 0 && index < infracciones.Count)
                infracciones.RemoveAt(index);
        }
#endif

    public List<Infraccion> GetInfracciones()
    {
        return new List<Infraccion>(infracciones); // devolver copia para evitar modificaciones externas
    }

    public float TotalInfracciones()
    {
        float total = 0;
        foreach(var infraccion in infracciones)
        {
            total += infraccion.Datos.monto;
        }
        return total;
    }
    public void LimpiarInfracciones()
    {
        infracciones.Clear();
    }
}
