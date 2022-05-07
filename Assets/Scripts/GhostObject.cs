using System.Collections.Generic;
using UnityEngine;

public class GhostObject : MonoBehaviour
{
    public static readonly List<GhostObject> ActiveObjects = new List<GhostObject>();
    
    public ObjectType ObjectType;

    [SerializeField] private MeshFilter Mesh;

    private void OnEnable()
    {
        ActiveObjects.Add(this);
    }

    private void OnDisable()
    {
        ActiveObjects.Remove(this);
    }
    
    public void TakeShape(GameObject go, ObjectType objectType)
    {
        var mesh = go.GetComponentInChildren<MeshFilter>();
        Mesh.sharedMesh = mesh.sharedMesh;
        ObjectType = objectType;
    }
}