using UnityEngine;

public class AudioOnCollision : MonoBehaviour
{
    [SerializeField] private string AudioName;
    [SerializeField] private float MinVelocity = 1f;
    
    private void OnCollisionEnter(Collision collision)
    {
        var vel = collision.relativeVelocity.magnitude;
        if (vel > MinVelocity)
        {
            Audio.Instance.PlayAt(AudioName, collision.transform.position);    
        }
    }
}
