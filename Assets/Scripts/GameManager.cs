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
    [SerializeField] private GameObject playerPrefab;
    private bool isGamePaused = false; // Variable to track if the game has been paused
    private bool isGameStarted = false; // Variable to track if the game has started
    public static GameManager instance;
    public GameObject player;
    

    private void Awake()
    {
     if(instance == null)
     {
        instance = this;
        DontDestroyOnLoad(gameObject);
     }
     else
     {
        Destroy(gameObject);
     }
    }
    
    void Start()
    {
        isGameStarted = false;
    }

    void Update()
    {
        if(isGameStarted) // Only allow pausing if the game has started
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

    // Functionality for Pausing the game
    private void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        Debug.Log("Game paused!");
    }

    // Functionality for Resuming the game
    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        Debug.Log("Game resumed!");
    }

    // Functionality for the Play button in order to start playing the game!
    public void PlayGame()
    {
        playPanelUI.SetActive(false);
        isGameStarted = true; // Set the game as started when the play button is clicked
        player = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
    }

    // Functionality for the Quit button in order to quit the game!
    public void QuitGame()
    {
        Application.Quit();
    }
}
