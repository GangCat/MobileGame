using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasResult : MonoBehaviour
{
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
        gameObject.SetActive(false);
        closeBtn.Init();
        closeBtn.SetOnClickAction(_closeResultAction);
    }

    public void EnterLobby()
    {
        gameObject.SetActive(false);
    }

    public void GameOver(SResult _sResult)
    {
        gameObject.SetActive(true);
        SetResult(_sResult);
    }

    public void SetResult(SResult _sResult)
    {
        float time = _sResult.time;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        scoreText.text = _sResult.score.ToString();
        totalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        bestAPSText.text = _sResult.bestAPS.ToString("F3");
    }
}
