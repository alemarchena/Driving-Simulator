using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;
    public GameObject gameOverPanel;
    public GameObject gameOverTextWin;
    [SerializeField] Color colorWin;
    public GameObject gameOverTextLose;
    [SerializeField] Color colorLose;
    public TextMeshProUGUI gameOverMensaje;
    private bool isGameOver = false;

    public bool IsGameOver => isGameOver;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        MessageDisplaySystem.instance.ShowMessage("Driving Simulator", 1f, 0f);
        MessageDisplaySystem.instance.ShowMessage("Enciende el motor", 1f, 2f);
    }

    /// <summary>
    /// Establece con true que gano y con false que perdio el juego
    /// </summary>
    /// <param name="state"></param>
    public void GameOver(bool state,string mensaje)
    {
        if (isGameOver) return;
        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        gameOverPanel.TryGetComponent(out Image image);
    
        if(image != null)
        {
            image.color = state ? colorWin : colorLose;
            ActiveTexts(state);
            gameOverMensaje.text = mensaje;
        }

    }

    public void ReiniciarNivel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ActiveTexts(bool state)
    {
        gameOverTextWin.SetActive(state);
        gameOverTextLose.SetActive(!state);

    }
} 