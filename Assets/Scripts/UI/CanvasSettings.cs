using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSettings : MonoBehaviour, IVolumeChangeSubject
{
    [SerializeField]
    private ButtonBase closeSettingsBtn = null;
    [SerializeField]
    private VibrationButton onVibeBtn= null;
    [SerializeField]
    private VibrationButton offVibeBtn = null;
    [SerializeField]
    private SliderBase bgmVolumeSlider = null;
    [SerializeField]
    private SliderBase sfxVolumeSlider = null;
    [SerializeField]
    private ButtonBase bgmPlusBtn = null;
    [SerializeField]
    private ButtonBase bgmMinusBtn = null;
    [SerializeField]
    private ButtonBase sfxPlusBtn = null;
    [SerializeField]
    private ButtonBase sfxMinusBtn = null;

    [SerializeField]
    private Color activeTextColor = Color.white;
    [SerializeField]
    private Color deactiveTextColor = Color.white;

    private List<IVolumeChangeObserver> volumeChangeObserverList = null;


    public void Init(Action _closeSettingsAction, Action<bool> _vibeSetAction)
    {
        closeSettingsBtn.Init();
        onVibeBtn.Init();
        offVibeBtn.Init();

        bgmVolumeSlider.Init();
        sfxVolumeSlider.Init();

        bgmPlusBtn.Init();
        bgmMinusBtn.Init();
        sfxPlusBtn.Init();
        sfxMinusBtn.Init();

        closeSettingsBtn.SetOnClickAction(() => { gameObject.SetActive(false); _closeSettingsAction?.Invoke(); } );
        onVibeBtn.SetOnClickAction(() => 
        { 
            _vibeSetAction?.Invoke(true);
            onVibeBtn.ChangeTextColor(activeTextColor);
            offVibeBtn.ChangeTextColor(deactiveTextColor);
        });
        offVibeBtn.SetOnClickAction(() => 
        { 
            _vibeSetAction?.Invoke(false);
            onVibeBtn.ChangeTextColor(deactiveTextColor);
            offVibeBtn.ChangeTextColor(activeTextColor);
        });

        if (volumeChangeObserverList == null)
            volumeChangeObserverList = new List<IVolumeChangeObserver>();

        bgmVolumeSlider.SetOnValueChangeAction((f) => { NotifyVolumeChangeObservers(EVolumeType.BGM, f); });
        sfxVolumeSlider.SetOnValueChangeAction((f) => { NotifyVolumeChangeObservers(EVolumeType.SFX, f); });

        bgmPlusBtn.SetOnClickAction(() => { SetVolume(EVolumeType.BGM, 0.1f); });
        bgmMinusBtn.SetOnClickAction(() => { SetVolume(EVolumeType.BGM, -0.1f); });
        sfxPlusBtn.SetOnClickAction(() => { SetVolume(EVolumeType.SFX, 0.1f); });
        sfxMinusBtn.SetOnClickAction(() => { SetVolume(EVolumeType.SFX, -0.1f); });

        gameObject.SetActive(false);
    }

    public void OpenSettings()
    {
        gameObject.SetActive(true);
    }

    private void SetVolume(EVolumeType _type, float _addVolume)
    {
        switch (_type)
        {
            case EVolumeType.BGM:
                bgmVolumeSlider.AddValue(_addVolume);
                NotifyVolumeChangeObservers(_type, bgmVolumeSlider.GetValue);
                break;
            case EVolumeType.SFX:
                sfxVolumeSlider.AddValue(_addVolume);
                NotifyVolumeChangeObservers(_type, sfxVolumeSlider.GetValue);
                break;
        }
    }


    public void RegisterVolumeChangeObserver(IVolumeChangeObserver _observer)
    {
        if (volumeChangeObserverList == null)
            volumeChangeObserverList = new List<IVolumeChangeObserver>();

        if(!volumeChangeObserverList.Contains(_observer))
            volumeChangeObserverList.Add(_observer);
    }

    public void UnregisterVolumeChangeObserver(IVolumeChangeObserver _observer)
    {
        if (volumeChangeObserverList.Contains(_observer))
            volumeChangeObserverList.Remove(_observer);
    }
    public void NotifyVolumeChangeObservers(EVolumeType _type, float _volume)
    {
        foreach (var observer in volumeChangeObserverList)
            observer.OnVolumeChangeNotify(_type, _volume);
    }
}
