using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JellyController : MonoBehaviour
{
    private Rigidbody Rigidbody;
    private float Charge;

    [SerializeField] private Camera Camera;
    [SerializeField] private float ChargePerSecond;
    [SerializeField] private float Force;
    [SerializeField] private float ForceUpwards; 

    private Transform inventory;
    private float inventoryCount => inventory.childCount;
        
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        inventory = transform.Find("Inventory");
    }

    private void Update()
    {

        float size = 1f + 0.1f * inventoryCount;
        transform.localScale = new Vector3(size, size, size);
        print(size);

        foreach (var child in inventory.GetComponentsInChildren<Transform>())
        {
            float childSize = 0.5f;
            child.localScale = new Vector3(childSize, childSize, childSize);
            // Move each child to a slightly randomized position
            child.position = child.position + Random.insideUnitSphere * 0.01f;
        }

        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.

        Vector2 move = gamepad.leftStick.ReadValue();
    
        if (gamepad.aButton.isPressed)
        {
            Charge += ChargePerSecond * Time.deltaTime;
            Charge = Mathf.Clamp(Charge, 0f, 1f);
        }
        
        if (gamepad.aButton.wasReleasedThisFrame)
        {
            var forceDir = CalcMoveDirection().normalized * Charge * Force;
            Rigidbody.AddForceAtPosition(forceDir, transform.position);
            Charge = 0f;
        }

        JellyUI.Instance.Charge = Charge;
    }

    Vector3 CalcMoveDirection()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return Vector3.zero; // No gamepad connected.
        
        var fx = (gamepad.leftStick.ReadValue().x + 1f) / 2f; 
        var fy = (gamepad.leftStick.ReadValue().y + 1f) / 2f;
        var dirx = Vector3.Lerp(-Camera.transform.right, Camera.transform.right, fx);
        var diry = Vector3.Lerp(-Camera.transform.forward, Camera.transform.forward, fy);
        var dir = (dirx + diry).normalized;
        var dirOnPlane = Vector3.ProjectOnPlane(dir, Vector3.up).normalized;
        var dirMove = (dirOnPlane + Vector3.up * ForceUpwards).normalized;
        return dirMove;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        var dirOnPlane = CalcMoveDirection();
        Gizmos.DrawLine(transform.position, transform.position + dirOnPlane);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.GetComponent<ChaosObject>() != null) {
            go.GetComponent<Rigidbody>().isKinematic = true;
            go.GetComponent<Collider>().enabled = false;
            go.transform.parent = inventory;
            go.transform.position = inventory.position + Random.insideUnitSphere * transform.localScale.x * 0.5f;
        }
    }
}
