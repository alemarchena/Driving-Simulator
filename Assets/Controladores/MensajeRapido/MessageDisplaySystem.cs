using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MessageDisplaySystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText; 
    [SerializeField] private Image imagenDerecha;
    [SerializeField] private Image imagenIzquierda;
    [SerializeField] Animator anim;

    public static MessageDisplaySystem instance;

    private void Awake()
    {
        // Implementación del patrón Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="speedAnimation"></param>
    /// <param name="delay"></param>
    /// <param name="imagenIzquierda"></param>
    /// <param name="imagenDerecha"></param>
    public void ShowMessage(string message, float speedAnimation , float delay, Sprite imagenIzquierda = null, Sprite imagenDerecha = null)
    {
        if (instance != null)
        {
            if (displayText == null) return;
             StartCoroutine( DisplayMessageInternal(message, speedAnimation, delay,imagenIzquierda, imagenDerecha));
        }
        else
        {
            Debug.LogWarning("MessageDisplaySystem not found in scene!");
        }
    }

    IEnumerator DisplayMessageInternal(string message, float displayTime,float delay, Sprite imagenMensaje = null, Sprite imagenGesto = null)
    {
        
        yield return new WaitForSeconds(delay);

        displayText.text = message;
        
        if (imagenMensaje != null) {
            this.imagenDerecha.gameObject.SetActive(true);
            this.imagenDerecha.sprite = imagenMensaje;
        }
        else
            this.imagenDerecha.gameObject.SetActive(false);


        if (imagenGesto != null)
        {
            this.imagenIzquierda.gameObject.SetActive(true);
            this.imagenIzquierda.sprite = imagenGesto;
        }
        else
            this.imagenIzquierda.gameObject.SetActive(false);



        anim.SetTrigger("Show");
        anim.speed = displayTime > 0 ? displayTime : 1f;

        yield return null;
        
    }
}
