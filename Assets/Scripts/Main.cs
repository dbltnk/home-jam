using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] private string MainLevel = "Scenes/SampleScene";
    
    private JellyInputs Inputs;

    public GameObject WinText;

    private StartZone start;
    private DropZone drop;

    void Awake()
    {
        Inputs = new JellyInputs();
        Inputs.Enable();
        start = FindObjectOfType<StartZone>();
        drop = FindObjectOfType<DropZone>();
    }

    void Update()
    {
        if (Inputs.Player.Restart.triggered || LegacyInput.RestartTriggered)
        {
            SceneManager.LoadScene(MainLevel);
        }

        //if (start.PlayerIsInStartZone && drop.ObjectHasBeenDelivered)
        //{
        //    WinText.SetActive(true);
        //}
    }

}
