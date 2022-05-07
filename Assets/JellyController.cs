using System;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JellyController : MonoBehaviour
{
    private Rigidbody Rigidbody;
    private float Charge;
    
    [SerializeField] private float ChargePerSecond;
    [SerializeField] private float Force;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private Vector3 CalcForce(float Factor)
    {
        return Vector3.forward * Factor * Force;
    }
    
    private void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.

        if (gamepad.rightTrigger.wasPressedThisFrame)
        {
            // 'Use' code here
        }

        Vector2 move = gamepad.leftStick.ReadValue();
    
        if (gamepad.aButton.isPressed)
        {
            Charge += ChargePerSecond * Time.deltaTime;
            Charge = Mathf.Clamp(Charge, 0f, 1f);
        }
        
        if (gamepad.aButton.wasReleasedThisFrame)
        {
            Rigidbody.AddForceAtPosition(Rigidbody.transform.position, CalcForce(Charge));
            Charge = 0f;
        }

        JellyUI.Instance.Charge = Charge;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + CalcForce(Charge));
    }
}
