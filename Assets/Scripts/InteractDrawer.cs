using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InteractDrawer : Interactable
{
    private bool _isOpen = false;
    private string _text = "Open Drawer";
    private Renderer _objRenderer;
    private Color originalColor;
    [SerializeField] private Vector3 _openedPosition;

    private void Start()
    {
        _objRenderer = GetComponent<Renderer>();
        originalColor = _objRenderer.material.color;
    }

    public override void OnInteract()
    {
        if (!_isOpen)
        {
            transform.DOLocalMove(_openedPosition, 0.75f);
            
                
            _isOpen = true;
            _text = "Close Drawer";
        }
        else
        {
            transform.DOLocalMove(new Vector3(0, 0, 0), 0.75f);
            _isOpen = false;
            _text = "Open Drawer";
        }
        
    }

    public override void OnFocus()
    {

        _objRenderer.material.DOColor((originalColor+ new Color(0.2f,0.2f,0.2f)), 0.2f);
        
        UIInteract.Instance.ShowText(_text);
        
        if (outline == null)
        {
            Debug.Log($"{gameObject.name} needs outline component");
        }
        else
        {
            outline.highlighted = true;
        }
    }

    public override void OnLoseFocus()
    {
        
        _objRenderer.material.DOColor(originalColor, 0.2f);
        
        UIInteract.Instance.HideText();
        
        if (outline == null)
        {
            Debug.Log($"{gameObject.name} needs outline component");
        }
        else
        {
            outline.highlighted = false;
        }
    }
}
