using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeelController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Collectable Heel"))
        {
            // collect
            Destroy(other.gameObject);
            PlayerController.instance.CollectHeel();
            
        }
        else if (other.transform.CompareTag("Obstacle"))
        {
            Debug.Log("Çarptı");
            foreach (Collider coll in other.GetComponents<BoxCollider>())
            {
                if (coll.isTrigger)
                {
                    coll.enabled = false;
                }
            }
            PlayerController.instance.ReleaseHeel(other.transform.position.y);
        }
    }
}
