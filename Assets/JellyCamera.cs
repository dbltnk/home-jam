using UnityEngine;

public class JellyCamera : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    [SerializeField] private float Distance;
    [SerializeField] private float Height;
    [SerializeField] private float RotationSpeed;

    private JellyInputs Inputs;
    
    private void Awake()
    {
        Inputs = new JellyInputs();
        Inputs.Enable();
    }

    private void LateUpdate()
    {
        if (Inputs == null) return;
        var rotate = Inputs.Player.Move.ReadValue<Vector2>();
        var rotDelta = rotate.x * RotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotDelta, 0f, Space.World);
        
        var forwardOnPlane = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        var offset = -forwardOnPlane * Distance + Vector3.up * Height;
        transform.position = Target.transform.position + offset;
    }
}
