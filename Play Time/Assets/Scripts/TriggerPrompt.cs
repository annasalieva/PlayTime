using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerPrompt : MonoBehaviour
{
    public GameObject prompt;

    void Start()
    {
        prompt.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            prompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            prompt.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
