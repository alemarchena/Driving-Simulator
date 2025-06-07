using UnityEngine;

public class MotorAudio : MonoBehaviour
{
    [SerializeField] AudioSource motorSource;
    [SerializeField] AudioClip motorClip;

    [SerializeField] float minPitch = 0.8f;
    [SerializeField] float maxPitch = 3.0f;

    [Range(0, 1)]
    [SerializeField] float volumen = 1f;

    [SerializeField] float rpmActual; // Este valor deberás obtenerlo del sistema de motor

    [SerializeField] private float rpmMin;
    [SerializeField] private float rpmMax;

    [SerializeField] Motor motor;

    private void SetRPM()
    {
        if(motor != null)
        {
            rpmMin = motor.RPMminima;
            rpmMax = motor.RPMmaxima;
        }else
        {
            Debug.LogError("Falta asignar el Motor");
        }

    }


    void Start()
    {
        SetRPM();
        motorSource.clip = motorClip;
        motorSource.loop = true;
        motorSource.volume = 0;
        motorSource.Play();
    }

    void Update()
    {
        if (motor.MotorEncendido)
        {
            rpmActual = motor.RPMactual;
            float t = Mathf.InverseLerp(rpmMin, rpmMax, rpmActual);
            motorSource.pitch = Mathf.Lerp(minPitch, maxPitch, t);
            motorSource.volume = volumen;
        }else
        {
            motorSource.volume = 0;
        }
    }
}
