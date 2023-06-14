using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject playPanelUI;
    [SerializeField] private GameObject GamePanelUI;
    [SerializeField] private GameObject pausePanelUI;
    [SerializeField] private GameObject gameOverPanelUI;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Image[] hearts;
    private bool isGamePaused; // Variable to track if the game has been paused
    public bool IsGameStarted { get; private set; } // Variable to track if the game has started
    public bool IsGameOver { get; private set; } // Variable to track if the game is over
    public static GameManager instance;
    public GameObject player;
    

    private void Awake()
    {
     if(instance == null)
        instance = this;
    }
    
    void Start()
    {
        isGamePaused = false;
        IsGameStarted = false;
        IsGameOver = false;
    }

    void Update()
    {
        if(IsGameStarted && !IsGameOver) // Only allow pausing if the game has started
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (isGamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
        
    }

    public void HandleHealthUI(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
                hearts[i].color = i < currentHealth ? Color.white : Color.black;
        }
    }

    // Functionality for Pausing the game
    private void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        Debug.Log("Game paused!");
        pausePanelUI.SetActive(true);
    }

    // Functionality for Resuming the game
    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        Debug.Log("Game resumed!");
        pausePanelUI.SetActive(false);
    }

    // Functionality for the Play button in order to start playing the game!
    public void PlayGame()
    {
        playPanelUI.SetActive(false);
        IsGameStarted = true; // Set the game as started when the play button is clicked
        IsGameOver = false;
        player = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
        GamePanelUI.SetActive(true);
    }

    // Functionality for the Quit button in order to quit the game!
    public void QuitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        IsGameOver = true;
        gameOverPanelUI.SetActive(true);
    }
}
