using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChaosCounter : MonoBehaviour
{
    ChaosObject[] chaosObjects;
    public float Chaos;
    public TMP_Text text;
    private JellyController jellyController;

    public float AttractionDistance = 5f;
    public float AttractionForce = 4f;

    void Awake() {
        text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        chaosObjects = FindObjectsOfType<ChaosObject>();
        jellyController = FindObjectOfType<JellyController>();
    }

    void Update()
    {
        Chaos = 0f;
        foreach (var chaosObject in chaosObjects)
        {
            if (chaosObject.ContributesToChaos) {
                Chaos += chaosObject.PosChaos;// + chaosObject.RotChaos;
            }     

            if (!chaosObject.Carried) {
                Vector3 dirToJelly = (jellyController.transform.position - chaosObject.transform.position);       
                float distanceToJelly = dirToJelly.magnitude;
                if (distanceToJelly < AttractionDistance) chaosObject.GetComponent<Rigidbody>().AddForce(dirToJelly * AttractionForce);
            }
        }

        text.text = "Chaos: " + Chaos.ToString("0");

    }
}
