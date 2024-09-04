using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVolumeChangeSubject 
{
    void RegisterVolumeChangeObserver(IVolumeChangeObserver _observer);
    void UnregisterVolumeChangeObserver(IVolumeChangeObserver _observer);
    void NotifyVolumeChangeObservers(EVolumeType _type, float _volume);
}
