using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class InteractTest : Interactable
{
    [SerializeField] private string _text = "";
    
    public override void OnInteract()
    {
        DialogueManager.StartConversation(GetComponent<DialogueSystemTrigger>().conversation);
        UIInteract.Instance.HideText();
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
