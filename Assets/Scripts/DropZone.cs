using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public bool ObjectHasBeenDelivered;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<DeliverObject>() != null) {
            ObjectHasBeenDelivered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<DeliverObject>() != null) {
            ObjectHasBeenDelivered = false;
        }
    }
}
