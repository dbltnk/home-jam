using UnityEngine;
using TMPro;

public class ChaosCounter : MonoBehaviour
{
    public float Chaos;
    public TMP_Text text;
    
    void Awake() {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        Chaos = 0f;
        foreach (var chaosObject in ChaosObject.ActiveObjects)
        {
            if (chaosObject.ContributesToChaos) {
                Chaos += chaosObject.PosChaos;// + chaosObject.RotChaos;
            }
        }

        text.text = "Chaos: " + Chaos.ToString("0");
    }
}
