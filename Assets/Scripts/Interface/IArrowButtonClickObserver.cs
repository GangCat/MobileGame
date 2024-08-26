using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArrowButtonClickObserver
{
    void OnArrowButtonClickNotify(in EArrowButtonType _arrowType);
}
