using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Instance

    private static SoundManager _instance;
    public static  SoundManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion
    
    //audio source that dont loop
    public AudioSource SoundSfx;
    //audio source that loops
    public AudioSource SoundBGMusic;
    
    public SoundType[] sounds;
    private Coroutine routine;
    
    public void PlayLoopMain(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);

        if (clip != null)
        {
            SoundBGMusic.clip = clip;
            SoundBGMusic.Play();
        }
        else
        {
            Debug.Log("No clip found for Sound Type");
        }
    }
    
    public void PlayLoopObject(AudioSource source, Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null || source!= null)
        {
            source.clip = clip;
            source.Play();
        }
        else
        {
            Debug.Log("Audio error.");
        }
    }

    public void PlayOnceMain(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            SoundSfx.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("No clip found for sound Type");
        }
    }
    
    public void PlayOnceObject(AudioSource source, Sounds sound, float offset = 0)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null && source != null)
        {
            StartCoroutine(PlayDelayed(source, clip, offset));
        }
        else
        {
            Debug.Log("Audio error.");
        }
    }

    public void StopPlayingBGMMain(Sounds sound)
    {
        if (routine != null)
        {
            StopCoroutine(routine); // to avoid stuttering 
        }
        
        AudioClip clip = getSoundClip(sound);

        if (clip != null)
        {
            SoundBGMusic.clip = clip;
            SoundBGMusic.Stop();
        }
        else
        {
            Debug.Log("No clip found for Sound Type");
        }
    }
    
    public void PausePlayingBGMMain(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);

        if (clip != null)
        {
            SoundBGMusic.clip = clip;
            SoundBGMusic.Pause();
        }
        else
        {
            Debug.Log("No clip found for Sound Type");
        }
    }
    
    public void ResumePlayingBGMMain(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);

        if (clip != null)
        {
            SoundBGMusic.clip = clip;
            SoundBGMusic.UnPause();
        }
        else
        {
            Debug.Log("No clip found for Sound Type");
        }
    }

    

    public void PlayFadeInMain(Sounds sound, float speed, float maxVolume)
    {
        routine = StartCoroutine(FadeIn(sound, speed, maxVolume));
    }
    
    
    IEnumerator FadeIn(Sounds sound, float speed, float maxVolume)
    {
        AudioClip clip = getSoundClip(sound);

        if (clip != null)
        {
            SoundBGMusic.clip = clip;
            SoundBGMusic.volume = 0;
            SoundBGMusic.Play();
            float audioVolume = SoundBGMusic.volume;

            while (SoundBGMusic.volume < maxVolume)
            {
                audioVolume += speed;
                SoundBGMusic.volume = audioVolume;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            Debug.Log("No clip found for Sound Type");
        }
    }
    
    private AudioClip getSoundClip(Sounds sound)
    {
        SoundType _soundType = Array.Find(sounds, s => s.soundType == sound);
        if (_soundType != null)
        {
            return _soundType.soundClip;
        }

        return null;
    }
    
    private IEnumerator PlayDelayed(AudioSource source, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.PlayOneShot(clip);
    }
    
    [Serializable] public class SoundType
    {
        public Sounds soundType;
        public AudioClip soundClip;
    }
    public enum Sounds  //add sound type here, add at the bottom of the list
    {
        DoorOpen,
        DoorClose,
        CarsOutside,
        InterrogationAmbience,
        CeilingFan,
        MirrorBreak,
        HitHead,
        PushBody,
        FallBody,
        WomanGasp,
        Jazz
    }
}