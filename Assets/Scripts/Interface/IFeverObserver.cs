using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFeverObserver
{
    void OnNotify(in bool _isFeverStart);
}
