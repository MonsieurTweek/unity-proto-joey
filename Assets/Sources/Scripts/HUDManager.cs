using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public Text score;
    public Text highScore;
    public Text scoreMultiplier;
    public Text currency;

    public void UpdateScore(int newScore)
    {
        score.text = "0" + newScore.ToString("000");
    }

    public void UpdateCurrency(int newCurrency)
    {
        currency.text = "0" + newCurrency.ToString("000");
    }

    public void UpdateScoreMultiplier(int newScoreMultiplier)
    {
        scoreMultiplier.text = "x" + newScoreMultiplier.ToString();
    }

    public void SetHighScore(int score)
    {
        highScore.text = "Highscore : " + score.ToString();
    }
}
