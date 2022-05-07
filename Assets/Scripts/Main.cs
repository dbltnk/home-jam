using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] private string MainLevel = "Scenes/SampleScene";
    
    private JellyInputs Inputs;

    void Awake()
    {
        Inputs = new JellyInputs();
        Inputs.Enable();
    }

    void Update()
    {
        if (Inputs.Player.Restart.triggered)
        {
            SceneManager.LoadScene(MainLevel);
        }
    }
}
