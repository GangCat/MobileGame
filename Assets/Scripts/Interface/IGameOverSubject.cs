using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameOverSubject
{
    void RegisterGameOverObserver(IGameOverObserver _observer);
    void UnregisterGameOverObserver(IGameOverObserver _observer);
    void NotifyObserversGameOver();
}
