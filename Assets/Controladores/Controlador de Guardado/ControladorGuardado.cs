using System;
using UnityEngine;

public class ControladorGuardado : MonoBehaviour
{

    private Transform jugador;
    [SerializeField] AudioClip clipGuardado;
  
    public void GuardarPosicion()
    {
        PlayerPrefs.SetFloat("JugadorX", jugador.localPosition.x);
        PlayerPrefs.SetFloat("JugadorY", jugador.localPosition.y);
        PlayerPrefs.SetFloat("JugadorZ", jugador.localPosition.z);
        PlayerPrefs.SetFloat("JugadorRotationX", jugador.localRotation.x);
        PlayerPrefs.SetFloat("JugadorRotationY", jugador.localRotation.y);
        PlayerPrefs.SetFloat("JugadorRotationZ", jugador.localRotation.z);

        PlayerPrefs.Save();

        if (clipGuardado != null )
        {
            ControladorSonidos.Instance.PlaySoundGlobal(clipGuardado, ControladorSonidos.ModePlay.playOneShoot, false, 0.8f);
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
         other.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo != null) {
            jugador = vehiculo.transform;
            GuardarPosicion();
        }
    }
}

