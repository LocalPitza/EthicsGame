using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;

public class Events : MonoBehaviour
{
    [Header("Changing 'Scenes'")]
    [SerializeField] private Image _blackScreen;
    [SerializeField] private Transform _apartmentSpawnPoint;
    [SerializeField] private Transform _interrogationRoomSpawnPoint;

    private void OnEnable()
    {
        Lua.RegisterFunction("TeleportToApartment", this, SymbolExtensions.GetMethodInfo(() => TeleportToApartment()));
        Lua.RegisterFunction("TakeCurrentObject", this, SymbolExtensions.GetMethodInfo(() => TakeCurrentObject()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("TeleportToApartment");
        Lua.UnregisterFunction("TakeCurrentObject");
    }

    public void TeleportToApartment()
    {
        _blackScreen.DOFade(1f, 2f).onComplete = () =>
        {
            transform.DOMove(_apartmentSpawnPoint.position, 0f);
            transform.rotation = _apartmentSpawnPoint.rotation;
            
            _blackScreen.DOFade(0f, 3f);
        };

    }

    public void TeleportToInterrogationRoom()
    {
        FirstPersonController.instance.CanMove = false;
        
        _blackScreen.DOFade(1f, 2f).onComplete = () =>
        {
            transform.position = _interrogationRoomSpawnPoint.position;
            transform.rotation = _interrogationRoomSpawnPoint.rotation;
            
            _blackScreen.DOFade(1f, 2f).onComplete = () =>
            {
                _blackScreen.DOFade(0f, 3f);
                FirstPersonController.instance.CanMove = true;
            };

        };
    }

    public void TakeCurrentObject()
    {
        FirstPersonController.instance.currentInteractable.gameObject.SetActive(false);
    }
    
    //TODO: - Fix sometimes not teleporting to apartment
    //TODO: - Fix sometimes outline of object not disappearing (done)
    //TODO: - Fix door colliding when toggled (done)
    //TODO: - Add bool values to objects (finished on existing objects)
    //TODO: - Wife Dialogue (not finished)
    //TODO: - Place interactable objects (story wise)
    //TODO: - Update 2D sprites of characters (done, just fix animation)
    //TODO: - Sound manager
    //TODO: - Update UI aesthetics
    //TODO: - Main Menu
    //TODO: - Ending


}
