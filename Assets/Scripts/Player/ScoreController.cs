using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private GameObject scoreText;
    private const int _scoreValue = 5;
    private int score = 0;
    public static ScoreController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ScoreUp()
    {
        score += _scoreValue;
        Debug.Log("Score increased. Current score: " + score);
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}
