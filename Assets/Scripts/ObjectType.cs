using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ObjectType", order = 1)]
public class ObjectType : ScriptableObject
{
    public float GhostSnapRadius;
    public float SizeGainOnCarry;
    public float MinSizeToPickup;
}
