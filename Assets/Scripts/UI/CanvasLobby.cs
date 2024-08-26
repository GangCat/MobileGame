using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLobby : MonoBehaviour, IFadeOutFinishObserver
{
    [SerializeField]
    private ButtonBase startBtn = null;

    public void Init(Action _startBtnAction)
    {
        gameObject.SetActive(true);
        startBtn.Init();

        startBtn.SetOnClickAction(_startBtnAction);
    }

    public void EnterLobby()
    {
        gameObject.SetActive(true);
    }

    public void ShowGameUI()
    {
        gameObject.SetActive(false);
    }

    public void OnFadeOutFinishNotify()
    {
        EnterLobby();
    }
}
