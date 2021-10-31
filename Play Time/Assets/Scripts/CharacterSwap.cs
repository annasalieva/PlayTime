using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterSwap : MonoBehaviour
{
    public Transform character;
    public List<Transform> characterList;
    public int characterIndex;

    public CinemachineVirtualCamera cam;
    public ParticleSystem particles;

    void Start()
    {
        if(character == null && characterList.Count >= 1)
        {
            character = characterList[0];
        }
        SwapCharacters();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(characterIndex == 0)
            {
                characterIndex = characterList.Count - 1;
            }
            else
            {
                characterIndex -= 1;
            }
            SwapCharacters();
        }

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (characterIndex == characterList.Count - 1)
        //    {
        //        characterIndex = 0;
        //    }
        //    else
        //    {
        //        characterIndex += 1;
        //    }
        //    SwapCharacters();
        //}
    }

    public void SwapCharacters()
    {
        character = characterList[characterIndex];
        //character.GetComponent<CharacterController>().enabled = true;

        particles.transform.position = character.position;
        particles.Play();

        //disable movement for all other characters
        //for(int i = 0; i < characterList.Count; i++)
        //{
        //    if(characterList[i] != character)
        //    {
        //        characterList[i].GetComponent<CharacterController>().enabled = false;
        //    }
        //}
        cam.LookAt = character;
        cam.Follow = character;

        //Orange following Lemon
        if(character.gameObject.name == "LemonRigged")
        {
            GetComponent<FollowCharacter>().followingLemon = true;
            GetComponent<FollowCharacter>().followingOrange = false;

            characterList[0].GetComponent<orangeMovement>().allowOrangeMovement = false;
            characterList[1].GetComponent<PlayerMovement>().allowLemonMovement = true;
        }

        //Lemon following Orange
        if(character.gameObject.name == "OrangePrefab")
        {
            GetComponent<FollowCharacter>().followingOrange = true;
            GetComponent<FollowCharacter>().followingLemon = false;

            characterList[0].GetComponent<orangeMovement>().allowOrangeMovement = true;
            characterList[1].GetComponent<PlayerMovement>().allowLemonMovement = false;
        }
    }
}
