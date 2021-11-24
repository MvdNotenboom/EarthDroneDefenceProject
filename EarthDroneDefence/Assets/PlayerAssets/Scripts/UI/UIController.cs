using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameManager s_gameManager;
    private AudioManager s_AudioManager;

    [SerializeField] private Button buttontest;

    AsyncOperation asyncLoad;

    void Start()
    {
        s_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        s_AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        asyncLoad = SceneManager.LoadSceneAsync("LoadingScene");
        asyncLoad.allowSceneActivation = false;
    }

    public void ButtonTest()
    {
        s_gameManager.setinLargeMenuBool(false);
        SceneManager.LoadScene("LoadingScene");
        s_gameManager.changedScene = true;
        s_gameManager.isGameover = false;
    }
    
    public void PaviseDroneButton() //Currently not enabled in the current state of the game.
    {
        s_gameManager.SetDrone("Pavise");
    }
    public void RapierDroneButton()
    {
        s_AudioManager.Play("rapier");
        s_gameManager.SetDrone("Rapier");
    }
    public void CutlassDroneButton()
    {
        s_AudioManager.Play("cutlass");
        s_gameManager.SetDrone("Cutlass");
    }
    public void MaceDroneButton()
    {
        s_AudioManager.Play("mace");
        s_gameManager.SetDrone("Mace");
    }
    public void HandButton()
    {
        s_gameManager.SetDrone("Hand");
    }

    public void RestartButton()
    {
        s_gameManager.setGameover(false);
        s_gameManager.setup();
    }

    public void Quit()
    {
        Application.Quit();
    }

}
