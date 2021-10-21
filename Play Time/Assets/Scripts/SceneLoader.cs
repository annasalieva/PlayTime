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
}
