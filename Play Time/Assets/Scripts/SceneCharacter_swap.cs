using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using UnityEngine.SceneManagement;

public class SceneCharacter_swap : MonoBehaviour
{
    
    

    [SerializeField]
    private Canvas canvas; 
    
    //[SerializeField]
    private TMP_Text helpText; 
    private bool inrange = false; 

    
    
    
    // Start is called before the first frame update
    void Start()
    {
       helpText = canvas.GetComponentInChildren<TMP_Text>(); //gets the text from the canvas
       helpText.gameObject.SetActive(false); //turns off text
    }

    // Update is called once per frame
    void Update()
    {
        
        if(inrange && Input.GetKeyDown("q"))
        {
            SceneManager.LoadScene("OrangeTest");
        }

    }


    public void OnTriggerEnter(Collider other)
    {
        print("on button");
        if (other.tag == "Player" && other.gameObject.GetComponent<PlayerMovement>())
        {
            other.gameObject.GetComponent<PlayerMovement>().allowLemonMovement = false; //player object is other
            
            other.gameObject.GetComponent<CharacterController>().SimpleMove(Vector3.zero);

            helpText.gameObject.SetActive(true);
            
        }
        inrange = true;
    }

}
