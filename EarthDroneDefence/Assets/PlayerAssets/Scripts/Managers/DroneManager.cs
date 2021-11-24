using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public static DroneManager instance;
    [SerializeField] public Drones[] drones;

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

    }

    public GameObject GetDrone(string name)
    {
        Drones d = Array.Find(drones, drone => drone.name == name);
        if (d == null)
        {
            Debug.Log(name + " Drone was not found");
            return null;
        }
        return d.droneObject;
    }

}
