using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IFeverObserver, IMultiScoreObserver
{
    [SerializeField]
    private CanvasResult canvasResult;
    [SerializeField]
    private CanvasLobby canvasLobby;
    [SerializeField]
    private CanvasGame canvasGame = null;
    [SerializeField]
    private CanvasFade canvasFade = null;
    [SerializeField]
    private CanvasSettings canvasSettings = null;
    [SerializeField]
    private CanvasContinue canvasContinue = null;

    public void Init(Action _startGameAction, Action<bool> _onVibeSetAction)
    {
        canvasLobby.Init(_startGameAction, canvasSettings.OpenSettings);
        canvasResult.Init(canvasFade.FadeOut);
        canvasGame.Init();
        canvasFade.Init();
        canvasSettings.Init(canvasLobby.ShowLobby, _onVibeSetAction);
        canvasContinue.Init();

        RegisterFadeFinishObserver(canvasLobby);
        RegisterFadeFinishObserver(canvasResult);
    }

    public void RegisterVolumeChangeObserver(IVolumeChangeObserver _observer)
    {
        canvasSettings.RegisterVolumeChangeObserver(_observer);
    }

    public void Countdown(int _count)
    {
        canvasGame.CountDown(_count);
    }

    /// <summary>
    /// 카운트다운 전
    /// </summary>
    public void ShowGameUI()
    {
        canvasLobby.HideLobby();
        canvasGame.ShowGameUI();
    }

    /// <summary>
    /// 카운트다운 후
    /// </summary>
    public void StartGame()
    {
        canvasGame.StartGame();
    }

    public void RegisterArrowClickObserver(IArrowButtonClickObserver _observer)
    {
        canvasGame.RegisterArrowButtonClickObserver(_observer);
    }

    public void RegisterFadeFinishObserver(IFadeOutFinishObserver _observer)
    {
        canvasFade.RegisterFadeOutFinishObserver(_observer);
    }

    public void UpdatePlayerHP(float _curHP)
    {
        canvasGame.UpdatePlayerHP(_curHP);
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

    public void OnMultiScoreNotify(bool _isStart, int _multiplyFactor)
    {
        if (_isStart)
            canvasGame.StartMultiplyScore(_multiplyFactor);
        else
            canvasGame.FinishMultiplyScore();
    }
}
