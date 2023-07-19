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
        UIInteract.Instance.ShowText(_text);

        if (!hasOutline) return;
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
        UIInteract.Instance.HideText();

        if (!hasOutline) return;
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
