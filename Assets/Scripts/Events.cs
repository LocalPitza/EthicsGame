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
    [SerializeField] private AudioSource _jazzSound;

    [Header("Look points")]
    [SerializeField] private Transform _lookAtDetective;
    [SerializeField] private Transform _lookAtWife;

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
        
        Lua.RegisterFunction("LookAtDetective", this, SymbolExtensions.GetMethodInfo(() => LookAtDetective()));
        Lua.RegisterFunction("LookAtWife", this, SymbolExtensions.GetMethodInfo(() => LookAtWife()));
        
        Lua.RegisterFunction("FadeToBlack", this, SymbolExtensions.GetMethodInfo(() => FadeToBlack()));
        Lua.RegisterFunction("FadeToNormal", this, SymbolExtensions.GetMethodInfo(() => FadeToNormal()));
        
        Lua.RegisterFunction("PlayDeathSound", this, SymbolExtensions.GetMethodInfo(() => PlayDeathSound()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("TeleportToApartment");
        Lua.UnregisterFunction("TeleportToInterrogationRoom");
        Lua.UnregisterFunction("TakeCurrentObject");
        Lua.UnregisterFunction("StandAndLeave");
        Lua.UnregisterFunction("InterrogationRoomDoor");
        
        Lua.UnregisterFunction("LookAtDetective");
        Lua.UnregisterFunction("LookAtWife");

        Lua.UnregisterFunction("FadeToBlack");
        Lua.UnregisterFunction("FadeToNormal");
        
        Lua.UnregisterFunction("PlayDeathSound");
        
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

            _jazzSound.DOFade(1, 0.5f);
            _blackScreen.DOFade(0f, 5f).SetEase(Ease.InQuad);
        };

    }

    public void TeleportToInterrogationRoom()
    {
        GetComponent<CharacterController>().enabled = false;

        _jazzSound.Stop();
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

    public void LookAtDetective()
    {
        FirstPersonController.instance.ForceLookAtObject(_lookAtDetective);
    }
    
    public void LookAtWife()
    {
        FirstPersonController.instance.ForceLookAtObject(_lookAtWife);
    }

    public void PlayDeathSound()
    {
        StartCoroutine(DeathAudioSequence());
    }

    IEnumerator DeathAudioSequence()
    {
        yield return new WaitForSeconds(0.25f);
        SoundManager.Instance.PlayOnceMain(SoundManager.Sounds.PushBody);
        yield return new WaitForSeconds(0.25f);
        SoundManager.Instance.PlayOnceMain(SoundManager.Sounds.WomanGasp);
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlayOnceMain(SoundManager.Sounds.MirrorBreak);
        yield return new WaitForSeconds(0.25f);
        SoundManager.Instance.PlayOnceMain(SoundManager.Sounds.HitHead);
        yield return new WaitForSeconds(0.75f);
        SoundManager.Instance.PlayOnceMain(SoundManager.Sounds.FallBody);
        
    }
    
    //TODO: - Fix sometimes not teleporting to apartment (done)
    //TODO: - Update Outline (done)
    //TODO: - Fix door colliding when toggled (done)
    //TODO: - Add bool values to objects (finished on existing objects)
    //TODO: - Wife Dialogue (done)
    //TODO: - Place interactable objects (story wise)
    //TODO: - Update 2D sprites of characters (done, also added changer)
    //TODO: - Sound manager (Done)
    //TODO: - Update UI aesthetics
    //TODO: - Main Menu
    //TODO: - Ending, credits
    //TODO: - Add more assets
    
    


}
