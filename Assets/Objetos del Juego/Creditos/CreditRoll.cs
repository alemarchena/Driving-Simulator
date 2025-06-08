using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;     // ← necesario para IEnumerator
using TMPro;

public class CreditRoll : MonoBehaviour
{
    [Header("Referencias UI")]
    public RectTransform content;
    public CanvasGroup canvasGroup;

    [Header("Parámetros")]
    public float velocidad = 50f;
    public float distancia = 2000f;
    public float fadeDuration = 1f;

    private float objetivoY;
    private bool started = false;

    void Start()
    {
        // Calcula punto final
        objetivoY = content.anchoredPosition.y + distancia;
        // Inicia fade-in
        StartCoroutine(Fade(0, 1));
        started = true;
    }

    void Update()
    {
        if (!started) return;

        // Mueve créditos hacia arriba
        Vector2 p = content.anchoredPosition;
        p.y += velocidad * Time.deltaTime;
        content.anchoredPosition = p;

        if (p.y >= objetivoY)
        {
            started = false;
            StartCoroutine(EndCredits());
        }
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    IEnumerator EndCredits()
    {
        // Fade-out
        yield return Fade(1, 0);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
