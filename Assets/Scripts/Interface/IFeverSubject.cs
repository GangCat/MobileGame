using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFeverSubject
{
    void RegisterObserver(IFeverObserver _observer);
    void UnregisterObserver(IFeverObserver _observer);
    void NotifyObservers(in bool _isFeverStart);
}
