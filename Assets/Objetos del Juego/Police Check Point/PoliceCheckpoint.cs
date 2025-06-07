using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class PoliceCheckpoint : MonoBehaviour
{
    [SerializeField] GameObject stopSignUI;
    [SerializeField] GameObject questionPanel;
    [SerializeField] TextMeshProUGUI questionText;

    // --- NUEVO: Referencias a los botones y sus textos ---
    [SerializeField] Button answerButtonA;
    [SerializeField] Button answerButtonB;
    [SerializeField] TextMeshProUGUI answerButtonAText;
    [SerializeField] TextMeshProUGUI answerButtonBText;

    bool    preguntasActivas = false;
    int     preguntasRespondidas = 0;

    [Header("Agente de Policia")]
    [SerializeField] Policia policia;

    [Header("Preguntas del control policial")]
    public List<Pregunta> preguntas = new List<Pregunta>();


    private bool playerInZone = false;
    private int currentQuestion = 0;

    private void Start()
    {
        stopSignUI.SetActive(false);
        questionPanel.SetActive(false); 

        if (answerButtonA != null)
        {
            answerButtonA.onClick.AddListener(() => HandleAnswer(true)); // true para opción A
        }
        if (answerButtonB != null)
        {
            answerButtonB.onClick.AddListener(() => HandleAnswer(false)); // false para opción B
        }
    }

    private void Update()
    {
        if (playerInZone && policia.PoliciaAlFrente && !questionPanel.activeSelf)
        {
            if (preguntasRespondidas == preguntas.Count)
            {
                questionPanel.SetActive(false);
                return;
            }
            preguntasActivas = true;

            stopSignUI.SetActive(false); 
            currentQuestion = 0;
            ShowNextQuestion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo)
        {
            stopSignUI.SetActive(true);
            playerInZone = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent(out Vehiculo vehiculo);

        if (vehiculo) 
        { 
            stopSignUI.SetActive(false);
            questionPanel.SetActive(false); 
            playerInZone = false;
            currentQuestion = 0; // Resetear
            
            if (preguntasActivas) {
                if (preguntasRespondidas < preguntas.Count)
                {
                    //Cobrar multa por escapar
                    Debug.Log("Multa por escapar..");
                }
                preguntasRespondidas=0;
                preguntasActivas = false;
            }
        }

    }

    private void ShowNextQuestion()
    {
        if (currentQuestion < preguntas.Count)
        {
            Pregunta pregunta = preguntas[currentQuestion];

            questionText.text = pregunta.texto;

            answerButtonAText.text = pregunta.opcionA;
            answerButtonBText.text = pregunta.opcionB;

            // --- NUEVO: Actualizar texto de los botones de respuesta ---
            //if (answerButtonAText != null && currentQuestion < answerOptionsA.Count)
            //{
            //    answerButtonAText.text = answerOptionsA[currentQuestion];
            //}
            //if (answerButtonBText != null && currentQuestion < answerOptionsB.Count)
            //{
            //    answerButtonBText.text = answerOptionsB[currentQuestion];
            //}
           
            questionPanel.SetActive(true); 
        }
        else
        {
            questionPanel.SetActive(false); // Ocultar el panel al terminar

        }
    }

    // --- NUEVO: Método para manejar la respuesta seleccionada ---
    private void HandleAnswer(bool isOptionA)
    {
        Pregunta pregunta = preguntas[currentQuestion];
        bool esCorrecta = (isOptionA == pregunta.opcionACorrecta);

        preguntasRespondidas += 1;

        if (!esCorrecta)
        {
            //Cobrar Multa
            Debug.Log("Tiene una multa...");
        }
       

        currentQuestion++; // Avanzar a la siguiente pregunta
        ShowNextQuestion(); // Mostrar la siguiente pregunta o finalizar
    }
}

[System.Serializable]
public class Pregunta
{
    public string texto;
    public string opcionA;
    public string opcionB;
    public bool opcionACorrecta;
}