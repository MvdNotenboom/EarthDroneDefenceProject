using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private AudioManager s_AudioManager;
    [SerializeField] private ControllerControl s_controllerControl;
    [SerializeField] public LevelManager s_levelmanager;

    [SerializeField] private bool inLargeMenu = true;
    [SerializeField] private bool audiobool = true;

    [SerializeField] public string rightDrone = "Hand";
    [SerializeField] public string leftDrone = "Hand"; 
    
    //Changed state bool
    [SerializeField] public bool isGameover;
    [SerializeField] public GameObject startButton;
    
    public GameObject target1;
    public GameObject target2;
    public GameObject droneSelectUI;
    public GameObject gameOverSelectUI;
    public GameObject introUI;
    public GameObject gameoverUI;

    AsyncOperation asyncLoad;
    [SerializeField] string newScene;

    public int  droneCounting;
    
    public bool changedScene = false;

    [SerializeField] public GameObject videoScreen;
    [SerializeField] public GameObject videoScreenoutro;
    private float videoLength = 65f;
    private float videoLengthoutro = 20f;
    private float videoStartDelay = 2f;

    private float timeSurvived;
    private int dronesDestroyed = 0;
    [SerializeField] public TextMeshProUGUI textTimeSurvived;
    [SerializeField] public TextMeshProUGUI textDronesDestroyed;

    void Start()
    {
        
        #region DO NOT DESTROY ON LOAD
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        #endregion   
        s_AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        s_controllerControl = GameObject.Find("XR Rig").GetComponent<ControllerControl>();
        isGameover = false;
        setup();
    }
    

    private void FixedUpdate()
    {
        if (!isGameover)
        {
            
            if (audiobool)
            {
                audiobool = false;
            }
            if (droneCounting <= 0 && !inLargeMenu)
            {
                
                GameOver();
            }
        }
        else
        {
              
            if (audiobool)
            {
                audiobool = false;
            }
        }
      
        if (changedScene)
        {
            changedScene = false;
        }

       
    }

    public void setup()
    {
        if(s_controllerControl == null)
        {
            s_controllerControl = GameObject.Find("XR Rig").GetComponent<ControllerControl>();
        }
        if (gameOverSelectUI == null)
        {
            gameOverSelectUI = GameObject.Find("gameOverSelectUI");

        }
        if (droneSelectUI == null)
        {
            droneSelectUI = GameObject.Find("droneSelectUI");

        }
        if(videoScreen == null)
        {
            videoScreen = GameObject.Find("VideoScreen");
        }
        if (videoScreenoutro == null)
        {
            videoScreenoutro = GameObject.Find("VideoScreenOutro");
        }
        if (startButton == null)
        {
            startButton = GameObject.Find("VideoUI");
        }
        if (textTimeSurvived == null)
        {
            textTimeSurvived = GameObject.Find("TextTimeSurvived").GetComponent<TextMeshProUGUI>();
        }
        if (textDronesDestroyed == null)
        {
            textDronesDestroyed = GameObject.Find("TextDronesDestroyed").GetComponent<TextMeshProUGUI>();
        }

        audiobool = true;
        leftDrone = "Hand";
        rightDrone = "Hand";
        s_controllerControl.SetupDrones();
        s_controllerControl.setupTheMainMenu(true);

        if (!isGameover)
        {
            videoScreenoutro.SetActive(false);
            gameOverSelectUI.SetActive(false);
            droneSelectUI.SetActive(true);
            Invoke("VideoStart", videoStartDelay); 
            startButton.SetActive(true);
        }
        else
        {
            videoScreen.SetActive(false);
            videoScreenoutro.SetActive(true);
            Invoke("VideoStartoutro", videoStartDelay);
            droneSelectUI.SetActive(false);
            startButton.SetActive(false);
            videoScreen.SetActive(false);
            textTimeSurvived.text = timeSurvived.ToString("0.00");
            textDronesDestroyed.text = dronesDestroyed.ToString();
        }
    }

    public void VideoStart()
    {
        videoScreen.SetActive(true);
        Invoke("VideoDone", videoLength);
    }

   public void VideoDone()
    {
        videoScreen.SetActive(false);

    }

    public void VideoStartoutro()
    {
        videoScreenoutro.SetActive(true);
        Invoke("VideoDoneoutro", videoLengthoutro);
    }

    public void VideoDoneoutro()
    {
        videoScreenoutro.SetActive(false);
    }

    public void updatedronecount()
    {
        droneCounting = 0;
        if (rightDrone.Equals("Pavise") || rightDrone.Equals("Rapier") || rightDrone.Equals("Cutlass") || rightDrone.Equals("Mace")  )
        {
            droneCounting++;
        }
        if (leftDrone.Equals("Pavise") || leftDrone.Equals("Rapier") || leftDrone.Equals("Cutlass") || leftDrone.Equals("Mace"))
        {
            droneCounting++;
        }
    }

    public void SwitchScene(string value)
    {
        newScene = value;
        asyncLoad = SceneManager.LoadSceneAsync(newScene);
        asyncLoad.allowSceneActivation = false;
        Invoke("SetActiveScene", 0.5f);
    }
    
    void SetActiveScene()
    {
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            Invoke("SetActiveScene", 0.5f);
            return;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));
        changedScene = true;
        setup();
    }
    
    public void DroneDestroyed()
    {
        droneCounting--;
    }

    public void SetDrone(string value)
    {
        s_controllerControl.SetDrone(value);
    }

    public bool getinLargeMenuBool()
    {
        return inLargeMenu;
    }
    public void setinLargeMenuBool(bool value)
    {
        inLargeMenu = value;
    }

    public void setAudioBool(bool value)
    {
        audiobool = value;
    }

    public void BackgroundAudio()
    {
        s_AudioManager.Play("background");
    }

    public void GameOver()
    {
        timeSurvived = s_levelmanager.GetTime();
        inLargeMenu = true;
        SwitchScene("Start_Menu");
        isGameover = true;
    }

    public void setGameover(bool value)
    {
        isGameover = value;
    }

    public void AddDroneDestroyed()
    {
        dronesDestroyed++;
    }

    public void ResetDroneDestroyed()
    {
        dronesDestroyed = 0;
    }

    public void GetLevelManager()
    {
        s_levelmanager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

}
