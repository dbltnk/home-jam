using UnityEngine;

public class ChaosObject : MonoBehaviour
{
    [SerializeField] private float MinToBeforeSettle = 1f;

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

    public bool ContributesToChaos => HasFoundInitial;
    
    public float PosChaos => HasFoundInitial ? (transform.position - InitialPosition).magnitude : 0f;
    public float RotChaos => HasFoundInitial ? (transform.rotation.eulerAngles - InitialRotation).magnitude : 0f;
    public bool Settled => CurrentState == State.SETTLED;
    public bool Carried => CurrentState == State.CARRIED;

    public bool CanBePickedUp => CurrentState == State.SETTLED;

    private Rigidbody _rigidbody;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        Release();
    }

    void Update()
    {
        if (CurrentState == State.WAITING_TO_SETTLE && Time.time - WaitingToSettleStartTime > MinToBeforeSettle) {
            if (_rigidbody.velocity.sqrMagnitude < 0.001f && _rigidbody.angularVelocity.sqrMagnitude < 0.001f) {
                Settle();
            }
        }   
    }

    void Settle()
    {
        if (!HasFoundInitial)
        {
            HasFoundInitial = true;
            InitialPosition = transform.position;
            InitialRotation = transform.rotation.eulerAngles;
        }
        CurrentState = State.SETTLED;
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
