using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTest : Interactable
{
    [SerializeField] private string _text = "";
    public override void OnInteract()
    {
        
    }

    public override void OnFocus()
    {
        outline.OutlineWidth = 10;
        UIInteract.Instance.ShowText(_text);
    }

    public override void OnLoseFocus()
    {
        outline.OutlineWidth = 0;
        UIInteract.Instance.HideText();
    }
}
