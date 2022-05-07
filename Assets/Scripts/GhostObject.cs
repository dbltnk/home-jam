using UnityEngine;

public class GhostObject : MonoBehaviour
{
    public GameObject Original;

    [SerializeField] private MeshFilter Mesh;

    public void TakeShape(GameObject go)
    {
        var mesh = go.GetComponentInChildren<MeshFilter>();
        Mesh.sharedMesh = mesh.sharedMesh;
        Original = go;
    }
}