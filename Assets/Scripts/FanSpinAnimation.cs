using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FanSpinAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _propeler;
    [SerializeField] private float _speed;
    
    // Start is called before the first frame update
    void Start()
    {
        startSpin();
    }

    void startSpin()
    {
        _propeler.transform.DOLocalRotate(new Vector3(0, 0, 360), _speed, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).onComplete = startSpin;
    }

}
