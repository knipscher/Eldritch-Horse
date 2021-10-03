using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stars;
    [SerializeField]
    private float starDelayTime = 0.3f;
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Horse horse;

    [SerializeField]
    private TextMeshProUGUI difficultyMultiplierText;

    [SerializeField]
    private TextMeshProUGUI finalScoreText;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private TextMeshProUGUI newHighScoreText;

    [SerializeField]
    private TextMeshProUGUI horseDescriptionText;

    [SerializeField]
    private float maxTimeForBonus;

    private void Start()
    {
        foreach (var star in stars)
        {
            star.SetActive(false);
        }

        SetFinalScoreText();

        ScoreHorse();
    }

    private void SetFinalScoreText()
    {
        if (PlayerPrefs.GetInt("Difficulty") == 0)
        {
            difficultyMultiplierText.text = "";
        }
        else if (PlayerPrefs.GetInt("Difficulty") == 1)
        {
            difficultyMultiplierText.text = "Normal difficulty bonus = x2!";
            GameManager.instance.MultiplyScoreByDifficulty();
        }
        else if (PlayerPrefs.GetInt("Difficulty") == 2)
        {
            difficultyMultiplierText.text = "Hard difficulty bonus = x4!";
            GameManager.instance.MultiplyScoreByDifficulty();
        }

        if (GameManager.instance.time < maxTimeForBonus)
        {
            var timeBonus = maxTimeForBonus / GameManager.instance.time;
            timeBonus *= 100;
            int timeBonusInt = Mathf.RoundToInt(timeBonus);
            timeBonus = timeBonusInt / 100;

            GameManager.instance.MultiplyScoreByTimeBonus(timeBonus);

            timeText.text = "Time bonus = x" + timeBonus + "!";
        }
        else
        {
            timeText.text = "No time bonus, slowpoke. Better luck next time!";
        }

        finalScoreText.text = "Final score = " + GameManager.instance.playerScore;

        if (GameManager.instance.playerScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", GameManager.instance.playerScore);
            newHighScoreText.text = "New high score!";
        }
        else
        {
            newHighScoreText.text = "";
        }
    }

    private void ScoreHorse()
    {
        var score = GameManager.instance.playerScore;
        if (score < 10000)
        {
            StartCoroutine(SetStarsActive(1));
            horseDescriptionText.text = "Scientific consensus: Just a regular horse, and not a very good one.";
        }
        else if (score < 20000)
        {
            StartCoroutine(SetStarsActive(2));
            horseDescriptionText.text = "Scientific consensus: Horse will live out his life with barely any unexpected limbs. It's fine, we guess.";
        }
        else if (score < 50000)
        {
            StartCoroutine(SetStarsActive(3));
            horseDescriptionText.text = "Scientific consensus: Researchers are writing a heated series of papers discrediting each other's work on whether or not this is a horse. Well done.";
        }
        else if (score < 100000)
        {
            StartCoroutine(SetStarsActive(4));
            horseDescriptionText.text = "Scientific consensus: Barely a horse anymore. Truly exceptional.";
        }
        else
        {
            StartCoroutine(SetStarsActive(5));
            horseDescriptionText.text = "Scientific consensus: Horse has absorbed enough DNA to become omnipotent, and also probably evil. We have awarded him one million Nobel Prizes just in case. Good luck, humanity!";
        }
    }

    private IEnumerator SetStarsActive(int starsCount)
    {
        for (int i = 0; i < starsCount; i++)
        {
            yield return new WaitForSeconds(starDelayTime);
            audioSource.Play();
            stars[i].SetActive(true);
        }
    }
}