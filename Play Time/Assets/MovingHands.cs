using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHands : MonoBehaviour
{
    public float steps;
    private Transform pos1;
    private Transform pos2;
    private Transform targetPos;
    public bool buttonon = false;
    // Start is called before the first frame update
    void Start()
    {
        pos1 = transform;
        pos2 = transform;
        pos2.position+= new Vector3(0,0,-9);
        targetPos = pos2;
    }
    // Update is called once per frame
    void Update()
    {
        if (buttonon){
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, steps*Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos.position) < 0.001f)
            {
                if (targetPos == pos2)
                {
                    wait();
                    targetPos = pos1;
                }
                else
                {
                    wait();
                    targetPos = pos2;
                }
            }
        }
        
    }

    IEnumerator wait(){
        yield return new WaitForSeconds(5);
    }

}