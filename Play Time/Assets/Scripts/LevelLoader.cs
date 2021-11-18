using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string NameOfSceneToLoad;
    public Animator transition;

    public float TransitionTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if() //figure out how to trigger the transition
        {
            LoadnextLevel();
        }*/
    }

    public void LoadnextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        //play animation
        transition.SetTrigger("Start");
        //wait
        yield return new WaitForSeconds(TransitionTime);
        //load Scene
        SceneManager.LoadScene(NameOfSceneToLoad);
    }
}
