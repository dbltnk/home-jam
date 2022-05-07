using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChaosCounter : MonoBehaviour
{
    ChaosObject[] chaosObjects;
    public float Chaos;
    public TMP_Text text;

    void Awake() {
        text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        chaosObjects = FindObjectsOfType<ChaosObject>();
    }

    void Update()
    {
        Chaos = 0f;
        foreach (var chaosObject in chaosObjects)
        {
            if (chaosObject.ContributesToChaos) {
                Chaos += chaosObject.PosChaos;// + chaosObject.RotChaos;
            }     
        }

        text.text = "Chaos: " + Chaos.ToString("0");

    }
}
