using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{

    [SerializeField] private float timeToSelfdestruct;

    void Start()
    {
        Invoke("SelfDestruct", timeToSelfdestruct);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
