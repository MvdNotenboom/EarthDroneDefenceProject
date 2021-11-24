using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandController : MonoBehaviour
{
    [SerializeField] private GameManager s_gameManager;
    [SerializeField] private DroneManager s_droneManager;
    [SerializeField] private DroneControl s_droneControl;

    [SerializeField] private XRInteractorLineVisual lineVisual;
    [SerializeField] private int lineLenght = 0;

    [SerializeField] private bool gripDown = false;
    [SerializeField] private bool triggerDown = false;
    [SerializeField] private bool primaryDown = false;

    [SerializeField] private bool inMainMenu;
    [SerializeField] private bool hasDrone = false;

    [SerializeField] private GameObject playerHand;
    [SerializeField] private GameObject heldDrone;

    //Shooting is moved to the droneControl //Kept here to avoid unforseen issues that could take too long to debug/fix due to time contraints
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform ProjectileSpawn;
    [SerializeField] private GameObject Projectile_0;
    [SerializeField] private float Velocity_Projectile_0 = 25f;
    [SerializeField] private bool shootOnCooldown = false;
    [SerializeField] private float shootCooldownTime;

    //Missile Shooting is moved to the droneControl //Kept here to avoid unforseen issues that could take too long to debug/fix due to time contraints
    [SerializeField] private GameObject missile;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private float velocity_Missile = 15f;
    [SerializeField] private bool missleUnderway = false;

    private void Awake()
    {
        s_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        s_droneManager = GameObject.Find("DroneManager").GetComponent<DroneManager>();
    }

    void Start()
    {
        
        lineVisual = GetComponent<XRInteractorLineVisual>();
        inMainMenu = s_gameManager.getinLargeMenuBool();
    }

    void Update()
    {
        if (triggerDown && hasDrone)
        {
            s_droneControl.Shoot();
        }

        if (gripDown && !missleUnderway)
        {
            s_droneControl.ShootMissile();
            missleUnderway = true;
        }
        if (!gripDown && missleUnderway)
        {
            s_droneControl.DetonateMissile();
            missleUnderway = false;
        }
        
    }
    private void FixedUpdate()
    {
        if (inMainMenu)
        {
            if (lineLenght != 0 && !primaryDown)
            {
                lineVisual.lineLength = 0;
                lineLenght = 0;
            }
            else if (lineLenght != 30 && primaryDown)
            {
                lineVisual.lineLength = 30;
                lineLenght = 30;
            }
        }
    }

    public void SetDrone(string value)
    {
        
        if(heldDrone != null)
        {
            Destroy(heldDrone);
            heldDrone = null;
        }
        if (value.Equals("Hand"))
        {
            hasDrone = false;
            playerHand.SetActive(true);
            heldDrone = s_droneManager.GetDrone(value);
        }
        else if (value.Equals(""))
        {
            if (heldDrone != null)
            {
                heldDrone = null;
            }
            hasDrone = false;
            playerHand.SetActive(true);
        }
        else {
            hasDrone = true;
            playerHand.SetActive(false);
            heldDrone = s_droneManager.GetDrone(value);
            heldDrone = Instantiate(s_droneManager.GetDrone(value), gameObject.transform);
            s_droneControl = heldDrone.GetComponent<DroneControl>();
            
        }
    }


    public void SetinMainMenu(bool value)
    {
        inMainMenu = true;
    }


    public void ShootProjectile_0()
    {
        projectile = Instantiate(Projectile_0, ProjectileSpawn.position, ProjectileSpawn.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(ProjectileSpawn.forward * Velocity_Projectile_0, ForceMode.Impulse);
        shootOnCooldown = true;
        Invoke("ShootingCooldownDone", shootCooldownTime);
    }

    public void ShootingCooldownDone()
    {
        shootOnCooldown = false;
    }

    public void ShootMissile()
    {
        missilePrefab = Instantiate(missile, ProjectileSpawn.position, ProjectileSpawn.rotation);
        missilePrefab.GetComponent<Rigidbody>().AddForce(ProjectileSpawn.forward * velocity_Missile, ForceMode.Impulse);

    }
    public void DetonateMissile()
    {
        missilePrefab.GetComponent<PlayerMissile>().DestroySelf();
    }

    #region SET CONTROLLER INPUTS FROM ControllerControl
    public void setGrip(bool value)
    {
        gripDown = value;
    }
    public void setTrigger(bool value)
    {
        triggerDown = value;
    }
    public void setPrimary(bool value)
    {
        primaryDown = value;
    }
    #endregion

}
