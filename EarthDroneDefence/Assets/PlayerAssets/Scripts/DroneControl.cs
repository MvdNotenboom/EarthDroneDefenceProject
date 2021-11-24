using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControl : MonoBehaviour
{
    [SerializeField] private GameManager s_gamemanager;
    [SerializeField] private AudioManager s_audiomanager;

    [SerializeField] private bool isOffenceDrone;

    [SerializeField] private GameObject shootProjectile;
    [SerializeField] private Transform projectileSpawnLocation;
    [SerializeField] private Transform projectileSpawnLocation_1;
    private bool shootingSide = true;
    [SerializeField] private GameObject spawnedProjectile;
    [SerializeField] private float projectileSpeed = 25f;
    [SerializeField] private int projectileDammage = 1;
    [SerializeField] private bool shootOnCooldown = false;
    [SerializeField] private float shootCooldownTime;

    [SerializeField] private GameObject shootMissle;
    [SerializeField] private Transform missleSpawnLocation;
    [SerializeField] private GameObject spawnedMissle;
    [SerializeField] private bool missleUnderway = false;
    [SerializeField] private int missleStock;   //Only applicable if there is a restocking option, otherwise make use of cooldowns
    [SerializeField] private float velocity_Missle = 5f;

    [SerializeField] public int health;
    [SerializeField] private ParticleSystem hidParticle;
    [SerializeField] private ParticleSystem destroyParticle;

    void Start()
    {
        s_gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        s_audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void FixedUpdate()
    {
        if(health <= 0)
        {
            s_audiomanager.Play("death");
            Instantiate(destroyParticle, transform.position, transform.rotation);
            s_gamemanager.DroneDestroyed();
            Destroy(gameObject);
            
        }
    }

    //basic shooting
    public void Shoot()
    {
        if (isOffenceDrone && !shootOnCooldown)
        {
            if (shootingSide)
            {
                spawnedProjectile = Instantiate(shootProjectile, projectileSpawnLocation.position, projectileSpawnLocation.rotation);
                spawnedProjectile.GetComponent<Rigidbody>().AddForce(projectileSpawnLocation.forward * projectileSpeed, ForceMode.Impulse);
                spawnedProjectile.GetComponent<Projectile>().dammage = projectileDammage;
                shootOnCooldown = true;
                shootingSide = false;
                Invoke("ShootingCooldownDone", shootCooldownTime);
            }
            else
            {
                spawnedProjectile = Instantiate(shootProjectile, projectileSpawnLocation_1.position, projectileSpawnLocation_1.rotation);
                spawnedProjectile.GetComponent<Rigidbody>().AddForce(projectileSpawnLocation_1.forward * projectileSpeed, ForceMode.Impulse);
                spawnedProjectile.GetComponent<Projectile>().dammage = projectileDammage;
                shootOnCooldown = true;
                shootingSide = true;
                Invoke("ShootingCooldownDone", shootCooldownTime);
            }
            
        }
    }

    public void ShootingCooldownDone()
    {
        shootOnCooldown = false;
    }

    public void ShootMissile()
    {
        if (isOffenceDrone)
        {
            s_audiomanager.Play("missileshoot");
            spawnedMissle = Instantiate(shootMissle, missleSpawnLocation.position, missleSpawnLocation.rotation);
            spawnedMissle.GetComponent<Rigidbody>().AddForce(missleSpawnLocation.forward * velocity_Missle, ForceMode.Impulse);
        }
    }

    public void DetonateMissile()
    {
        if (isOffenceDrone)
        {
            if (spawnedMissle != null)
            {
                spawnedMissle.GetComponent<PlayerMissile>().DestroySelf();
            }
        }
    }

    public void ShootProjectile_0()
    {
        Instantiate(shootProjectile, projectileSpawnLocation.position, projectileSpawnLocation.rotation);
        shootOnCooldown = true;
        Invoke("ShootingCooldownDone", shootCooldownTime);
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("ProjectileEnemy"))
        {
            Instantiate(hidParticle, transform.position, transform.rotation);
            health--;
        } else
        if   (other.gameObject.CompareTag("Enemy")) {
            other.gameObject.GetComponent<EnemyAI>().DestroySelf();
            Instantiate(hidParticle, transform.position, transform.rotation);
            health-=3;
        } 
      
    }
}
