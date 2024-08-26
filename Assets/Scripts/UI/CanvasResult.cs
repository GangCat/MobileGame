using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasResult : MonoBehaviour, IFadeOutFinishObserver
{
    [SerializeField]
    private GameObject resultPanelGO = null;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text totalTimeText;
    [SerializeField]
    private Text bestAPSText;
    [SerializeField]
    private ButtonBase closeBtn = null;

    public void Init(Action _closeResultAction)
    {
        resultPanelGO.SetActive(false);
        closeBtn.Init();
        closeBtn.SetOnClickAction(_closeResultAction);
    }

    public void EnterLobby()
    {
        resultPanelGO.SetActive(false);
    }

    public void GameOver(SResult _sResult)
    {
        SetResult(_sResult);
        StartCoroutine(nameof(ShowResultCoroutine));
        
    }

    private IEnumerator ShowResultCoroutine()
    {
        // 일정시간이후 천천히 보여지기

        yield return new WaitForSeconds(2f);
        resultPanelGO.SetActive(true);
    }

    private void SetResult(SResult _sResult)
    {
        float time = _sResult.time;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        scoreText.text = _sResult.score.ToString();
        totalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        bestAPSText.text = _sResult.bestAPS.ToString("F3");
    }

    public void OnFadeOutFinishNotify()
    {
        EnterLobby();
    }
}
