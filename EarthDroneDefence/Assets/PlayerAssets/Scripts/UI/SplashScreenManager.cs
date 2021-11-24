using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    AsyncOperation asyncLoad;

    void Start()
    {
        asyncLoad = SceneManager.LoadSceneAsync("Start_Menu");
        asyncLoad.allowSceneActivation = false;
        Invoke("SetActiveScene", 5f);
    }

    void SetActiveScene()
    {
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            Invoke("SetActiveScene", 1f);
            return;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Start_Menu"));
    }

}
