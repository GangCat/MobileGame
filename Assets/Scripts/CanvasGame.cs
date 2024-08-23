using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGame : MonoBehaviour
{
    [SerializeField]
    private SpeedLineImage speedLineImage = null;
    [SerializeField]
    private Text scoreText = null;

    public void Init()
    {
        speedLineImage.Init();
        scoreText.text = "0";
        scoreText.enabled = false;
        gameObject.SetActive(false);
    }

    public void ShowGameUI()
    {
        gameObject.SetActive(true);
    }

    public void GameOver()
    {
        speedLineImage.GameOver();
        scoreText.enabled = false;
    }

    public void StartGame()
    {
        scoreText.text = "0";
        scoreText.enabled = true;
    }

    public void UpdateScore(int _score)
    {
        scoreText.text = _score.ToString();
    }

    public void StartSpeedLine()
    {
        speedLineImage.ShowSpeedLine();
    }

    public void FinishSpeedLine()
    {
        speedLineImage.HideSpeedLine();
    }
}
