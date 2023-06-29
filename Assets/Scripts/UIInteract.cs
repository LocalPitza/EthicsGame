using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIInteract : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private GameObject holder;

    #region Instance

    public static  UIInteract Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    private void ChangeText(string text)
    {
        Instance.interactText.text = text;
    }

    public void ShowText(string text)
    {
        ChangeText(text);
        Instance.holder.SetActive(true);
    }

    public void HideText()
    {
        Instance.holder.SetActive(false);
    }
}
