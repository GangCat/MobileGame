using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFade : MonoBehaviour, IFadeOutFinishSubject
{
    [SerializeField]
    private Image fadeImage = null;
    [SerializeField]
    private float fadeTime = 1f;

    private List<IFadeOutFinishObserver> observerList = null;

    public void Init()
    {
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.enabled = false;
        observerList ??= new List<IFadeOutFinishObserver>();
    }

    /// <summary>
    /// 화면 밝아짐
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(nameof(FadeCoroutine), false);
    }

    /// <summary>
    /// 화면 어두워짐
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(nameof(FadeCoroutine), true);
    }


    private IEnumerator FadeCoroutine(bool _isFadeOut)
    {
        if(_isFadeOut)
            fadeImage.enabled = true;

        float elapsedTime = 0f;
        Color newColor = fadeImage.color;
        float originAlpha = newColor.a;
        float newAlpha = Mathf.Abs(originAlpha - 1f);
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            newColor.a = Mathf.Lerp(originAlpha, newAlpha, elapsedTime / fadeTime);
            fadeImage.color = newColor;

            yield return null;
        }
        newColor.a = newAlpha;
        fadeImage.color = newColor;

        

        if (!_isFadeOut)
            fadeImage.enabled = false;
        else
        {
            NotifyObservers();
            FadeIn();
        }
    }

    public void RegisterObserver(IFadeOutFinishObserver _observer)
    {
        observerList ??= new List<IFadeOutFinishObserver>();
        if(!observerList.Contains(_observer))
            observerList.Add(_observer);
    }

    public void UnregisterObserver(IFadeOutFinishObserver _observer)
    {
        if(observerList.Contains(_observer))
            observerList.Remove(_observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observerList)
            observer.OnFadeOutFinishNotify();
    }
}
