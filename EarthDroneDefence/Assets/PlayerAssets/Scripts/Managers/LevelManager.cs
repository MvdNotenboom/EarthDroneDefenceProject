using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameManager s_gamemanager;
    [SerializeField] private SpawnManager s_SpawnManager;
    [SerializeField] private AudioManager s_audiomanager;

    [SerializeField] private byte currentLevelState = STARTUP;
    private const byte STARTUP = 0;
    private const byte FIRST = 1;
    private const byte SECOND = 2;
    private const byte THIRD = 3;
    private const byte FOURTH = 4;
    private const byte FIFTH = 5;
    private const byte CONCURRENT = 6;

    private Coroutine firstCoroutine;
    private float FIRSTspawntime = 8f;
    private float SECONDspawntime = 7f;
    private float THIRDspawntime = 6f;
    private float FOURTHspawntime = 5f;
    private float FIFTHspawntime = 4f;
    private float CONCURRENTspawntime = 2f;

    //spawnvariation values
    int health = 1;
    int dammage = 1;
    int defectivespawn = 33;
    int defectivedeath = 35;
    float speed = 3f;
    float idletime = 3f;
    float vprojectilespeed = 10f;
    float vshootingdelay = 2.5f;

    [SerializeField] private float timer = 0f;

    private float first = 10;
    private float second = 50;
    private float third = 90;
    private float fourth = 140;
    private float fifth = 190;
    private float concurrent = 250;

    private bool waveoneaudio = true;

    void Start()
    {
        s_gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        s_audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        timer = 0.0f;
        s_gamemanager.ResetDroneDestroyed();
        firstCoroutine = StartCoroutine(FIRSTmovement());
        s_gamemanager.GetLevelManager();
        s_audiomanager.Play("wave0");
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        switch (currentLevelState)
        {
            case STARTUP:
                break;
            case FIRST:
                if (waveoneaudio)
                {
                    s_audiomanager.Play("wave1");
                    waveoneaudio = false;
                }
                break;
            case SECOND:
                if(health != 2)
                {
                    s_audiomanager.Play("wave2");
                    health = 2;
                    dammage = 1;
                    defectivespawn = 30;
                    defectivedeath = 40;
                    speed = 4f;
                    idletime = 2f;
                    vprojectilespeed = 10f;
                    vshootingdelay = 2.0f;
                }
                break;
            case THIRD:
                if (health != 3)
                {
                    s_audiomanager.Play("wave3");
                    health = 3;
                    dammage = 2;
                    defectivespawn = 27;
                    defectivedeath = 45;
                    speed = 4f;
                    idletime = 2f;
                    vprojectilespeed = 11f;
                    vshootingdelay = 2.0f;
                }
                break;
            case FOURTH:
                if (health != 4)
                {
                    s_audiomanager.Play("wave4");
                    health = 4;
                    dammage = 2;
                    defectivespawn = 24;
                    defectivedeath = 50;
                    speed = 4f;
                    idletime = 1f;
                    vprojectilespeed = 11f;
                    vshootingdelay = 1.5f;
                }
                break;
            case FIFTH:
                if (health != 5)
                {
                    s_audiomanager.Play("wave5");
                    health = 5;
                    dammage = 3;
                    defectivespawn = 21;
                    defectivedeath = 55;
                    speed = 5f;
                    idletime = 1f;
                    vprojectilespeed = 12f;
                    vshootingdelay = 1.0f;
                }
                break;
            case CONCURRENT:
                if (health != 6)
                {
                    s_audiomanager.Play("waveC");
                    health = 6;
                    dammage = 3;
                    defectivespawn = 15;
                    defectivedeath = 60;
                    speed = 5f;
                    idletime = 0f;
                    vprojectilespeed = 13f;
                    vshootingdelay = 0.5f;
                }
                break;
            default:
                Debug.Log("Error in LevelManagerState");
                break;
        }

        if(timer < first)
        {
            currentLevelState = STARTUP;
        }
        else if(timer > first && timer < second)
        {
            currentLevelState = FIRST;
        }
        else if (timer > second && timer < third)
        {
            currentLevelState = SECOND;
        }
        else if (timer > third && timer < fourth)
        {
            currentLevelState = THIRD;
        }
        else if (timer > fourth && timer < fifth)
        {
            currentLevelState = FOURTH;
        }
        else if (timer > fifth && timer < concurrent)
        {
            currentLevelState = FIFTH;
        }
        else if (timer > concurrent)
        {
            currentLevelState = CONCURRENT;
        }

    }

    public void SpawnEnemy()
    {
        s_SpawnManager.SpawnEnemy();
    }

    public void SpawnEnemyVariation(int vhealth, int vdammage, int vdefectivespawn, int vdefectivedeath, float vspeed, float vidletime, float vprojectilespeed, float vshootingdelay)
    {
        s_SpawnManager.SpawnEnemyVar(vhealth, vdammage, vdefectivespawn, vdefectivedeath, vspeed, vidletime, vprojectilespeed, vshootingdelay);
    }

    IEnumerator FIRSTmovement()
    {
        
        while (currentLevelState == STARTUP)
        {
            yield return new WaitForSecondsRealtime(FIRSTspawntime);
        }
        while (currentLevelState == FIRST)
        {
            SpawnEnemy();
            yield return new WaitForSecondsRealtime(FIRSTspawntime);
        }
        while (currentLevelState == SECOND)
        {
            SpawnEnemyVariation(health, dammage, defectivespawn, defectivedeath, speed, idletime, vprojectilespeed, vshootingdelay);
            yield return new WaitForSecondsRealtime(SECONDspawntime);
        }
        while (currentLevelState == THIRD)
        {
            SpawnEnemyVariation(health, dammage, defectivespawn, defectivedeath, speed, idletime, vprojectilespeed, vshootingdelay);
            yield return new WaitForSecondsRealtime(THIRDspawntime);
        }
        while (currentLevelState == FOURTH)
        {
            SpawnEnemyVariation(health, dammage, defectivespawn, defectivedeath, speed, idletime, vprojectilespeed, vshootingdelay);
            yield return new WaitForSecondsRealtime(FOURTHspawntime);
        }
        while (currentLevelState == FIFTH)
        {
            SpawnEnemyVariation(health, dammage, defectivespawn, defectivedeath, speed, idletime, vprojectilespeed, vshootingdelay);
            yield return new WaitForSecondsRealtime(FIFTHspawntime);
        }
        while (currentLevelState == CONCURRENT)
        {
            SpawnEnemyVariation(health, dammage, defectivespawn, defectivedeath, speed, idletime, vprojectilespeed, vshootingdelay);
            yield return new WaitForSecondsRealtime(CONCURRENTspawntime);
        }

        Debug.Log("Error in LevelManager Coroutine");
        yield return null;
    }

    public float GetTime()
    {
        return timer;
    }
}
