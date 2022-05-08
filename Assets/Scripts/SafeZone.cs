using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public Bounds Zone;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(Zone.center, Zone.size);
    }
}
