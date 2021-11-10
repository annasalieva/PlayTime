using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKill : MonoBehaviour
{
    public Transform respawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Kill()
    {
        //Destroy(gameObject);
        gameObject.transform.position = respawnPosition.position;
    }
}
