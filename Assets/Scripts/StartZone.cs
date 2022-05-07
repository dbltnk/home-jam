using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    public bool PlayerIsInStartZone;

    void OnTriggerEnter(Collider other)
     {
        if (other.gameObject.GetComponent<JellyController>() != null) {
            PlayerIsInStartZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<JellyController>() != null) {
            PlayerIsInStartZone = false;
        }
    }
}