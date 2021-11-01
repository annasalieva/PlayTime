using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Transform Orange;
    public Transform Lemon;

    public bool followingOrange;
    public bool followingLemon;

    public bool allowFollow;

    public float followingOrangeSpeed;
    public float followingLemonSpeed;


    // Start is called before the first frame update
    void Start()
    {
        followingOrange = true;
        followingLemon = false;

        allowFollow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            allowFollow = !allowFollow;
        }

        if(followingOrange && allowFollow)
        {
            Vector3 moveDirection = (Orange.position - Lemon.position);
            moveDirection.Set(moveDirection.x, 0, moveDirection.z);

            if (moveDirection.magnitude > 2.0f)
            {
                float step = followingOrangeSpeed * Time.deltaTime;
                moveDirection = moveDirection.normalized;
                Lemon.GetComponent<CharacterController>().Move(moveDirection * step);
            }
        }

        if(followingLemon && allowFollow)
        {
            Vector3 moveDirection = (Lemon.position - Orange.position);
            moveDirection.Set(moveDirection.x, 0, moveDirection.z);

            if (moveDirection.magnitude > 2.0f)
            {
                float step = followingLemonSpeed * Time.deltaTime;
                moveDirection = moveDirection.normalized;
                Orange.GetComponent<CharacterController>().Move(moveDirection * step);
            }
        }
    }
}
