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
    private FeverImage feverImage = null;
    [SerializeField]
    private TextMeshProUGUI scoreText = null;
    [SerializeField]
    private ArrowButton[] arrowBtnArr = null;

    private List<IArrowButtonClickObserver> observerList = null;

    public void Init()
    {
        speedLineImage.Init();
        scoreText.text = "0";
        scoreText.enabled = false;
        gameObject.SetActive(false);
        observerList = new List<IArrowButtonClickObserver>();

        foreach(var arrowBtn in  arrowBtnArr)
        {
            arrowBtn.Init();
            arrowBtn.SetOnClickAction(() => { NotifyObservers(arrowBtn.ArrowType); });
        }
    }

    public void StartFever()
    {
        feverImage.StartFever();
    }

    public void StopFever()
    {
        feverImage.StopFever();
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

    public void StartSpeedLine()
    {
        speedLineImage.ShowSpeedLine();
    }

    public void FinishSpeedLine()
    {
        speedLineImage.HideSpeedLine();
    }

    public void RegisterObserver(IArrowButtonClickObserver _observer)
    {
        if (!observerList.Contains(_observer))
        {
            observerList.Add(_observer);
        }
    }

    public void UnregisterObserver(IArrowButtonClickObserver _observer)
    {
        if (observerList.Contains(_observer))
        {
            observerList.Remove(_observer);
        }
    }

    public void NotifyObservers(in EArrowButtonType _arrowType)
    {
        foreach (var observer in observerList)
        {
            observer.OnArrowButtonClickNotify(_arrowType);
        }
    }
}
