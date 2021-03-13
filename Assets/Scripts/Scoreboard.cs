using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays one score at a time: current score or high score.
/// </summary>
public class Scoreboard : MonoBehaviour
{
    public Text scoreCount;
    int score = 0;
    int hiScore = 0;

    public void IncrementScore()
    {
        ChangeScore(1);
    }

    public int ChangeScore(int delta)
    {
        score += delta;
        UpdateScore(score);
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScore(score);
    }

    public int UpdateHighScore(int other)
    {
        hiScore = Mathf.Max(other, hiScore);
        UpdateScore(hiScore);
        return score;
    }

    public int GetScore()
    {
        return score;
    }

    void UpdateScore(int displayText)
    {
        scoreCount.text = $"{displayText}";
    }
}
