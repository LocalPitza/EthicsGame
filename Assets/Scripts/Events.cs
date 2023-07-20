using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

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
    
    [Header("MainMenu")]
    [SerializeField] private GameObject menuHolder;
    [SerializeField] private GameObject cameraMenu;
    [SerializeField] private GameObject roomMainMenu;

    [Header("Endings")]
    [SerializeField] private TextMeshProUGUI ending1;
    [SerializeField] private TextMeshProUGUI ending2;
    [SerializeField] private TextMeshProUGUI ending3;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject quitButton;

    private void Start()
    {
        FirstPersonController.instance.CanMove = false;
        FirstPersonController.instance.ToggleCrouchStand();

        FirstPersonController.instance.enabled = false;
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
        
        Lua.RegisterFunction("ShowEnding1", this, SymbolExtensions.GetMethodInfo(() => ShowEnding1()));
        Lua.RegisterFunction("ShowEnding2", this, SymbolExtensions.GetMethodInfo(() => ShowEnding2()));
        Lua.RegisterFunction("ShowEnding3", this, SymbolExtensions.GetMethodInfo(() => ShowEnding3()));
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
        
        Lua.UnregisterFunction("ShowEnding1");
        Lua.UnregisterFunction("ShowEnding2");
        Lua.UnregisterFunction("ShowEnding3");
        
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
        _jazzSound.DOPitch(0.2f, 10f);
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

    public void CreditsButton()
    {
        ending1.DOFade(0, 1);
        ending2.DOFade(0, 1);
        ending3.DOFade(0, 1).onComplete = () =>
        {
            creditsText.DOFade(1, 1);
            creditsButton.SetActive(false);
            quitButton.SetActive(true);
        };
    }

    public void ShowEnding1()
    {
        FirstPersonController.instance.enabled = false;
        
        ending1.DOFade(1, 1).onComplete = () => { StartCoroutine(ShowCreditsButton());};
    }
    
    public void ShowEnding2()
    {
        FirstPersonController.instance.enabled = false;
        
        ending2.DOFade(1, 1).onComplete = () => { StartCoroutine(ShowCreditsButton());};;
    }
    
    public void ShowEnding3()
    {
        FirstPersonController.instance.enabled = false;
        
        ending3.DOFade(1, 1).onComplete = () => { StartCoroutine(ShowCreditsButton());};;
    }

    IEnumerator ShowCreditsButton()
    {
        yield return new WaitForSeconds(5f);
        creditsButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    
    //mainmenu
    
    
    public void Play()
    {
        FirstPersonController.instance.enabled = true;
        FirstPersonController.instance.canInteract = false;
        _blackScreen.DOFade(1, 1).onComplete = () =>
        {
            menuHolder.SetActive(false);
            cameraMenu.SetActive(false);
            roomMainMenu.SetActive(false);
            

            _blackScreen.DOFade(0, 1).onComplete = () =>
            {
                FirstPersonController.instance.canInteract = true;
            };

        };
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    //TODO: - Fix sometimes not teleporting to apartment (done)
    //TODO: - Update Outline (done)
    //TODO: - Fix door colliding when toggled (done)
    //TODO: - Add bool values to objects (finished on existing objects)
    //TODO: - Wife Dialogue (done)
    //TODO: - Place interactable objects (story wise)
    //TODO: - Update 2D sprites of characters (done, also added changer)
    //TODO: - Sound manager (Done)
    //TODO: - Update UI aesthetics (Done)
    //TODO: - Main Menu (done)
    //TODO: - Ending, credits (Done)
    //TODO: - Add more assets (Somewhat Done)
    
    


}
