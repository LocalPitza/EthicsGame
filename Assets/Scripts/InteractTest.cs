using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTest : Interactable
{

    public override void OnInteract()
    {
        
    }

    public override void OnFocus()
    {
        outline.OutlineWidth = 10;
        UIInteract.Instance.ShowText("Magazines");
    }

    public override void OnLoseFocus()
    {
        outline.OutlineWidth = 0;
        UIInteract.Instance.HideText();
    }
}
