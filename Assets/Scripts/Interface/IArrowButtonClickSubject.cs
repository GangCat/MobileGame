using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArrowButtonClickSubject 
{
    void RegisterArrowButtonClickObserver(IArrowButtonClickObserver _observer);
    void UnregisterArrowButtonClickObserver(IArrowButtonClickObserver _observer);
    void NotifyArrowButtonClickObservers(in EArrowButtonType _arrowType);
}
