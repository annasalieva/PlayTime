using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void loadLemonScene()
    {
        SceneManager.LoadScene("LemonTest");
    }

    public void loadOrangeScene()
    {
        SceneManager.LoadScene("OrangeTest");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reloadCurrentScene();
        }
    }
    public void reloadCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
