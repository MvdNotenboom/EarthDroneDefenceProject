 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private AudioManager s_audiomanager;

    void Start()
    {
        s_audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Invoke("DestroySelf", 200f);
    }

    public void DestroySelf()
    {
        s_audiomanager.Play("death");
        Instantiate(explosion, transform.position, transform.rotation);
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Invoke("selfDestroy", 0.1f);
    }

    private void selfDestroy()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyAI>().UpdateHealth(Random.Range(-15, -2));
        }
        if (other.gameObject.CompareTag("Player"))
        {
            //uncomment this to enable friendly fire on the missile
            //other.gameObject.GetComponent<DroneControl>().health--;
        }
    }
    

}
