using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [HideInInspector]
    public Outline outline;
    public virtual void Awake()
    {
        gameObject.layer = 6;
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineWidth = 0f;
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.green;
    }

    public abstract void OnInteract();
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
    
    
    
}
