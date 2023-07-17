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
    [SerializeField] private Vector3 _start;
    [SerializeField] private Vector3 _end;
    

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    public override void OnInteract()
    {
        if (!_isOpen)
        {
            _collider.enabled = false;
            transform.DORotate(_end, 0.75f).onComplete = () => {  _collider.enabled = true;};
                
            _isOpen = true;
            _text = "Close Door";
        }
        else
        {
            _collider.enabled = false;
            transform.DORotate(_start, 0.75f).onComplete = () => {  _collider.enabled = true;};
            _isOpen = false;
            _text = "Open Door";
        }
        
    }

    public override void OnFocus()
    {
        if (hasOutline)
        {
            outline.OutlineWidth = 10;
        }
        
        UIInteract.Instance.ShowText(_text);
    }

    public override void OnLoseFocus()
    {
        if (hasOutline)
        {
            outline.OutlineWidth = 0;
        }
        UIInteract.Instance.HideText();
    }
}
