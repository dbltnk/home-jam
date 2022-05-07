using System;
using UnityEngine;
using UnityEngine.UI;

public class JellyUI : MonoBehaviour
{
    public static JellyUI Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Image ChargeImage;

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
}
