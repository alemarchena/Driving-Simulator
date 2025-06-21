using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntersectionController : MonoBehaviour
{
    public enum Estado { Verde, Amarillo, Rojo }

    [Header("Duraciones (segundos)")]
    public int duracionVerde = 5;
    public int duracionAmarillo = 3;

    [Header("Semáforo 1")]
    public GameObject lamparaVerde1;
    public GameObject lamparaAmarilla1;
    public GameObject lamparaRoja1;
    public Text display1;

    [Header("Semáforo 2")]
    public GameObject lamparaVerde2;
    public GameObject lamparaAmarilla2;
    public GameObject lamparaRoja2;
    public Text display2;

    private void Start()
    {
        StartCoroutine(CicloInterseccion());
    }

    private IEnumerator CicloInterseccion()
    {
        float totalRojo = duracionVerde + duracionAmarillo;

        while (true)
        {
            // 1 VERDE / 2 ROJO
            yield return Fase(
                Estado.Verde, Estado.Rojo,
                duracionVerde,
                duracionVerde, totalRojo
            );

            // 1 AMARILLO / 2 ROJO
            yield return Fase(
                Estado.Amarillo, Estado.Rojo,
                duracionAmarillo,
                duracionAmarillo, duracionAmarillo
            );

            // 1 ROJO / 2 VERDE
            yield return Fase(
                Estado.Rojo, Estado.Verde,
                duracionVerde,
                totalRojo, duracionVerde
            );

            // 1 ROJO / 2 AMARILLO
            yield return Fase(
                Estado.Rojo, Estado.Amarillo,
                duracionAmarillo,
                duracionAmarillo, duracionAmarillo
            );
        }
    }


    private IEnumerator Fase(
        Estado estado1, Estado estado2,
        float duracionFase,
        float timerInicial1, float timerInicial2
    )
    {
        SetSemaforo(lamparaVerde1, lamparaAmarilla1, lamparaRoja1, display1, estado1);
        SetSemaforo(lamparaVerde2, lamparaAmarilla2, lamparaRoja2, display2, estado2);

        float t1 = timerInicial1;
        float t2 = timerInicial2;
        float elapsed = 0f;

        while (elapsed < duracionFase)
        {
            float dt = Time.deltaTime;
            elapsed += dt;
            t1 = Mathf.Max(0f, t1 - dt);
            t2 = Mathf.Max(0f, t2 - dt);

            if (display1 != null) display1.text = Mathf.CeilToInt(t1).ToString();
            if (display2 != null) display2.text = Mathf.CeilToInt(t2).ToString();

            yield return null;
        }
    }

    private void SetSemaforo(
        GameObject verde, GameObject amarillo, GameObject rojo,
        Text display, Estado estado
    )
    {
        verde.SetActive(estado == Estado.Verde);
        amarillo.SetActive(estado == Estado.Amarillo);
        rojo.SetActive(estado == Estado.Rojo);

        if (display != null)
        {
            switch (estado)
            {
                case Estado.Verde: display.color = Color.green; break;
                case Estado.Amarillo: display.color = Color.yellow; break;
                case Estado.Rojo: display.color = Color.red; break;
            }
        }
    }
}
