using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Renderer rend;

    public BarreraController nextBarrera; // Referencia a la barrera del siguiente checkpoint

    void Start()
    {
        rend = GetComponent<Renderer>();
        SetDefaultColor();
    }

    public void SetActiveColor()
    {
        if (rend != null)
        {
            rend.material.color = Color.green;

            // Si hay una barrera asociada al siguiente checkpoint, bájala
            if (nextBarrera != null)
            {
                nextBarrera.BajarBarrera();
            }
        }else
        {
            rend = GetComponent<Renderer>();
        }
    }

    public void SetDefaultColor()
    {
        if (rend != null)
        {
            rend.material.color = Color.white;
        }else
        {
            rend = GetComponent<Renderer>();
        }
    }
}