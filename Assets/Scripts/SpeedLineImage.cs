using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedLineImage : MonoBehaviour
{
    [SerializeField]
    private Sprite[] speedLineSpriteArr = null;

    private Image image = null;
    private int curSpeiteNum = 0;

    public void Init()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void GameOver()
    {
        image.enabled = false;
        StopCoroutine(nameof(ShowSpeedLineCoroutine));
    }


    public void ShowSpeedLine()
    {
        image.enabled = true;
        StartCoroutine(nameof(ShowSpeedLineCoroutine));
    }

    public void HideSpeedLine()
    {
        image.enabled = false;
        StopCoroutine(nameof(ShowSpeedLineCoroutine));
    }

    private IEnumerator ShowSpeedLineCoroutine()
    {
        while (true)
        {
            image.sprite = speedLineSpriteArr[curSpeiteNum++];
            if (curSpeiteNum >= speedLineSpriteArr.Length)
                curSpeiteNum = 0;
            yield return null;
        }
    }
}
