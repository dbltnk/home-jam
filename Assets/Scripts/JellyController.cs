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

    [SerializeField] private float size;
    
    public float Size => size;
    public GameObject DecalSlime;

    private AudioSource audioSource;

    private int selectedItemIndex = 0;
        
    private void Awake()
    {
        Inputs = new JellyInputs();
        Inputs.Enable();
        Rigidbody = GetComponent<Rigidbody>();
        inventory = transform.Find("Inventory");
        audioSource = GetComponent<AudioSource>();
        var jellyCam = FindObjectOfType<JellyCamera>();
        jellyCam.SetTarget(gameObject);
        Camera = jellyCam.GetComponent<Camera>();
    }

    private Vector3 UnScaleObject(GameObject o) {
        Vector3 scaleTmp = o.transform.localScale;
        scaleTmp.x /= o.transform.parent.localScale.x;
        scaleTmp.y /= o.transform.parent.localScale.y;
        scaleTmp.z /= o.transform.parent.localScale.z;
        return scaleTmp;
    }

    private void Update()
    {   
        foreach (var o in CanBecomeInvis.ActiveObjects)
        {
            o.Show();
        }

        // cast a ray from the camera to the transform
        RaycastHit[] hits = Physics.RaycastAll(Camera.transform.position, transform.position - Camera.transform.position, (transform.position - Camera.transform.position).magnitude);
        //Ray ray = new Ray(Camera.transform.position, transform.position - Camera.transform.position);
        //RaycastHit hit;
        foreach (var hit in hits) {
           CanBecomeInvis c = hit.collider.GetComponentInParent<CanBecomeInvis>();
           if (c != null) c.Hide();
        }      

        size = 0.5f;
        foreach (Transform child in inventory)
        {
            size+= child.GetComponent<ChaosObject>().ObjectType.SizeGainOnCarry;
        }
        foreach (Transform child in inventory)
        {
            child.position = child.position + Random.insideUnitSphere * 0.01f;
            float delta = size / 2f;
            child.position = new Vector3(   
                Mathf.Clamp(child.position.x, transform.position.x - delta, transform.position.x + delta),
                Mathf.Clamp(child.position.y, transform.position.y - delta, transform.position.y + delta),
                Mathf.Clamp(child.position.z, transform.position.z - delta, transform.position.z + delta)
            );

            child.localScale = Vector3.one / size;

            if (child.GetComponent<ChaosObject>().ObjectType.MinSizeToPickup > size) {
               ReleaseObject(child.gameObject);
            }
        }

        size = Mathf.Min(size, 3f);
        transform.localScale = new Vector3(size, size, size);
        float fov = Utils.MapIntoRange(size, 1f, 3f, 60f, 90f);
        Camera.fieldOfView = fov;
        
        // print(size);

        if (Inputs.Player.Jump.IsPressed() || LegacyInput.JumpPressed)
        {
            Charge += ChargePerSecond * Time.deltaTime;
            Charge = Mathf.Clamp(Charge, 0f, 1f);
            audioSource.enabled = true;
        }
        else {
            audioSource.enabled = false;
        }
        
        if (Inputs.Player.Jump.WasReleasedThisFrame() || LegacyInput.JumpWasReleasedThisFrame)
        {
            var forceDir = CalcMoveDirection().normalized * Charge * Force;
            Rigidbody.AddForceAtPosition(forceDir, transform.position);
            Charge = 0f;
            Audio.PlayAt("jump", transform.position);
        }

        // on mouse wheel up increase selected item index
        if (LegacyInput.MouseWheelUp || Inputs.Player.InventoryCycleLeft.triggered) selectedItemIndex++;
        if (LegacyInput.MouseWheelDown || Inputs.Player.InventoryCycleRight.triggered) selectedItemIndex--;
        // loop the selected item index as an int
        selectedItemIndex = (int)Mathf.Repeat(selectedItemIndex, inventoryCount);      
        
        GameObject selectedInventoryItem = null;
        var count = inventory.childCount;
        if (count > 0) {
            // get the first child of the inventory
            selectedInventoryItem = inventory.GetChild(selectedItemIndex).gameObject;
            
        }

        JellyUI.Instance.SetSize(Size);
        
        if (selectedInventoryItem != null)  {
            string rawName = selectedInventoryItem.GetComponent<ChaosObject>().ObjectType.PrettyName;
            // take rawname and remove everything after the space or after a bracket
            string actualName = $"#{selectedItemIndex+1} selected: {rawName}";//.Split('(')[0].Trim()}";
            //string actualName = rawName.Substring(0, rawName.IndexOf(" "));
            JellyUI.Instance.SetSelectedText(actualName);
        }
        else {
            JellyUI.Instance.SetSelectedText("");
        } 

        if (Inputs.Player.Release.triggered || LegacyInput.ReleaseTriggered)
        {
            if (selectedInventoryItem != null) {
                ReleaseObject(selectedInventoryItem);
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
        var forward = Camera.transform.forward;
        var f = move.magnitude;
        dir = Vector3.Lerp(forward, dir, 0f);
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
        go.GetComponent<ChaosObject>().PickUp();
        go.GetComponent<Rigidbody>().isKinematic = true;
        go.GetComponent<Collider>().enabled = false;
        go.transform.parent = inventory;
        go.transform.position = inventory.position + Random.insideUnitSphere * transform.localScale.x * 0.5f;

        foreach (var it in go.GetComponentsInChildren<DestroyOnPickup>())
        {
            Destroy(it.gameObject);
        }
    }

    void ReleaseObject(GameObject go)
    {
        go.GetComponent<ChaosObject>().Release();
        go.GetComponent<Rigidbody>().isKinematic = false;
        go.GetComponent<Collider>().enabled = true;
        go.transform.SetParent(null);
        go.transform.localScale = Vector3.one;
        // add a force in the camera direction

        go.transform.position = transform.position + Vector3.ProjectOnPlane(Camera.transform.forward, Vector3.up).normalized / 1.5f + Vector3.up / 1.5f; //new Vector3(0f, 1.25f, 0f) + transform.forward;
        var forceDir = Camera.transform.forward.normalized * 100f;
        go.GetComponent<Rigidbody>().AddForce(forceDir);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        ChaosObject co = go.GetComponent<ChaosObject>(); 
        if (co != null && co.CanBePickedUpBy(size)) {
            CaptureObject(go);
        }

        //Collider[] others = Physics.OverlapSphere(transform.position, size + 1f);
        //foreach (var other in others) {
        //    if (other.transform.CompareTag("slime")) return;
        //}

        if (go.GetComponentInChildren<CanBeSlimed>() != null) {
            var p = collision.contacts[0];
            var q = Quaternion.FromToRotation(Vector3.up, p.normal);
            var decal = Instantiate(DecalSlime, p.point + p.normal.normalized * 0.01f, q);
            // scale the decal to the size of the player
            float s = Random.Range(0.5f, 1.25f);
            decal.transform.localScale = transform.localScale + new Vector3(s, s, s);
            if (go.GetComponentInChildren<ChaosObject>() != null) decal.transform.localScale /= 2f;
            // rotate the decal around the y axis randomly
            decal.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
            decal.transform.parent = go.transform;
            Audio.PlayAt("slime", transform.position);
        }
    }
}
