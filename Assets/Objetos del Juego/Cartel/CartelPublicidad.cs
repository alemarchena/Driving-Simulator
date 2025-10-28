using UnityEngine;

public class CartelPublicidad : MonoBehaviour
{
    public Texture[] imagenes; // Las dos imágenes
    public float tiempoCambio = 4f; // Tiempo entre cambios
    private Renderer rend;
    private int index = 0;
    private float tiempoActual = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (imagenes.Length > 0)
        {
            rend.material.mainTexture = imagenes[0];
        }
    }

    void Update()
    {
        tiempoActual += Time.deltaTime;
        if (tiempoActual >= tiempoCambio)
        {
            index = (index + 1) % imagenes.Length;
            rend.material.mainTexture = imagenes[index];
            tiempoActual = 0f;
        }
    }
}
