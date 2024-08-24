using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArrowButtonClickObserver
{
    void OnNotify(in EArrowButtonType _arrowType);
}
