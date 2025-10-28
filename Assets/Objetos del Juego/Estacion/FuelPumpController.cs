using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FuelPumpController : MonoBehaviour
{
    [Header("Paneles y UI")]
    [SerializeField] private List<GameObject> panelesCarga;
    [SerializeField] private GameObject panelAdvertencia;
    [SerializeField] private TMP_Dropdown dropdownNafta;
    [SerializeField] private TMP_Text cantidadText;
    [SerializeField] private TMP_Text textoSeleccionado;

    [Header("Botones")]
    [SerializeField] private Button botonConfirmar;
    [SerializeField] private Button botonPorPeso;
    [SerializeField] private Button botonPorLitro;
    [SerializeField] private Button botonLlenarTanque;
    [SerializeField] private Button botonSumar;
    [SerializeField] private Button botonRestar;

    private GameObject auto;
    private FuelTank tanque;
    private Motor motor;

    private enum ModoCarga { PorPeso, PorLitro, Llenar }
    private ModoCarga modoActual = ModoCarga.PorPeso;

    private float cantidad = 0f;
    private float velocidadCarga = 1f;
    private float[] preciosNafta = new float[] { 100f, 150f, 200f };
    private Dictionary<int, float> stockNafta = new Dictionary<int, float>();

    private float plataJugador = 1000f;
    private float plataInicial = 1000f;

    private void Start()
    {

        foreach(var panel in panelesCarga)
        {
            panel.SetActive(false);
        }

        panelAdvertencia.SetActive(false);

        botonPorPeso.onClick.AddListener(() => CambiarModo(ModoCarga.PorPeso, botonPorPeso,"Carga por dinero"));
        botonPorLitro.onClick.AddListener(() => CambiarModo(ModoCarga.PorLitro, botonPorLitro, "Carga por litro"));
        botonLlenarTanque.onClick.AddListener(() => CambiarModo(ModoCarga.Llenar, botonLlenarTanque, "Carga por tanque lleno"));


        botonSumar.onClick.AddListener(() => cantidad += velocidadCarga);
        botonRestar.onClick.AddListener(() => cantidad = Mathf.Max(0, cantidad - velocidadCarga));

        botonConfirmar.onClick.AddListener(ConfirmarCarga);

        // Inicializar stock aleatorio para cada tipo de nafta
        for (int i = 0; i < preciosNafta.Length; i++)
        {
            stockNafta[i] = Random.Range(i == 0 ? 300 : 0, 1000);
        }
    }

    private void CambiarModo(ModoCarga nuevoModo, Button botonAsociado,string tipoCarga)
    {
        modoActual = nuevoModo;
        velocidadCarga = nuevoModo == ModoCarga.PorPeso ? 10f : 1f;

        if (nuevoModo == ModoCarga.Llenar && tanque != null)
        {
            float porcentaje = tanque.GetFuelPercentage();
            cantidad = Mathf.Round((1f - porcentaje) * 50f); // 50f = capacidad estándar
        }

        textoSeleccionado.text = tipoCarga;
        StartCoroutine(HighlightButtonTemporal(botonAsociado,  3f));

    }

    private IEnumerator HighlightButtonTemporal(Button boton, float duracion)
    {
        Image img = boton.GetComponent<Image>();
        if (img == null) yield break;

        // 🎨 Colores base
        Color colorActivo = Color.green;
        ColorBlock colors = boton.colors;
        Color colorOriginal = colors.normalColor; // ✅ Usa el color del bloque original

        // 🔒 Desactiva transición temporalmente
        var transicionOriginal = boton.transition;
        boton.transition = Selectable.Transition.None;

        // 🟢 Activa color verde
        img.color = colorActivo;

        // Espera el tiempo de activación
        yield return new WaitForSeconds(duracion);

        // 🔄 Fade de regreso al color original
        float tiempoFade = 0.5f;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / tiempoFade;
            img.color = Color.Lerp(colorActivo, colorOriginal, t);
            yield return null;
        }

        // ✅ Asegura color final y reactiva transición original
        img.color = colorOriginal;
        boton.transition = transicionOriginal;

        // 🔁 Restablece ColorBlock (por seguridad, si el botón usa Color Tint)
        colors.normalColor = colorOriginal;
        boton.colors = colors;
    }




    private void OnTriggerStay(Collider other)
    {
        other.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo == null) return;

        auto = other.gameObject;
        tanque = auto.GetComponentInChildren<FuelTank>();
        motor = auto.GetComponentInChildren<Motor>();

        if (tanque == null || motor == null) return;

        if (!motor.MotorEncendido && auto.GetComponent<Rigidbody>().linearVelocity.magnitude <= 0.1f)
        {
            panelAdvertencia.SetActive(false);
            foreach (var panel in panelesCarga)
            {
                panel.SetActive(true);
            }
        }
        else
        {
            foreach (var panel in panelesCarga)
            {
                panel.SetActive(false);
            }
            panelAdvertencia.SetActive(true);
        }

        cantidadText.text = $"{cantidad:0.0}";
    }

    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo == null) return;

        panelAdvertencia.SetActive(false);
        foreach (var panel in panelesCarga)
        {
            panel.SetActive(false);
        }
    }

    private void ConfirmarCarga()
    {
        if (tanque == null) return;

        int tipo = dropdownNafta.value;
        float precioPorLitro = preciosNafta[tipo];

        float litrosACargar = 0f;
        float costo = 0f;

        if (modoActual == ModoCarga.PorPeso)
        {
            costo = cantidad;
            litrosACargar = costo / precioPorLitro;
        }
        else if (modoActual == ModoCarga.PorLitro)
        {
            litrosACargar = cantidad;
            costo = litrosACargar * precioPorLitro;
        }
        else if (modoActual == ModoCarga.Llenar)
        {
            float porcentaje = tanque.GetFuelPercentage();
            litrosACargar = Mathf.Round((1f - porcentaje) * 50f);
            costo = litrosACargar * precioPorLitro;
        }

        if (stockNafta[tipo] < litrosACargar)
        {
            Debug.Log("⛽ No hay suficiente stock.");
            return;
        }

        if (plataJugador < costo)
        {
            Debug.Log("💸 Sin dinero. Regenerando saldo.");
            plataJugador = plataInicial;
            //return;
        }

        // Cargar combustible usando método público
        tanque.Refuel(litrosACargar);
        stockNafta[tipo] -= litrosACargar;
        plataJugador -= costo;

        // Reabastecer stock si es necesario
        if (tipo == 0 && stockNafta[tipo] < 100)
            stockNafta[tipo] = Random.Range(300, 1000);
        else if (stockNafta[tipo] <= 0)
            stockNafta[tipo] = Random.Range(0, 1000);

        cantidad = 0f;
        foreach (var panel in panelesCarga)
        {
            panel.SetActive(false);
        }
    }
}
