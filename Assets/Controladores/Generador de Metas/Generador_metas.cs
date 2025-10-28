using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Genera un conjunto aleatorio de metas a partir de una lista
/// de objetos arrastrados en el Inspector. Permite configurar la cantidad mínima
/// y máxima de metas y si se generan al iniciar la escena.
/// </summary>
public class GeneradorMetasAleatorio : MonoBehaviour
{
    [Header("Posibles metas (arrastrar GameObjects del mapa)")]
    [Tooltip("Arrastrá acá todos los puntos/lugares posibles del mapa (sus Transforms).")]
    [SerializeField] private List<Transform> posiblesMetas = new List<Transform>();

    [Header("Cantidad de metas a generar")]
    [Min(0)][SerializeField] private int minMetas = 2;
    [Min(0)][SerializeField] private int maxMetas = 3;

    [Header("Opciones")]
    [SerializeField] private bool permitirRepetidas = false;
    [SerializeField] private bool generarAlIniciar = true;

    [Header("Metas generadas (solo lectura en runtime)")]
    [SerializeField] private List<Transform> transformMetasSeleccionadas = new List<Transform>();

    [Header("UI")]
    [Tooltip("Texto donde se mostrarán las metas activas.")]
    [SerializeField] private TextMeshProUGUI textoMetas;
    [SerializeField] GameObject PanelMetas;



    public List<Transform> TransformMetasSeleccionadas => transformMetasSeleccionadas;

    private void OnValidate()
    {
        if (maxMetas < minMetas) maxMetas = minMetas;

        if (!permitirRepetidas && posiblesMetas != null)
        {
            if (maxMetas > posiblesMetas.Count) maxMetas = posiblesMetas.Count;
            if (minMetas > posiblesMetas.Count) minMetas = posiblesMetas.Count;
        }
    }

    
    private void Start()
    {
        if (generarAlIniciar) GenerarMetas();
    }

    public void ShowMetas()
    {
        PanelMetas.SetActive(!PanelMetas.activeSelf);
        ActualizarTextoMetas();
    }
    public void GenerarMetas()
    {
        transformMetasSeleccionadas.Clear();

        if (posiblesMetas == null || posiblesMetas.Count == 0)
        {
            Debug.LogWarning("[GeneradorMetasAleatorio] No hay posibles metas cargadas en la lista.");
            return;
        }

        int min = Mathf.Clamp(minMetas, 0, permitirRepetidas ? int.MaxValue : posiblesMetas.Count);
        int max = Mathf.Clamp(maxMetas, min, permitirRepetidas ? int.MaxValue : posiblesMetas.Count);

        int cantidad = UnityEngine.Random.Range(min, max + 1);

        if (permitirRepetidas)
        {
            for (int i = 0; i < cantidad; i++)
                transformMetasSeleccionadas.Add(posiblesMetas[UnityEngine.Random.Range(0, posiblesMetas.Count)]);
        }
        else
        {
            List<Transform> copia = new List<Transform>(posiblesMetas);
            BarajarEnLugar(copia);
            for (int i = 0; i < cantidad; i++)
                transformMetasSeleccionadas.Add(copia[i]);
        }

        // 🔹 Desactivar todas
        foreach (var meta in posiblesMetas)
        {
            if (meta != null)
                meta.gameObject.SetActive(false);
        }

        // 🔹 Activar solo las seleccionadas
        foreach (var meta in transformMetasSeleccionadas)
        {
            if (meta != null)
                meta.gameObject.SetActive(true);
        }

        // 🔹 Actualizar texto en pantalla
        ActualizarTextoMetas();
    }

    public void ActualizarTextoMetas()
    {
        if (textoMetas == null) return;

        if (transformMetasSeleccionadas.Count == 0)
        {
            textoMetas.text = "No hay metas activas.";
            return;
        }

        string texto = "\n";
        for (int i = 0; i < transformMetasSeleccionadas.Count; i++)
        {
            var meta = transformMetasSeleccionadas[i];
            if (meta != null)
            {
                meta.TryGetComponent(out MetaCollider metaencontrada);

                if(metaencontrada != null && !metaencontrada.GetComponentInChildren<Meta>().Resolved)
                texto += $"• {metaencontrada.Meta.Nombre}\n";
            }
        }

        textoMetas.text = texto.TrimEnd();
    }

    public bool TieneMetas() => transformMetasSeleccionadas != null && transformMetasSeleccionadas.Count > 0;

    public Transform GetMeta(int index)
    {
        if (!TieneMetas() || index < 0 || index >= transformMetasSeleccionadas.Count) return null;
        return transformMetasSeleccionadas[index];
    }

    private static void BarajarEnLugar(List<Transform> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }
    }

    /// <summary>
    /// Devuelve la lista de Metas
    /// </summary>
    /// <returns></returns>
    public List<Meta> MetasSeleccionadasRandom()
    {
        var lista = new List<Meta>();
        foreach(var meta in transformMetasSeleccionadas)
        {
            lista.Add(meta.GetComponent<MetaCollider>().GetComponentInChildren<Meta>());
        }

        return lista;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (transformMetasSeleccionadas == null) return;
        for (int i = 0; i < transformMetasSeleccionadas.Count; i++)
        {
            var t = transformMetasSeleccionadas[i];
            if (t == null) continue;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(t.position, 1.0f);
#if UNITY_EDITOR
            UnityEditor.Handles.Label(t.position + Vector3.up * 1.2f, $"Meta {i + 1}");
#endif
        }
    }
#endif
}
