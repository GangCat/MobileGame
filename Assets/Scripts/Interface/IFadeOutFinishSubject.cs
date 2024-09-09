using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFadeOutFinishSubject
{
    void RegisterFadeOutFinishObserver(IFadeOutFinishObserver _observer);
    void UnregisterFadeOutFinishObserver(IFadeOutFinishObserver _observer);
    void NotifyFadeOutFinishObservers();
}
