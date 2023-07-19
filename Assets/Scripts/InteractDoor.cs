using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractDoor : Interactable
{
    private bool _isOpen = false;
    private string _text = "Open Door";
    private BoxCollider _collider;
    private AudioSource _audioSource;
    [SerializeField] private Vector3 _start;
    [SerializeField] private Vector3 _end;
    

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _audioSource = GetComponent<AudioSource>();
    }

    public override void OnInteract()
    {
        if (!_isOpen)
        {
            SoundManager.Instance.PlayOnceObject(_audioSource, SoundManager.Sounds.DoorOpen);
            _collider.enabled = false;
            transform.DORotate(_end, 0.75f).onComplete = () => {  _collider.enabled = true;};
                
            _isOpen = true;
            _text = "Close Door";
        }
        else
        {
            SoundManager.Instance.PlayOnceObject(_audioSource, SoundManager.Sounds.DoorClose, 0.35f);
            _collider.enabled = false;
            transform.DORotate(_start, 0.75f).onComplete = () => {  _collider.enabled = true;};
            _isOpen = false;
            _text = "Open Door";
        }
        
    }

    public override void OnFocus()
    {
        
        UIInteract.Instance.ShowText(_text);
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }
}
