using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFade : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage = null;

    public void Init()
    {
        fadeImage.enabled = false;
    }

    public void FadeIn()
    {
        fadeImage.enabled = true;
    }

    public void FadeOut()
    {
        fadeImage.enabled = false;
    }

    private IEnumerator FadeCoroutine(bool _isFadeIn)
    {
        while (true)
        {
            yield return null;
        }
    }
}
