using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPriority : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera priority;
    [SerializeField]
    private CinemachineVirtualCamera[] nonPriority;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            priority.Priority = 1;
            other.gameObject.GetComponent<NewPlayerMovement>().setCamera(priority);
            foreach(CinemachineVirtualCamera cam in nonPriority){
                cam.Priority = 0;
            }
        }
    }
}
