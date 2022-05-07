using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosObject : MonoBehaviour
{
    public bool Settled;
    public Vector3 InitialPosition;
    public Vector3 InitialRotation;

    public float PosChaos => (Settled) ? (transform.position - InitialPosition).magnitude : 0f;
    public float RotChaos => (Settled) ? (transform.rotation.eulerAngles - InitialRotation).magnitude : 0f;

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
