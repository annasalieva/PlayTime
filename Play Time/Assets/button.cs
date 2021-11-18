using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (Input.GetKey("e"))
            {
                Debug.Log("Button on");
                GameObject.FindGameObjectWithTag("mplatform").GetComponent<MoveBlock>().buttonon = true;
            }
        }
    }
}
