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
    private ArrowButton[] arrowBtnArr = null;
    [SerializeField]
    private MultiScoreTextEffect[] multiScoreTextEffectArr = null;

    private List<IArrowButtonClickObserver> observerList = null;

    public void Init()
    {
        speedLineImage.Init();
        scoreText.text = "0";
        scoreText.enabled = false;
        gameObject.SetActive(false);
        observerList = new List<IArrowButtonClickObserver>();

        foreach (var effect in multiScoreTextEffectArr)
            effect.Init();

        foreach(var arrowBtn in  arrowBtnArr)
        {
            arrowBtn.Init();
            arrowBtn.SetOnClickAction(() => { NotifyArrowButtonClickObservers(arrowBtn.ArrowType); });
        }
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
    }

    public void GameOver()
    {
        speedLineImage.GameOver();
        scoreText.enabled = false;
        gameObject.SetActive(false);
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
