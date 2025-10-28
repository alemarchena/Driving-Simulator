using UnityEngine;
using System.Collections;

public class MotorAudio : MonoBehaviour
{
    [Header("🎧 Configuración de audio")]
    [SerializeField] private AudioSource motorSource;
    [SerializeField] private AudioClip motorClip;

    [Header("⚙️ Configuración de pitch")]
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 2.5f;
    [SerializeField] private float maxPitchNitro = 3.0f;

    [Header("🔊 Volumen general")]
    [Range(0, 1)]
    [SerializeField] private float volumen = 1f;
    [SerializeField] private float tiempoFade = 1.5f; // Duración del fade-in/out

    [Header("🔇 Control desde el editor")]
    [SerializeField] private bool muteFromEditor = false;

    [Header("📈 RPM")]
    [SerializeField] private float rpmActual;
    [SerializeField] private float rpmMin;
    [SerializeField] private float rpmMax;

    [SerializeField] private Motor motor;

    private float originalVolume;
    private bool isMuted = false;
    private Coroutine fadeCoroutine;

    private void SetRPM()
    {
        if (motor != null)
        {
            rpmMin = motor.RPMminima;
            rpmMax = motor.RPMmaxima;
        }
        else
        {
            Debug.LogError("Falta asignar el Motor al componente MotorAudio.");
        }
    }

    void Start()
    {
        SetRPM();
        motorSource.clip = motorClip;
        motorSource.loop = true;
        motorSource.volume = 0;
        motorSource.Play();
        originalVolume = volumen;
    }

    void Update()
    {
        // Control de mute manual desde el editor
        if (muteFromEditor && !isMuted)
        {
            Mute();
        }
        else if (!muteFromEditor && isMuted)
        {
            Restore();
        }

        if (motor == null) return;

        if (motor.MotorEncendido)
        {
            rpmActual = motor.RPMactual;
            float t = Mathf.InverseLerp(rpmMin, rpmMax, rpmActual);

            motorSource.pitch = motor.Nitro.NitroActivo
                ? Mathf.Lerp(minPitch, maxPitchNitro, t)
                : Mathf.Lerp(minPitch, maxPitch, t);

            // Si el motor se acaba de encender y no hay fade, inicia fade-in
            if (motorSource.volume < volumen * originalVolume && fadeCoroutine == null)
                fadeCoroutine = StartCoroutine(FadeVolume(volumen, tiempoFade));
        }
        else
        {
            // Si el motor se apaga, inicia fade-out
            if (motorSource.volume > 0.05f && fadeCoroutine == null)
                fadeCoroutine = StartCoroutine(FadeVolume(0f, tiempoFade));
        }
    }

    // 🔇 Mutea el audio instantáneamente
    public void Mute()
    {
        if (isMuted) return;
        originalVolume = motorSource.volume;
        motorSource.volume = 0f;
        isMuted = true;
    }

    // 🔊 Restaura el volumen original
    public void Restore()
    {
        if (!isMuted) return;
        motorSource.volume = originalVolume;
        isMuted = false;
    }

    // 🎚️ Corrutina para cambiar el volumen suavemente
    private IEnumerator FadeVolume(float targetVolume, float duration)
    {
        float startVolume = motorSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            motorSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        motorSource.volume = targetVolume;
        fadeCoroutine = null;
    }
}
