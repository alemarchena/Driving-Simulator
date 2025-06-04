using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ControladorInfracciones))]
public class ControladorInfraccionesEditor : Editor
{
    Infraccion infraccionParaAgregar;
    bool mostrarLista = true; // ← nuevo estado local del inspector

    public override void OnInspectorGUI()
    {
        ControladorInfracciones controlador = (ControladorInfracciones)target;

        // SECCIÓN: Agregar nueva infracción
        EditorGUILayout.LabelField("Agregar nueva infracción", EditorStyles.boldLabel);
        infraccionParaAgregar = (Infraccion)EditorGUILayout.ObjectField("Nueva Infracción", infraccionParaAgregar, typeof(Infraccion), false);

        GUI.enabled = infraccionParaAgregar != null;

        if (GUILayout.Button("Agregar Infracción a la lista"))
        {
            var so = new SerializedObject(infraccionParaAgregar);
            var infraccionField = so.FindProperty("infraccion");

            var imagen = infraccionField.FindPropertyRelative("imagenInfraccion");
            var gesto = infraccionField.FindPropertyRelative("gestoInfraccion");

            if (imagen.objectReferenceValue == null || gesto.objectReferenceValue == null)
            {
                EditorUtility.DisplayDialog(
                    "Error al agregar infracción",
                    $"La infracción \"{infraccionParaAgregar.name}\" no tiene todas las imágenes asignadas.",
                    "Entendido"
                );
            }
            else
            {
                Undo.RecordObject(controlador, "Agregar infracción");
                controlador.AgregarManual(infraccionParaAgregar);
                EditorUtility.SetDirty(controlador);
                infraccionParaAgregar = null;
            }
        }

        GUI.enabled = true;

        EditorGUILayout.Space(20);

        // BOTÓN para expandir/colapsar la lista
        mostrarLista = EditorGUILayout.Foldout(mostrarLista, "Lista de Infracciones", true, EditorStyles.foldoutHeader);

        if (mostrarLista)
        {
            if (controlador.Infracciones != null && controlador.Infracciones.Count > 0)
            {
                for (int i = 0; i < controlador.Infracciones.Count; i++)
                {
                    var infraccion = controlador.Infracciones[i];

                    if (infraccion == null)
                    {
                        EditorGUILayout.HelpBox($"Elemento {i} está vacío.", MessageType.Warning);
                        continue;
                    }

                    SerializedObject so = new SerializedObject(infraccion);
                    SerializedProperty infraccionProp = so.FindProperty("infraccion");

                    string nombre = infraccionProp.FindPropertyRelative("nombre")?.stringValue ?? "(Sin nombre)";
                    var tipo = infraccionProp.FindPropertyRelative("TypeInfraccion").enumDisplayNames[infraccionProp.FindPropertyRelative("TypeInfraccion").enumValueIndex];
                    var img = infraccionProp.FindPropertyRelative("imagenInfraccion").objectReferenceValue;
                    var gesto = infraccionProp.FindPropertyRelative("gestoInfraccion").objectReferenceValue;

                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField($"Nombre: {nombre}", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField($"Tipo: {tipo}");

                    EditorGUILayout.LabelField("Imagen:", img ? "✔️ Asignada" : "❌ Faltante");
                    EditorGUILayout.LabelField("Gesto:", gesto ? "✔️ Asignado" : "❌ Faltante");

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Seleccionar"))
                    {
                        Selection.activeObject = infraccion;
                    }
                    if (GUILayout.Button("Eliminar"))
                    {
                        Undo.RecordObject(controlador, "Eliminar infracción");
                        controlador.EliminarManual(i);
                        EditorUtility.SetDirty(controlador);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No hay infracciones en la lista.", MessageType.Info);
            }
        }
    }
}
