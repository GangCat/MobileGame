using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFeverSubject
{
    void RegisterFeverObserver(IFeverObserver _observer);
    void UnregisterFeverObserver(IFeverObserver _observer);
    void NotifyFeverObservers(in bool _isFeverStart);
}
