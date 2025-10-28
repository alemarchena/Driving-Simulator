using UnityEngine;

public class DetectorGolpes : MonoBehaviour
{
    [Header("Sonidos")]
    [SerializeField] AudioClip audioGolpe;
    [SerializeField] Motor motor;

    public Transform objetivo; // arrastra aquí el coche o parte del coche

    void LateUpdate()
    {
        transform.position = objetivo.position;
        transform.rotation = objetivo.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Vehiculo"))
            return;

        if (audioGolpe != null && motor != null)
        {
            if (motor.MotorEncendido)
            {
                ControladorSonidos.Instance.PlaySoundGlobal(audioGolpe, ControladorSonidos.ModePlay.play, false, 0.3f);
            }
        }else
        {
            Debug.LogWarning("Falta asignar el audio de golpe o el Motor al Detector de Golpes");
        }
    }
   
}
