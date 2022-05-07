using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] private string MainLevel = "Scenes/SampleScene";
    [SerializeField] private JellyController JellyController;
    [SerializeField] private JellyCamera JellyCamera;

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

    IEnumerator Start()
    {
        // disable inputs
        JellyController.enabled = false;
        JellyCamera.enabled = false;
        // wait for settle
        while (ChaosObject.ActiveObjects.All(it => it.Settled)) yield return null;
        // spawn ghost
        foreach (var it in ChaosObject.ActiveObjects) it.SpawnGhost();
        // randomize
        foreach (var it in ChaosObject.ActiveObjects) it.Randomize();
        // play
        JellyController.enabled = true;
        JellyCamera.enabled = true;
    }

}
