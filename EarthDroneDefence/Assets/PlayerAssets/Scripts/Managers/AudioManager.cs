using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] public Sounds[] sounds;

    // Start is called before the first frame update
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

        #region CREATE AUDIO SOURCES
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;

        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region PLAY LOGIC TO BE CALLED FROM WHERE NEEDED
    public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log(name + " Audio clip was not found");
            return;
        }
        s.source.Play();
    }
    #endregion

    #region STOP PLAY LOGIC TO BE CALLED FROM WHERE NEEDED
    public void Stop(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log(name + " Audio clip was not found");
            return;
        }
        s.source.Stop();
    }
    #endregion

}
