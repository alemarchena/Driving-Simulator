using UnityEngine;

public class DetectorGolpes : MonoBehaviour
{
    [Header("Sonidos")]
    [SerializeField] AudioClip audioGolpe;

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

        if (audioGolpe != null)
        {
            ControladorSonidos.Instance.PlaySoundGlobal(audioGolpe, ControladorSonidos.ModePlay.play, false, 0.3f);
        }
    }
   
}
