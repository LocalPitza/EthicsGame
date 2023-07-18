using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Sprite[] _detectiveImages;
    [SerializeField] private Sprite[] _wifeImages;
    
    [Header("Characters")]
    [SerializeField] private SpriteRenderer _spDetective;
    [SerializeField] private SpriteRenderer _spWife;

    private void OnEnable()
    {
        Lua.RegisterFunction("ChangeDetectiveSprite", this, SymbolExtensions.GetMethodInfo(() => ChangeDetective()));
        Lua.RegisterFunction("ChangeWifeSprite", this, SymbolExtensions.GetMethodInfo(() => ChangeWife()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("ChangeDetectiveSprite");
        Lua.UnregisterFunction("ChangeWifeSprite");
    }

    public void ChangeDetective()
    {
        int index = DialogueLua.GetVariable("DetectiveSpriteIndex").asInt;
        
        if (index > _detectiveImages.Length)
        {
            index = _detectiveImages.Length;
        }

        _spDetective.sprite = _detectiveImages[index];
        
    }
    
    public void ChangeWife()
    {
        int index = DialogueLua.GetVariable("WifeSpriteIndex").asInt;
        
        if (index > _wifeImages.Length)
        {
            index = _wifeImages.Length;
        }

        _spWife.sprite = _wifeImages[index];
    }
}
