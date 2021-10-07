using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        else if (other.transform.CompareTag("Finish Ladder"))
        {
            other.enabled = false;

            PlayerController.instance.ReleaseHeelForFinish(other.transform.position.y);
        }
        else if (other.transform.CompareTag("Finish Cube"))
        {
            other.GetComponent<MeshRenderer>().material.DOColor(Color.green, .5f);
        }
        else if (other.transform.CompareTag("Finish"))
        {
            // GAME Finished
            PlayerController.instance.playerCanMove = false;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.DOMoveZ(transform.position.z - .5f, .2f);
            PlayerController.instance.animator.SetTrigger("Victory");
            StartCoroutine(InGameUI.instance.levelComplete());
        }
    }
}
