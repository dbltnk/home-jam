using UnityEngine;

[RequireComponent(typeof(ChaosObject))]
public class Magnetic : MonoBehaviour
{
    private ChaosObject chaosObject;
    private JellyController jellyController;
    
    public float AttractionDistance = 2.5f;
    public float AttractionForce = 2f;

    private void Awake()
    {
        chaosObject = GetComponent<ChaosObject>();
        jellyController = FindObjectOfType<JellyController>();
    }

    void FixedUpdate()
    {
        if (!chaosObject.Carried) {
            Vector3 dirToJelly = (jellyController.transform.position - chaosObject.transform.position);       
            float distanceToJelly = dirToJelly.magnitude;
            if (distanceToJelly < AttractionDistance) chaosObject.GetComponent<Rigidbody>().AddForce(dirToJelly * AttractionForce * Time.deltaTime);
        }
    }
}
