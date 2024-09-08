using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateManager : MonoBehaviour, IPlayerMoveObserver
{
    public void Init()
    {

    }

    private bool isVibrate = true;

    public void OnPlayerMoveNotify(in EBlockType _blockType)
    {
        if(isVibrate)
            Vibration.Vibrate(100);
    }

    public void SetIsVibrate(bool _isVibrate)
    {
        isVibrate = _isVibrate;
    }
}
