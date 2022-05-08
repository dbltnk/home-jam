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

    void Awake()
    {
        Inputs = new JellyInputs();
        Inputs.Enable();
    }

    void Update()
    {
        if (Inputs.Player.Restart.triggered || LegacyInput.RestartTriggered)
        {
            SceneManager.LoadScene(MainLevel);
        }
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
        var safeZone = FindObjectOfType<SafeZone>().Zone;
        foreach (var it in ChaosObject.ActiveObjects) it.Randomize(safeZone);
        // play
        JellyController.enabled = true;
        JellyCamera.enabled = true;
    }

}
