using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRotate : MonoBehaviour
{
    [SerializeField] bool freezeXZAxis = true;

    void Update()
    {
        if (freezeXZAxis)
        {
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
