using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFadeOutFinishSubject
{
    void RegisterObserver(IFadeOutFinishObserver _observer);
    void UnregisterObserver(IFadeOutFinishObserver _observer);
    void NotifyObservers();
}
