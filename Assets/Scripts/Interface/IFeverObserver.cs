using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFeverObserver
{
    void OnFeverNotify(in bool _isFeverStart);
}
