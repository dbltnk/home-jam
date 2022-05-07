using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

public class ChaosObject : MonoBehaviour
{
    public static readonly List<ChaosObject> ActiveObjects = new List<ChaosObject>();
    
    [SerializeField] private float MinToBeforeSettle = 1f;
    [SerializeField] private Assets Assets;
    [SerializeField] private bool CanSpawnGhost;
    [SerializeField] public ObjectType ObjectType;
    [SerializeField] public Renderer Renderer;
    [SerializeField] private float BlinkScale = 10f;
    [SerializeField] private Color BlinkColor = Color.black;
    
    private bool HasFoundInitial;
    private Vector3 InitialPosition;
    private Vector3 InitialRotation;
    private float WaitingToSettleStartTime;
    
    public enum State
    {
        WAITING_TO_SETTLE,
        SETTLED,
        CARRIED,
    }
    
    private State CurrentState;

    public bool ContributesToChaos => HasFoundInitial && GetComponent<DeliverObject>() == null;
    
    public float PosChaos => HasFoundInitial ? (transform.position - InitialPosition).magnitude : 0f;
    public float RotChaos => HasFoundInitial ? (transform.rotation.eulerAngles - InitialRotation).magnitude : 0f;
    public bool Carried => CurrentState == State.CARRIED;

    public float TimeStampLastReleased;

    public float PickupTimeoutAfterRelease = 1f;
    public bool CanBePickedUp => Time.time - TimeStampLastReleased > PickupTimeoutAfterRelease; //CurrentState == State.SETTLED;

    public bool HasBeenMoved => PosChaos >= 1f || RotChaos >= 1f;
    public bool Settled => CurrentState == State.SETTLED;

    private Rigidbody _rigidbody;
    private GhostObject Ghost;
    private JellyController jellyController;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    
    TrailRenderer trailRenderer;


    void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        Release();
        Renderer = GetComponentInChildren<Renderer>();
        jellyController = FindObjectOfType<JellyController>();
    }

    private void OnEnable()
    {
        ActiveObjects.Add(this);
    }

    private void OnDisable()
    {
        ActiveObjects.Remove(this);
    }

    void FixedUpdate()
    {
        if (CurrentState == State.WAITING_TO_SETTLE && Time.time - WaitingToSettleStartTime > MinToBeforeSettle) {
            if (_rigidbody.velocity.sqrMagnitude < 0.001f && _rigidbody.angularVelocity.sqrMagnitude < 0.001f) {
                Settle();
            }
        }

        if (transform.position.y < -2f) {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }

        if (Ghost == null)
        {
            if (CurrentState == State.WAITING_TO_SETTLE || CurrentState == State.SETTLED)
            {
                TryToSnapToNearbyGhost();
            }
        }
        else
        {
            ResetToGhost();
        }

        var blinking = CanBePickedUpBy(jellyController.Size);
        var f = Utils.MapIntoRange(Mathf.Sin(Time.time * BlinkScale), -1f, 1f, 0f, 1f);
        var color = blinking ? Color.Lerp(Color.white, BlinkColor, f) : Color.white;
        Renderer.material.SetColor(BaseColor, color);
        trailRenderer.enabled = !Carried;
    }

    void Settle()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.ResetInertiaTensor();
        
        if (!HasFoundInitial)
        {
            HasFoundInitial = true;
            InitialPosition = transform.position;
            InitialRotation = transform.rotation.eulerAngles;
        }
        CurrentState = State.SETTLED;
    }

    public void SpawnGhost()
    {
        var go = Instantiate(Assets.GhostPrefab, transform.position, transform.rotation);
        var ghost = go.GetComponent<GhostObject>();
        ghost.TakeShape(gameObject, ObjectType);
    }

    public void PickUp()
    {
        CurrentState = State.CARRIED;
        if (Ghost != null)
        {
            Ghost.gameObject.SetActive(true);
            Ghost = null; 
        }
    }

    public void Release()
    {
        CurrentState = State.WAITING_TO_SETTLE;
        WaitingToSettleStartTime = Time.time;
        TimeStampLastReleased = Time.time;
    }

    public void TryToSnapToNearbyGhost()
    {
        var pos = transform.position;
        
        foreach (var it in GhostObject.ActiveObjects)
        {
            if (it.ObjectType == ObjectType &&
                Vector3.Distance(pos, it.transform.position) < it.ObjectType.GhostSnapRadius)
            {
                Ghost = it;
                Ghost.gameObject.SetActive(false);
                Settle();
                ResetToGhost();
                break;
            }
        }
    }

    private void ResetToGhost()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.ResetInertiaTensor();
        transform.position = Ghost.transform.position;
        transform.rotation = Ghost.transform.rotation;
    }

    private void OnDrawGizmosSelected()
    {
        if (ObjectType != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, ObjectType.GhostSnapRadius);
        }
    }

    public void Randomize()
    {
        transform.position += Random.insideUnitSphere + new Vector3(0f, 1.5f, 0f);
        transform.rotation = Quaternion.Euler(Random.insideUnitSphere * 360f);
    }

    public bool CanBePickedUpBy(float size)
    {
        return CanBePickedUp && ObjectType.MinSizeToPickup <= size;
    }
}
