using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosObject : MonoBehaviour
{
    public bool Settled;
    public Vector3 InitialPosition;
    public Vector3 InitialRotation;

    public float PosChaos => (transform.position - InitialPosition).magnitude;
    public float RotChaos => (transform.rotation.eulerAngles - InitialRotation).magnitude;

    private Rigidbody _rigidbody;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!Settled) {
            if (_rigidbody.velocity.sqrMagnitude < 0.001f && _rigidbody.angularVelocity.sqrMagnitude < 0.001f) {
                NoteInitialState();
            }
        }   
    }

    void NoteInitialState()
    {
        InitialPosition = transform.position;
        InitialRotation = transform.rotation.eulerAngles;
        Settled = true;
    }
}
