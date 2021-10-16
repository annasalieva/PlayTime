using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbableObject : MonoBehaviour
{
    [SerializeField] GameObject Ntrigger;
    [SerializeField] GameObject Etrigger;
    [SerializeField] GameObject Strigger;
    [SerializeField] GameObject Wtrigger;

    bool NActive;
    bool EActive;
    bool SActive;
    bool WActive;

    private Vector3 moveDirection;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        NActive = Ntrigger.GetComponent<triggerBool>().isActive;
        EActive = Etrigger.GetComponent<triggerBool>().isActive;
        SActive = Strigger.GetComponent<triggerBool>().isActive;
        WActive = Wtrigger.GetComponent<triggerBool>().isActive;

        if (SActive && Input.GetKey(KeyCode.J))
        {
            Debug.Log("isGrabbed");
            
        }
    }
}
