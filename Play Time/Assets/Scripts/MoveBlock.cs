using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public Transform position1;
    public Transform position2;
    private float time;
    private Transform targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        targetPosition = position2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition.position) < 0.001f){
            if (targetPosition == position2){
                targetPosition = position1;
            }
            else{
                targetPosition = position2;
            }
        }
    }
}
