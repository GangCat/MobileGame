using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVolumeChangeObserver 
{
    public void OnVolumeChangeNotify(EVolumeType _volumeType, float _volume);
}
