using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="ControladorInfracciones",menuName = "Scriptable Objects/Infracciones/Controlador", order =1)]
public class ControladorInfracciones : ScriptableObject
{
    private List<Infraccion> infracciones;

    List<Infraccion> infraccionesPlayer = new List<Infraccion>();

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
            Debug.LogWarning($"La infracci�n '{nueva.name}' no tiene la imagen de la infracci�n y no ser� agregada.");
            return;
        }
        if (imagen.objectReferenceValue == null || gesto.objectReferenceValue == null)
        {
            Debug.LogWarning($"La infracci�n '{nueva.name}' no tiene la imagen del gesto y no ser� agregada.");
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


    public void AgregarInfraccionAlPlayer(Infraccion nueva)
    {
        if (nueva == null)
        {
            Debug.LogWarning("Infracci�n nula. No se puede agregar.");
            return;
        }

        if (!infraccionesPlayer.Contains(nueva))
        {
            infraccionesPlayer.Add(nueva);
            Debug.Log($"Infracci�n agregada al jugador: {nueva.name}");
            // Aqu� podr�as disparar un evento si quer�s reaccionar en UI, por ejemplo.
        }
        else
        {
            Debug.Log($"El jugador ya tiene esta infracci�n: {nueva.name}");
        }
    }

    public List<Infraccion> GetInfraccionesDelPlayer()
    {
        return new List<Infraccion>(infraccionesPlayer); // devolver copia para evitar modificaciones externas
    }

    public void LimpiarInfraccionesDelPlayer()
    {
        infraccionesPlayer.Clear();
    }
}
