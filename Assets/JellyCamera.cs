using System;
using UnityEngine;

public class JellyCamera : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    [SerializeField] private float Distance;
    [SerializeField] private float Height;

    private void LateUpdate()
    {
        var forwardOnPlane = Vector3.ProjectOnPlane(Target.transform.forward, Vector3.up).normalized;
        var offset = -forwardOnPlane * Distance + Vector3.up * Height; 
        transform.position = Target.transform.position + offset;
    }
}
