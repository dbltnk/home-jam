using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JellyUI : MonoBehaviour
{
    public static JellyUI Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Image ChargeImage;
    [SerializeField] private TMP_Text SelectedText;


    public float Charge
    {
        get
        {
            return ChargeImage.transform.localScale.x;
        }
        set
        {
            var scale = ChargeImage.transform.localScale;
            scale.x = value;
            ChargeImage.transform.localScale = scale;
        }
    }

    public void SetSelectedText(string text)
    {
        SelectedText.text = text;
    }
}
