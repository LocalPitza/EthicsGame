using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _minIntensity = 0.5f;
    [SerializeField] private float _maxIntensity = 1f;
    [SerializeField] private float _minSpeed = 0.1f;
    [SerializeField] private float _maxSpeed = 3f;

    void Start()
    {
        if (_light == null)
        {
            _light = GetComponent<Light>();
        }
        StartCoroutine(Flicker());
    }

    private void OnDisable()
    {
        StopCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            _light.intensity = Random.Range(_minIntensity, _maxIntensity);

            yield return new WaitForSeconds(Random.Range(_minSpeed, _maxSpeed));

            _light.intensity = Random.Range(_minIntensity, _maxIntensity);

            yield return new WaitForSeconds(Random.Range(_minSpeed, _maxSpeed));
        }
    }
}
