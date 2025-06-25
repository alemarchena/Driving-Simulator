using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivadorNivel : Simulator
{
    [SerializeField] private Creadores creadores = Creadores.Rada_Lizarraga_Lucas;
    public GameObject siguienteNivel;

    private void Start()
    {
        AsignarCreador(creadores);
    }
    private void OnTriggerEnter(Collider otro)
    {
        otro.gameObject.TryGetComponent(out Vehiculo vehiculo);
        if (vehiculo)
        {
            Debug.Log("Vehículo entró a nivel: " + name);
            ActivarObjeto(siguienteNivel);
        }
    }

    private void ActivarObjeto(GameObject nivel)
    {
        if (nivel != null)
            nivel.SetActive(true);
    }

    public override void AsignarCreador(Creadores creador)
    {
        CreadoresSimulator = creador;
    }
}
