using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoliceCheckpoint : MonoBehaviour
{
    public GameObject stopSignUI;
    public GameObject questionPanel;
    public Text questionText;

    public List<string> questions = new List<string>
    {
        "¿Tiene su licencia de conducir?",
        "¿Ha consumido alcohol?",
        "¿Tiene los papeles del vehículo?",
        "¿Viene solo?"
    };

    private bool playerInZone = false;
    private int currentQuestion = 0;

    private void Start()
    {
        Debug.Log("✅ Script PoliceCheckpoint está funcionando.");
        stopSignUI.SetActive(false);
        questionPanel.SetActive(false);
    }

    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("🟡 Tecla E presionada: mostrando siguiente pregunta");
            ShowNextQuestion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("🔵 Jugador entró al control policial");
        stopSignUI.SetActive(true);
        playerInZone = true;
        if (other.CompareTag("Player"))
        {
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🔴 Jugador salió del control policial");
            stopSignUI.SetActive(false);
            questionPanel.SetActive(false);
            playerInZone = false;
            currentQuestion = 0;
        }
    }

    private void ShowNextQuestion()
    {
        if (currentQuestion < questions.Count)
        {
            Debug.Log($"📝 Mostrando pregunta {currentQuestion + 1}: {questions[currentQuestion]}");
            questionPanel.SetActive(true);
            questionText.text = questions[currentQuestion];
            currentQuestion++;
        }
        else
        {
            Debug.Log("✅ Se terminaron las preguntas");
            questionPanel.SetActive(false);
        }
    }
}
