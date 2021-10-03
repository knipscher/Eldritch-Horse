using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private int easyMaxMutation = 30000;
    [SerializeField]
    private int normalMaxMutation = 15000;
    [SerializeField]
    private int difficultMaxMutation = 5000;

    public void SetDifficulty(int difficulty)
    {
        if (difficulty == 0)
        {
            PlayerPrefs.SetInt("DifficultyAdjustedMaxMutation", easyMaxMutation);
            PlayerPrefs.SetInt("Difficulty", 0);
        }
        else if (difficulty == 1)
        {
            PlayerPrefs.SetInt("DifficultyAdjustedMaxMutation", normalMaxMutation);
            PlayerPrefs.SetInt("Difficulty", 1);
        }
        else if (difficulty == 2)
        {
            PlayerPrefs.SetInt("DifficultyAdjustedMaxMutation", difficultMaxMutation);
            PlayerPrefs.SetInt("Difficulty", 2);
        }
        else
        {
            Debug.LogWarning("Invalid difficulty.");
        }
    }
}