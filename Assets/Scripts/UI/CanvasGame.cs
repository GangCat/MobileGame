using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasGame : MonoBehaviour, IArrowButtonClickSubject
{
    [SerializeField]
    private SpeedLineImage speedLineImage = null;
    [SerializeField]
    private TextMeshProUGUI scoreText = null;
    [SerializeField]
    private TextMeshProUGUI countdownText = null;
    [SerializeField]
    private Slider hpSlider = null;
    [SerializeField]
    private ArrowButton[] arrowBtnArr = null;
    [SerializeField]
    private MultiScoreTextEffect[] multiScoreTextEffectArr = null;

    private List<IArrowButtonClickObserver> observerList = null;

    public void Init()
    {
        speedLineImage.Init();
        scoreText.text = "0";
        hpSlider.value = 100;
        scoreText.enabled = false;
        observerList = new List<IArrowButtonClickObserver>();

        foreach (var effect in multiScoreTextEffectArr)
            effect.Init();

        foreach(var arrowBtn in  arrowBtnArr)
        {
            arrowBtn.Init();
            arrowBtn.SetOnClickAction(() => { NotifyArrowButtonClickObservers(arrowBtn.ArrowType); });
        }

        countdownText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void StartFever()
    {

    }

    public void StopFever()
    {

    }

    public void ShowGameUI()
    {
        gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        speedLineImage.GameOver();
        scoreText.enabled = false;
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        countdownText.gameObject.SetActive(false);
        scoreText.text = "0";
        scoreText.enabled = true;
    }

    public void UpdateScore(int _score)
    {
        scoreText.text = _score.ToString();
    }

    public void StartMultiplyScore(int _multiFactor)
    {
        speedLineImage.ShowSpeedLine();
        foreach (var effect in multiScoreTextEffectArr)
            effect.StartEffect(_multiFactor);
    }

    public void FinishMultiplyScore()
    {
        speedLineImage.HideSpeedLine();
        foreach (var effect in multiScoreTextEffectArr)
            effect.FinishEffect();
    }

    internal void CountDown(int _count)
    {
        countdownText.text = _count.ToString();
    }

    internal void UpdatePlayerHP(float _curHP)
    {
        hpSlider.value = _curHP;
    }

    public void RegisterArrowButtonClickObserver(IArrowButtonClickObserver _observer)
    {
        if (!observerList.Contains(_observer))
        {
            observerList.Add(_observer);
        }
    }

    public void UnregisterArrowButtonClickObserver(IArrowButtonClickObserver _observer)
    {
        if (observerList.Contains(_observer))
        {
            observerList.Remove(_observer);
        }
    }

    public void NotifyArrowButtonClickObservers(in EArrowButtonType _arrowType)
    {
        foreach (var observer in observerList)
        {
            observer.OnArrowButtonClickNotify(_arrowType);
        }
    }


}
