using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverManager : MonoBehaviour, IPlayerMoveObserver, IFeverSubject
{
    [SerializeField]
    private float feverTime = 10f;
    [SerializeField]
    private Slider feverSlider = null;

    private List<IFeverObserver> observerList = null;

    public void Init()
    {
        observerList = new();
        feverSlider.gameObject.SetActive(false);
    }

    public void StartFever()
    {
        StartCoroutine(nameof(FeverCoroutine));
    }

    private IEnumerator FeverCoroutine()
    {
        NotifyFeverObservers(true);
        feverSlider.gameObject.SetActive(true);
        feverSlider.value = 1f;
        float elapsedTime = feverTime;
        while(elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            feverSlider.value = elapsedTime / feverTime;
            yield return null;
        }
        feverSlider.gameObject.SetActive(false);
        NotifyFeverObservers(false);
    }

    public void NotifyFeverObservers(in bool _isFeverStart)
    {
        foreach(var observer in observerList)
            observer.OnFeverNotify(_isFeverStart);
    }

    public void RegisterFeverObserver(IFeverObserver _observer)
    {
        observerList ??= new();
        if(!observerList.Contains(_observer))
            observerList.Add(_observer);
    }

    public void UnregisterFeverObserver(IFeverObserver _observer)
    {
        if (observerList.Contains(_observer))
            observerList.Remove(_observer);
    }

    public void OnPlayerMoveNotify(in EBlockType _blockType)
    {
        if (_blockType.Equals(EBlockType.FEVER_BUFF))
            StartFever();
    }
}
