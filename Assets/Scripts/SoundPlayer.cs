using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    //idk i mainly used this for bg audios
    
    private AudioSource _audioSource;
    [SerializeField] private bool _instantPlay = false;
    [SerializeField] private bool _isLoop = false;
    [SerializeField]private SoundManager.Sounds _sounds;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (!_instantPlay) return;
        PlaySound();
    }

    private void PlaySound()
    {
        if (!_isLoop)
        {
            SoundManager.Instance.PlayOnceObject(_audioSource, _sounds);
        }
        else
        {
            SoundManager.Instance.PlayLoopObject(_audioSource, _sounds);
        }
        
    }
}
