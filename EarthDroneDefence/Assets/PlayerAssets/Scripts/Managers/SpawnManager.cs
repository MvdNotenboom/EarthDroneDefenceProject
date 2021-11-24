using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Collider spawnAreaFront;
    [SerializeField] private GameObject testspawn;
    [SerializeField] private GameObject drone_1;
    [SerializeField] private GameObject drone_2;
    [SerializeField] private GameObject[] droneArray;
    private GameObject spawnedobject;

    public Vector3 getSpawnPointFront(Bounds bounds)
    {
        return new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z));
    }

    public void SpawnEnemy()
    {
        int variant = Random.Range(1, 3);
        Debug.Log(variant.ToString());
        Instantiate(droneArray[variant], getSpawnPointFront(spawnAreaFront.bounds), transform.rotation * Quaternion.Euler(0f, 0f, 0f));
    }
    
    public void SpawnEnemyVar(int vhealth, int vdammage, int vdefectivespawn, int vdefectivedeath, float vspeed, float vidletime, float vprojectilespeed, float vshootingdelay)
    {
        int variant = Random.Range(1, 3);
        Debug.Log(variant.ToString());
        spawnedobject = Instantiate(droneArray[variant], getSpawnPointFront(spawnAreaFront.bounds), transform.rotation * Quaternion.Euler(0f, 0f, 0f));
        spawnedobject.GetComponent<EnemyAI>().VariationAdjustment(vhealth, vdammage, vdefectivespawn, vdefectivedeath, vspeed, vidletime, vprojectilespeed, vshootingdelay);
    }
}
