using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCharacter_swap : MonoBehaviour
{
    private bool inrange = false;
    public GameObject SwapTutorial;
    void start()
    {
        SwapTutorial.gameObject.text.enabled = false;
    }

    void update()
    {
        if(inrange && Input.GetKeyDown("Q"))
        {
            SceneManager.LoadScene("OrangeTest");
        }
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.gameObject.GetComponent<PlayerMovement>())
        {
            other.gameObject.GetComponent<PlayerMovement>().allowLemonMovement = false;
            other.gameObject.velocity = Vector3.zero;
            SwapTutorial.gameObject.text.enabled = true;
        }
        inrange = true;
    }
}
