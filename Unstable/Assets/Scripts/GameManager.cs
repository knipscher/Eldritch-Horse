using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameOver = false;

    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject losePanel;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI highScoreText;

    [SerializeField]
    private HorseTracker horseTracker;

    [SerializeField]
    private TextMeshProUGUI timeText;

    public float time { get; private set; }

    public int playerScore { get; private set; }
    public int playerHighScore { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        SetScore(0);

        playerHighScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = "High score: " + playerHighScore;
    }

    private void Update()
    {
        time += Time.deltaTime;
        timeText.text = Mathf.RoundToInt(time).ToString();
    }

    public void BeatLevel()
    {
        SceneChangeManager.instance.LoadNextScene();
    }

    public void Win()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            horseTracker.focusOnHorse = true;
            winPanel.SetActive(true);
        }
    }

    public void Lose()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            losePanel.SetActive(true);
            if (playerScore > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", playerScore);
            }
        }
    }

    public void AddToScore(int scoreToAdd)
    {
        SetScore(playerScore + scoreToAdd);
    }

    public void MultiplyScoreByDifficulty()
    {
        if (PlayerPrefs.GetInt("Difficulty") == 1)
        {
            playerScore *= 2;
        }
        else if (PlayerPrefs.GetInt("Difficulty") == 1)
        {
            playerScore *= 4;
        }

    }

    public void MultiplyScoreByTimeBonus(float timeBonus)
    {
        playerScore = Mathf.RoundToInt(playerScore * timeBonus);
    }

    public void SetScore(int newScore)
    {
        playerScore = newScore;
        scoreText.text = "Score: " + playerScore;
    }
}