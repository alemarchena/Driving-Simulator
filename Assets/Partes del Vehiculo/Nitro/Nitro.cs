using UnityEngine;
using System.Collections;

public class Nitro : Simulator
{
    [SerializeField] private float multiplicadorNitro = 1f;
    [SerializeField] private Creadores creadores = Creadores.Diaz_Corvalan_Matias_Federico;

    // Duraciones
    [SerializeField] private float nitroDuracion = 3f;
    [SerializeField] private float cooldownDuracion = 5f;

    // Estados
    private bool nitroActivo = false;
    private bool enCooldown = false;

    private void Awake()
    {
        creadores = Creadores.Diaz_Corvalan_Matias_Federico;
    }

    private void Start()
    {
        AsignarCreador(creadores);
        AsignarComandos();
        Describir();
    }

    public override void AsignarCreador(Creadores creador)
    {
        CreadoresSimulator = creador;
    }

    private void Update()
    {
        // Detectar pulsación
        if (SePresionoLaTecla() && !nitroActivo && !enCooldown)
        {
            StartCoroutine(NitroBoost());
        }
    }

    private IEnumerator NitroBoost()
    {
        nitroActivo = true;
        multiplicadorNitro = 2f;
        Debug.Log("[DEBUG] Nitro activado: multiplicador = 2");

        yield return new WaitForSeconds(nitroDuracion);

        multiplicadorNitro = 1f;
        nitroActivo = false;
        enCooldown = true;
        Debug.Log("[DEBUG] Nitro terminado: multiplicador = 1. Cooldown iniciado");

        yield return new WaitForSeconds(cooldownDuracion);

        enCooldown = false;
        Debug.Log("[DEBUG] Cooldown terminado: listo para nuevo nitro");
    }

    public override void Describir()
    {
        // Descripción opcional
    }

    /// <summary>
    /// Devuelve el valor actual del multiplicador de nitro.
    /// </summary>
    public float GetNitroMult()
    {
        return multiplicadorNitro;
    }
}
