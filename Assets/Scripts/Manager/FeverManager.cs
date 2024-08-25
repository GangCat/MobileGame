using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverManager : MonoBehaviour, IPlayerMoveObserver, IFeverSubject
{
    [SerializeField]
    private float feverTime = 10f;

    private List<IFeverObserver> observerList = null;
    private WaitForSeconds waitFeverTime = null;

    public void Init()
    {
        observerList = new();
        waitFeverTime = new WaitForSeconds(feverTime);
    }

    public void StartFever()
    {
        StartCoroutine(nameof(FeverCoroutine));
    }

    private IEnumerator FeverCoroutine()
    {
        NotifyObservers(true);
        yield return waitFeverTime;
        NotifyObservers(false);
    }

    public void NotifyObservers(in bool _isFeverStart)
    {
        foreach(var observer in observerList)
            observer.OnNotify(_isFeverStart);
    }

    public void RegisterObserver(IFeverObserver _observer)
    {
        observerList ??= new();
        if(!observerList.Contains(_observer))
            observerList.Add(_observer);
    }

    public void UnregisterObserver(IFeverObserver _observer)
    {
        if (observerList.Contains(_observer))
            observerList.Remove(_observer);
    }

    public void OnNotify(in EBlockType _blockType)
    {
        if (_blockType.Equals(EBlockType.FEVER_BUFF))
            StartFever();
    }
}
