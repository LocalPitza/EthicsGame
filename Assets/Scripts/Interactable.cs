using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;
using QualityLevel = HighlightPlus.QualityLevel;

public abstract class Interactable : MonoBehaviour
{
    [HideInInspector] public HighlightEffect outline;

    public bool hasOutline;
    public virtual void Awake()
    {
        gameObject.layer = 6;
        if (!hasOutline) return;
        outline = gameObject.AddComponent<HighlightEffect>();
        outline.outlineWidth = 1f;
        outline.outlineColor = Color.white;
        outline.outlineVisibility = Visibility.AlwaysOnTop;
        outline.highlighted = false;
        outline.cullBackFaces = false;
        outline.outlineQuality = QualityLevel.High;

    }

    public abstract void OnInteract();
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
    
    
    
}
