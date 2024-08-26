using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IFeverObserver
{
    [SerializeField]
    private Slider hpSlider = null;
    [SerializeField]
    private Text scoreText = null;
    [SerializeField]
    private Text countdownText = null;

    [SerializeField]
    private CanvasResult canvasResult;
    [SerializeField]
    private CanvasLobby canvasLobby;
    [SerializeField]
    private CanvasGame canvasGame = null;
    [SerializeField]
    private CanvasFade canvasFade = null;

    public void Init(Action _StartGameAction)
    {
        hpSlider.value = 100;
        scoreText.text = "0";
        canvasLobby.Init(_StartGameAction);
        canvasResult.Init(canvasFade.FadeOut);
        canvasGame.Init();
        canvasFade.Init();

        RegisterFadeFinishObserver(canvasLobby);
        RegisterFadeFinishObserver(canvasResult);
    }

    public void Countdown(int _count)
    {
        countdownText.text = _count.ToString();
    }

    /// <summary>
    /// 카운트다운 전
    /// </summary>
    public void ShowGameUI()
    {
        canvasLobby.ShowGameUI();
        canvasGame.ShowGameUI();
        countdownText.gameObject.SetActive(true);
    }

    /// <summary>
    /// 카운트다운 후
    /// </summary>
    public void StartGame()
    {
        countdownText.gameObject.SetActive(false);
        canvasGame.StartGame();
    }

    public void RegisterArrowClickObserver(IArrowButtonClickObserver _observer)
    {
        canvasGame.RegisterObserver(_observer);
    }

    public void RegisterFadeFinishObserver(IFadeOutFinishObserver _observer)
    {
        canvasFade.RegisterObserver(_observer);
    }


    public void StartSpeedLine()
    {
        canvasGame.StartSpeedLine();
    }

    public void FinishSpeedLine()
    {
        canvasGame.FinishSpeedLine();
    }

    public void UpdatePlayerHP(float _curHP)
    {
        hpSlider.value = _curHP;
    }

    public void UpdateScore(int _score)
    {
        canvasGame.UpdateScore(_score);
    }

    public void GameOver(SResult _sResult)
    {
        canvasResult.GameOver(_sResult);
        canvasGame.GameOver();
    }

    public void OnFeverNotify(in bool _isFeverStart)
    {
        if (_isFeverStart is true)
            canvasGame.StartFever();
        else
            canvasGame.StopFever();
    }
}
