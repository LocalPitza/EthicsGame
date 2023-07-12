using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Events : MonoBehaviour
{

    [SerializeField] private Image _blackScreen;
    [SerializeField] private Transform _apartmentSpawnPoint;
    [SerializeField] private Transform _interrogationRoomSpawnPoint;
    
    public void TeleportToApartment()
    {
        _blackScreen.DOFade(1f, 2f).onComplete = () =>
        {
            transform.position = _apartmentSpawnPoint.position;
            
            transform.rotation = _apartmentSpawnPoint.rotation;
            
            _blackScreen.DOFade(1f, 2f).onComplete = () =>
            {
                _blackScreen.DOFade(0f, 3f);
            };

        };

    }

    public void TeleportToInterrogationRoom()
    {
        _blackScreen.DOFade(1f, 2f).onComplete = () =>
        {
            transform.position = _interrogationRoomSpawnPoint.position;
            transform.rotation = _interrogationRoomSpawnPoint.rotation;
            
            _blackScreen.DOFade(1f, 2f).onComplete = () =>
            {
                _blackScreen.DOFade(0f, 3f);
            };

        };
    }
    
    //TODO: - Fix sometimes not teleporting to apartment
    //TODO: - Fix sometimes outline of object not disappearing
    //TODO: - Add bool values to objects
    //TODO: - Wife Dialogue
    //TODO: - Place interactable objects (story wise)
    //TODO: - Update 2D sprites of characters
    //TODO: - Sound manager
    //TODO: - Update UI aesthetics
    //TODO: - Main Menu
    //TODO: - Ending


}
