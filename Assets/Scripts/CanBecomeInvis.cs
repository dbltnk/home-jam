using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBecomeInvis : MonoBehaviour
{
    public static readonly List<CanBecomeInvis> ActiveObjects = new List<CanBecomeInvis>();

    private Renderer[] Renderers;
    
    private void OnEnable()
    {
        ActiveObjects.Add(this);
        Renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnDisable()
    {
        ActiveObjects.Remove(this);
    }

    public void Hide()
    {
        foreach (var it in Renderers) it.enabled = false;
    }
    
    public void Show()
    {
        foreach (var it in Renderers) it.enabled = true;
    }
}
