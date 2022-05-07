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

    private JellyInputs Inputs;
    
    private Transform inventory;
    private float inventoryCount => inventory.childCount;
        
    private void Awake()
    {
        Inputs = new JellyInputs();
        Inputs.Enable();
        Rigidbody = GetComponent<Rigidbody>();
        inventory = transform.Find("Inventory");
    }

    private void Update()
    {

        float size = 1f + 0.1f * inventoryCount;
        transform.localScale = new Vector3(size, size, size);
        
        foreach (Transform child in inventory)
        {
            float childSize = 0.5f;
            child.localScale = new Vector3(childSize, childSize, childSize);
            // Move each child to a slightly randomized position
            child.position = child.position + Random.insideUnitSphere * 0.01f;
        }
        
        if (Inputs.Player.Jump.IsPressed())
        {
            Charge += ChargePerSecond * Time.deltaTime;
            Charge = Mathf.Clamp(Charge, 0f, 1f);
        }
        
        if (Inputs.Player.Jump.WasReleasedThisFrame())
        {
            var forceDir = CalcMoveDirection().normalized * Charge * Force;
            Rigidbody.AddForceAtPosition(forceDir, transform.position);
            Charge = 0f;
        }

        if (Inputs.Player.Release.triggered)
        {
            foreach (Transform child in inventory)
            {
                ReleaseObject(child.gameObject);
            }
        }
        
        JellyUI.Instance.Charge = Charge;
    }

    Vector3 CalcMoveDirection()
    {
        if (Inputs == null) return Vector3.zero;
        Vector2 move = Inputs.Player.Move.ReadValue<Vector2>();
        var fx = (move.x + 1f) / 2f; 
        var fy = (move.y + 1f) / 2f;
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

    void CaptureObject(GameObject go)
    {
        go.GetComponent<Rigidbody>().isKinematic = true;
        go.GetComponent<Collider>().enabled = false;
        go.transform.parent = inventory;
        go.transform.position = inventory.position + Random.insideUnitSphere * transform.localScale.x * 0.5f;
    }

    void ReleaseObject(GameObject go)
    {
        go.GetComponent<Rigidbody>().isKinematic = false;
        go.GetComponent<Collider>().enabled = true;
        go.transform.SetParent(null);
        go.transform.localScale = Vector3.one;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.GetComponent<ChaosObject>() != null) {
            CaptureObject(go);
        }
    }
}
