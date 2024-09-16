using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IPlayerMoveObserver, IFadeOutFinishObserver, IGameOverObserver, IFeverObserver, IVolumeChangeObserver
{
    [Header("-Background")]
    [SerializeField]
    private AudioSource backgroundAudioSrc = null;
    [SerializeField]
    private AudioClip menuBackgroundAudioClip = null;
    [SerializeField]
    private AudioClip gameBackgroundAudioClip = null;

    [Header("-blockSFX")]
    [SerializeField]
    private AudioSource[] blockSfxAudioSrc = null;
    [SerializeField]
    private AudioClip[] normalBlockSfxAudioClip = null;

    [Header("-SFX")]
    [SerializeField]
    private AudioSource[] sfxAudioSrc = null;
    [SerializeField]
    private AudioClip[] sfxAudioClip = null;

    [Header("-ETC")]
    [SerializeField]
    private AudioClip countdownClip = null;
    [SerializeField]
    private AudioClip fallingAudioClip = null;

    private float fadeoutTime = 2f;
    private float backgroundVolume = 1f;

    private bool isPause = false;

    public void Init()
    {
        backgroundAudioSrc = gameObject.AddComponent<AudioSource>();
        backgroundAudioSrc.playOnAwake = false;
        backgroundAudioSrc.loop = true;

        blockSfxAudioSrc = new AudioSource[(int)EBlockSFXType.LENGTH];
        sfxAudioSrc = new AudioSource[(int)ESFXType.LENGTH];
        for (int i = 0; i < blockSfxAudioSrc.Length; i++) 
        { 
            blockSfxAudioSrc[i] = gameObject.AddComponent<AudioSource>();
            blockSfxAudioSrc[i].playOnAwake = false;
        }
        for (int i = 0; i < sfxAudioSrc.Length; i++)
        {
            sfxAudioSrc[i] = gameObject.AddComponent<AudioSource>();
            sfxAudioSrc[i].playOnAwake = false;
        }
    }

    public void PauseBGM()
    {
        backgroundAudioSrc.Pause();
        foreach (var src in sfxAudioSrc)
            src.Pause();
        foreach(var src in blockSfxAudioSrc)
            src.Pause();

        isPause = true;
    }

    public void PlayBGM()
    {
        backgroundAudioSrc.Play();
        foreach (var src in sfxAudioSrc)
            src.Play();
        foreach (var src in blockSfxAudioSrc)
            src.Play();

        isPause = false;
    }

    public void PlayMenuBackgroundMusic()
    {
        backgroundAudioSrc.clip = menuBackgroundAudioClip;
        backgroundAudioSrc.Play();
        backgroundAudioSrc.volume = backgroundVolume;
    }

    public void PlayCountdownSFX(float _pitch)
    {
        var audioSource = sfxAudioSrc[(int)ESFXType.COUNTDOWN];
        audioSource.clip = countdownClip;
        audioSource.pitch = _pitch;
        audioSource.Play();
    }

    public void OnPlayerMoveNotify(in EBlockType _blockType)
    {
        OnPlayerMoveBlock(_blockType);
    }

    public void OnPlayerMoveBlock(in EBlockType _blockType)
    {
        switch (_blockType)
        {
            case EBlockType.NORMAL:
                PlayBlockSFX(EBlockSFXType.ON_NORMAL_BLOCK);
                break;
            case EBlockType.DOUBLE_SCORE:
                PlayBlockSFX(EBlockSFXType.ON_GOLD_BLOCK);
                break;
            case EBlockType.TRIPLE_SCORE:
                PlayBlockSFX(EBlockSFXType.ON_DIA_BLOCK);
                break;
            case EBlockType.FEVER_BUFF:
                PlayBlockSFX(EBlockSFXType.ON_INVINCIBLE_BUFF_BLOCK);
                break;
        }

    }

    private void PlayBlockSFX(EBlockSFXType _sfxType)
    {
        var audioSource = blockSfxAudioSrc[(int)_sfxType];
        if(_sfxType.Equals(EBlockSFXType.ON_NORMAL_BLOCK))
        {
            audioSource.clip = normalBlockSfxAudioClip[UnityEngine.Random.Range(0, normalBlockSfxAudioClip.Length)];
        }
        else
            audioSource.clip = sfxAudioClip[(int)_sfxType];
        audioSource.Play();
    }


    internal void ChangeToGameBackgroundMusic()
    {
        StartCoroutine(nameof(ChangeToGameBackgroundMusicCoroutine));
    }

    private IEnumerator ChangeToGameBackgroundMusicCoroutine()
    {
        float elapsedTime = fadeoutTime;
        while (elapsedTime / fadeoutTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            backgroundAudioSrc.volume = elapsedTime / fadeoutTime;

            yield return null;
        }


        backgroundAudioSrc.clip = gameBackgroundAudioClip;
        backgroundAudioSrc.volume = 1;
        backgroundAudioSrc.Play();
        //backgroundAudioSrc.Stop();
    }

    public void OnFadeOutFinishNotify()
    {
        PlayMenuBackgroundMusic();
    }

    public void OnGameOverNotify()
    {
        sfxAudioSrc[0].clip = fallingAudioClip;
        backgroundAudioSrc.pitch = 1f;

        if (isPause)
            return;
        sfxAudioSrc[0].Play();
    }

    public void OnFeverNotify(in bool _isFeverStart)
    {
        backgroundAudioSrc.pitch = _isFeverStart ? 1.4f : 1f;
    }

    public void ChangeBackgroundVolume(float _volume)
    {
        backgroundAudioSrc.volume = _volume;
    }

    public void ChangeSFXVolume(float _volume)
    {
        foreach (var src in sfxAudioSrc)
            src.volume = _volume;
    }

    public void OnVolumeChangeNotify(EVolumeType _volumeType, float _volume)
    {
        switch (_volumeType)
        {
            case EVolumeType.BGM:
                ChangeBackgroundVolume(_volume);
                break;
            case EVolumeType.SFX:
                ChangeSFXVolume(_volume);
                break;
        }
    }
}
