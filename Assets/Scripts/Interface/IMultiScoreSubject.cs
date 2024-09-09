using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMultiScoreSubject
{
    void RegisterMultiScoreObserver(IMultiScoreObserver _observer);
    void UnregisterMultiScoreObserver(IMultiScoreObserver _observer);
    void NotifyMultiScoreObservers(bool _isStart, int _multiplyFactor);
}
