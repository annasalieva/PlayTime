using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHands : MonoBehaviour
{
    public float steps;
    public float time;
    private Transform pos1;
    private Transform pos2;
    private Transform targetPos;
    // Start is called before the first frame update
    void Start()
    {
        pos1 = this.transform;
        pos2 = this.transform;
        pos2.position+= new Vector3(0,0,9);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
