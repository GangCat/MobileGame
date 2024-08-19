using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IPlayerMoveObserver
{
    [SerializeField]
    private AudioSource backgroundAudioSrc = null;
    [SerializeField]
    private AudioSource[] blockSfxAudioSrc = null;
    [SerializeField]
    private AudioSource[] sfxAudioSrc = null;

    [SerializeField]
    private AudioClip menuBackgroundAudioClip = null;
    [SerializeField]
    private AudioClip[] sfxAudioClip = null;
    [SerializeField]
    private AudioClip countdownClip = null;
    [SerializeField]
    private AudioClip[] normalBlockSfxAudioClip = null;

    private float fadeoutTime = 2f;
    private float backgroundVolume = 1f;

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

    public void OnNotify(in EBlockType _blockType)
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
            case EBlockType.INVINCIBLE_BUFF:
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


    internal void FadeOutBackground()
    {
        StartCoroutine(nameof(FadeOutBackgroundCoroutine));
    }

    private IEnumerator FadeOutBackgroundCoroutine()
    {
        float elapsedTime = fadeoutTime;
        while (elapsedTime / fadeoutTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            backgroundAudioSrc.volume = elapsedTime / fadeoutTime;

            yield return null;
        }

        backgroundAudioSrc.Stop();
    }
}
