using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMultiScoreObserver
{
    void OnMultiScoreNotify(bool _isStart, int _multiplyFactor);

}
