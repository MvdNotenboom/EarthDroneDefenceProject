using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float destroySelfTimer = 5f;
    [SerializeField] public int dammage = 1;
    [SerializeField] public ParticleSystem hitParticle;
    //[SerializeField] private AudioManager s_audiomanager;

    void Start()
    {
        //s_audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Destroy(gameObject, destroySelfTimer);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ProjectilePlayer"))
        {
            //This is here to enable bullet on bullet ricochets
        }
        else
        {
            DestroySelf();
        }
    }

}
