using System.Collections.Generic;
using UnityEngine;

public class Volante : MonoBehaviour
{
    [SerializeField] ParteSubParte parteSubparteDerecha;
    [SerializeField] List<KeyCode> teclasComandoDerecha = new List<KeyCode>();

    [SerializeField] ParteSubParte parteSubparteIzquierda;
    [SerializeField] List<KeyCode> teclasComandoIzquierda = new List<KeyCode>();

    [SerializeField] bool sePresionoTeclaDerecha;
    [SerializeField] bool sePresionoTeclaIzquierda;

    public enum MovimientoVolante {Derecha,Izquierda,Neutro }
    private MovimientoVolante movimientoVolante;
    private void Start()
    {
        teclasComandoDerecha = ControladorComandos.instance.AsignaTeclas(parteSubparteDerecha);
        teclasComandoIzquierda = ControladorComandos.instance.AsignaTeclas(parteSubparteIzquierda);

    }

    private void Update()
    {
        sePresionoTeclaIzquierda = false;

        foreach (KeyCode kd in teclasComandoIzquierda)
        {
            if (Input.GetKey(kd))
            {
                sePresionoTeclaIzquierda = true;
                break;
            }
        }


        sePresionoTeclaDerecha = false;

        foreach (KeyCode kd in teclasComandoDerecha)
        {
            if (Input.GetKey(kd))
            {
                sePresionoTeclaDerecha = true;
                break;
            }
        }
    }

    public MovimientoVolante GiroVolante()
    {
        if ((sePresionoTeclaDerecha && sePresionoTeclaIzquierda) || (!sePresionoTeclaDerecha && !sePresionoTeclaIzquierda))
        {
            movimientoVolante = MovimientoVolante.Neutro;
        }
        else
        {
            if(sePresionoTeclaDerecha) movimientoVolante = MovimientoVolante.Derecha;
            if(sePresionoTeclaIzquierda) movimientoVolante = MovimientoVolante.Izquierda;
        }
        return movimientoVolante;
    }
}
