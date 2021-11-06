using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enemy hit something");
        if(other.tag == "Player")
        {
            //Debug.Log("Player entered hit zone");
            Hit(other);
        }
    }

    private void Hit(Collider player)
    {
        player.gameObject.GetComponent<PlayerKill>().Kill();
    }
}