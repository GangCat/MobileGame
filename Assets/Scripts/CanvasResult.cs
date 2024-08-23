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

    public void Init()
    {
        gameObject.SetActive(false);
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
