using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLobby : MonoBehaviour, IFadeOutFinishObserver
{
    [SerializeField]
    private ButtonBase startBtn = null;
    [SerializeField]
    private ButtonBase settingsBtn = null;

    public void Init(Action _startBtnAction, Action _settingsBtnAction)
    {
        gameObject.SetActive(true);
        startBtn.Init();
        settingsBtn.Init();

        startBtn.SetOnClickAction(_startBtnAction);
        settingsBtn.SetOnClickAction(_settingsBtnAction);
        settingsBtn.SetOnClickAction(HideLobby);
    }

    public void ShowLobby()
    {
        gameObject.SetActive(true);
    }

    public void HideLobby()
    {
        gameObject.SetActive(false);
    }

    public void OnFadeOutFinishNotify()
    {
        ShowLobby();
    }
}
