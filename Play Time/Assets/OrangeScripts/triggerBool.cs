using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerBool : MonoBehaviour
{
    public bool isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("orangePlayer"))
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("orangePlayer"))
        {
            isActive = false;
        }
    }
}
