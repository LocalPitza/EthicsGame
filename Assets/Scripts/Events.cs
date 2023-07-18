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
    [SerializeField] private GameObject[] _objectsToSpawnAfterTalkingToDetective;

    private void Start()
    {
        FirstPersonController.instance.CanMove = false;
        FirstPersonController.instance.ToggleCrouchStand();
    }

    private void OnEnable()
    {
        Lua.RegisterFunction("TeleportToApartment", this, SymbolExtensions.GetMethodInfo(() => TeleportToApartment()));
        Lua.RegisterFunction("TeleportToInterrogationRoom", this, SymbolExtensions.GetMethodInfo(() => TeleportToInterrogationRoom()));
        Lua.RegisterFunction("TakeCurrentObject", this, SymbolExtensions.GetMethodInfo(() => TakeCurrentObject()));
        Lua.RegisterFunction("StandAndLeave", this, SymbolExtensions.GetMethodInfo(() => StandAndLeave()));
        Lua.RegisterFunction("InterrogationRoomDoor", this, SymbolExtensions.GetMethodInfo(() => InterrogationRoomDoor()));
        
        Lua.RegisterFunction("FadeToBlack", this, SymbolExtensions.GetMethodInfo(() => FadeToBlack()));
        Lua.RegisterFunction("FadeToNormal", this, SymbolExtensions.GetMethodInfo(() => FadeToNormal()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("TeleportToApartment");
        Lua.UnregisterFunction("TeleportToInterrogationRoom");
        Lua.UnregisterFunction("TakeCurrentObject");
        Lua.UnregisterFunction("StandAndLeave");
        Lua.UnregisterFunction("InterrogationRoomDoor");
        
        Lua.UnregisterFunction("FadeToBlack");
        Lua.UnregisterFunction("FadeToNormal");
        
    }

    public void TeleportToApartment()
    {
        //for some reason, it doesnt teleport if character controller is enabled
        
        _blackScreen.DOFade(1f, 2f).onComplete = () =>
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = _apartmentSpawnPoint.position;
            transform.rotation = _apartmentSpawnPoint.rotation;
            
            FirstPersonController.instance.ToggleCrouchStand();
            FirstPersonController.instance.CanMove = true;
            
            GetComponent<CharacterController>().enabled = true;

            _blackScreen.DOFade(0f, 5f).SetEase(Ease.InQuad);
        };

    }

    public void TeleportToInterrogationRoom()
    {
        GetComponent<CharacterController>().enabled = false;

        transform.position = _interrogationRoomSpawnPoint.position;
        transform.rotation = _interrogationRoomSpawnPoint.rotation;
        
        GetComponent<CharacterController>().enabled = true;
        
        FirstPersonController.instance.ResetMoveDir();
        FirstPersonController.instance.CanMove = false;
        FirstPersonController.instance.ToggleCrouchStand();
        
        _blackScreen.DOFade(0f, 5f);
    }

    public void FadeToBlack()
    {
        _blackScreen.DOFade(1f, 1f);
    }
    
    public void FadeToNormal()
    {
        _blackScreen.DOFade(0f, 2f);
    }

    public void TakeCurrentObject()
    {
        FirstPersonController.instance.currentInteractable.gameObject.SetActive(false);
    }

    public void StandAndLeave()
    {
        foreach (GameObject gameObject in _objectsToSpawnAfterTalkingToDetective)
        {
            gameObject.SetActive(true);
        }
        
        FirstPersonController.instance.CanMove = true;
        FirstPersonController.instance.ToggleCrouchStand();
    }

    public void InterrogationRoomDoor()
    {
        //DialogueLua.SetVariable("DetectiveStartingIndex", 3);
        Debug.Log("test");
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
