using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallCheck : MonoBehaviour
{
    // private void OnTriggerEnter(Collider other)
    // {
    //     if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //     {
    //         Debug.Log(other.gameObject.layer);
    //         gameObject.tag = "grabbable";
    //     }
    // }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Debug.Log(other.gameObject.layer);
            gameObject.tag = "grabbable";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            gameObject.tag = "Untagged";
        }
    }
}
