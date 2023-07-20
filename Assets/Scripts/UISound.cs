using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    public void UIHover()
    {
        SoundManager.Instance.PlayOnceMain(SoundManager.Sounds.UIHover, 0.5f);
    }
    
    public void UIClick()
    {
        SoundManager.Instance.PlayOnceMain(SoundManager.Sounds.UIClick,0.5f);
    }
}
