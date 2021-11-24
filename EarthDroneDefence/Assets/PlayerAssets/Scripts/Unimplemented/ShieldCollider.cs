using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
    [SerializeField] private GameObject hitParticle;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(hitParticle, transform.position, transform.rotation);
    }
}
