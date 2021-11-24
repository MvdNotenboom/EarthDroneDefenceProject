using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour
{
    [SerializeField] private AudioManager s_audiomanager;

    [SerializeField] private GameObject particle;
    [SerializeField] private GameObject testTarget;

    private float respawnTimer = 5f;
    [SerializeField] private int health = 3;
    private bool respawning = false;

    void Start()
    {
        s_audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void FixedUpdate()
    {
        if(health <= 0 && !respawning)
        {
            ResetTaget();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        s_audiomanager.Play("dummyhit");
        health = health - collision.gameObject.GetComponent<Projectile>().dammage;
    }

    private void OnTriggerEnter(Collider other)
    {
        ResetTaget();
    }

    public void ResetTaget()
    {
        respawning = true;
        testTarget.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        Invoke("RespawnTarget", respawnTimer);
    }

    public void RespawnTarget()
    {
        health = 3;
        respawning = false;
        testTarget.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
    }
}
