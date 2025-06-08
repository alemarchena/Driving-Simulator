using UnityEngine;

public class ArbolCrece : MonoBehaviour
{
    public float velocidadCrecimiento = 0.5f;
    public Vector3 escalaFinal = Vector3.one;
    private Vector3 escalaInicial;
    private float tiempo = 0;

    void Start()
    {
        escalaInicial = Vector3.zero;
        transform.localScale = escalaInicial;
    }

    void Update()
    {
        if (transform.localScale.x < escalaFinal.x)
        {
            tiempo += Time.deltaTime * velocidadCrecimiento;
            transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, tiempo);
        }
    }
}
