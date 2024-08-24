using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
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

    public void Init(Action _StartGameAction, Action _closeResultAction)
    {
        hpSlider.value = 100;
        scoreText.text = "0";
        canvasLobby.Init(_StartGameAction);
        canvasResult.Init(_closeResultAction);
        canvasGame.Init();
        canvasFade.Init();
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

    public void EnterLobby()
    {
        //canvasFade.FadeOut();
        // 화면이 어두워지고

        // UI나오고
        // 게임 로직초기화되고
        canvasResult.EnterLobby();
        canvasLobby.EnterLobby();

        // 화면이 밝아짐

    }

}
