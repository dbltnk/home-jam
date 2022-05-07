using System.Collections.Generic;
using UnityEngine;

public class ChaosObject : MonoBehaviour
{
    public static readonly List<ChaosObject> ActiveObjects = new List<ChaosObject>();
    
    [SerializeField] private float MinToBeforeSettle = 1f;
    [SerializeField] private Assets Assets;
    [SerializeField] private bool CanSpawnGhost;
    [SerializeField] private ObjectType ObjectType;

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
    public bool Settled => CurrentState == State.SETTLED;
    public bool Carried => CurrentState == State.CARRIED;

    public bool CanBePickedUp => CurrentState == State.SETTLED;

    public bool HasBeenMoved => PosChaos >= 1f || RotChaos >= 1f;

    private Rigidbody _rigidbody;
    private GhostObject Ghost;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        Release();
    }

    private void OnEnable()
    {
        ActiveObjects.Add(this);
    }

    private void OnDisable()
    {
        ActiveObjects.Remove(this);
    }

    void Update()
    {
        if (CurrentState == State.WAITING_TO_SETTLE && Time.time - WaitingToSettleStartTime > MinToBeforeSettle) {
            if (_rigidbody.velocity.sqrMagnitude < 0.001f && _rigidbody.angularVelocity.sqrMagnitude < 0.001f) {
                Settle();
            }
        }

        if (HasBeenMoved && Settled && Ghost != null) Ghost.gameObject.SetActive(true);  

        if (transform.position.y < 0f) {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }
                
    }

    void Settle()
    {
        if (!HasFoundInitial)
        {
            HasFoundInitial = true;
            InitialPosition = transform.position;
            InitialRotation = transform.rotation.eulerAngles;
            if (CanSpawnGhost)
            {
                SpawnGhost();
            }
        }
        CurrentState = State.SETTLED;

        transform.position += UnityEngine.Random.insideUnitSphere + new Vector3(0f, 1.5f, 0f);
        transform.rotation = Quaternion.Euler(UnityEngine.Random.insideUnitSphere * 360f);
    }

    private void SpawnGhost()
    {
        var go = Instantiate(Assets.GhostPrefab, transform.position, transform.rotation);
        Ghost = go.GetComponent<GhostObject>();
        Ghost.TakeShape(gameObject);
        go.SetActive(false);
    }

    public void PickUp()
    {
        CurrentState = State.CARRIED;
    }

    public void Release()
    {
        CurrentState = State.WAITING_TO_SETTLE;
        WaitingToSettleStartTime = Time.time;
    }
}
