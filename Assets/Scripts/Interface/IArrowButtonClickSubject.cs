using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArrowButtonClickSubject 
{
    void RegisterObserver(IArrowButtonClickObserver _observer);
    void UnregisterObserver(IArrowButtonClickObserver _observer);
    void NotifyObservers(in EArrowButtonType _arrowType);
}
