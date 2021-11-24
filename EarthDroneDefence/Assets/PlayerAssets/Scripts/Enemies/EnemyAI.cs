using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject s_HandControllerR;
    [SerializeField] private GameObject s_HandControllerL;
    [SerializeField] private AudioManager s_audiomanager;
    [SerializeField] private GameManager s_gamemanager;

    [SerializeField] private GameObject shootProjectile;
    [SerializeField] private Transform projectileSpawnLocation;
    [SerializeField] private GameObject spawnedProjectile;
    private float projectileSpeed = 10f;
    private int projectileDammage = 1;
    private float shootingDelay = 2.5f;
    private float shootingTimer = 0f;
    private bool isShooting = false;

    [SerializeField] public Transform target;
    [SerializeField] public Transform dronetorotate;
    
    public float speed;
    private Quaternion _lookrotation;
    private Vector3 _direction;

    [SerializeField] private Transform waypoint;
    [SerializeField] private Vector3 way;
    [SerializeField] private Vector3 dir;

    private float speedmove = 3f;

    private byte currentBehaviourState = WARPINGIN;
    private const byte IDLESTATE = 0;
    private const byte MOVESTATE = 1;
    private const byte GETNEWWAYPOINTSTATE = 2;
    private const byte DEFECTIVE = 3;
    private const byte DEATH = 4;
    private const byte WARPINGIN = 5;   //start state

    private bool isSwitchingState = false;

    private float timeInIdleState = 3f;
    private float timeInMoveState = 2f;

    //movement bounderies
    private int minx = -15, maxx = 15, midx = 0;
    private int miny = -5, maxy = 15, midy = 5;
    private int minz = 10, maxz = 35, midz = 20;
    private bool moveIntoBounds = false;

    private int defectiveRamleftOrRight;
    private bool destroySelfActive = false;

    [SerializeField] ParticleSystem warpingParticle;
    [SerializeField] GameObject enemy;
    [SerializeField] EnemyFacing s_Enemy;
    [SerializeField] float warpInTime = 3f;

    [SerializeField] ParticleSystem electroboom;
    [SerializeField] ParticleSystem fireboom;

    private bool haveRamLocation = false;
    private float ramTilDeathDelay = 20f;
    private int defectiveChangePercentOnWarp = 33;
    private int defectiveChangePercentOnDeath = 35;

    [SerializeField] private int health = 1;
    [SerializeField] public ParticleSystem hitParticle;
    [SerializeField] public BoxCollider hitCollider;

    private int defectiveDeath = -3;

    private bool playsound = true;

    private bool inDeathState = false;

    // Start is called before the first frame update
    void Start()
    {
        s_audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        s_gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (Random.Range(0,1) < 0.03)
        {
            timeInIdleState = 0.0f;
        }
        s_HandControllerR = GameObject.Find("RightHand Controller");
        s_HandControllerL = GameObject.Find("LeftHand Controller");
        defectiveRamleftOrRight = Random.Range(0, 2);
        s_audiomanager.Play("enemywarp");
        Invoke("WarpInEnemy", warpInTime);

    }

    private void FixedUpdate()
    {
        if (!inDeathState)
        {
            if (health <= 0)
            {
                inDeathState = true;
                if (Random.Range(1, 101) <= defectiveChangePercentOnDeath)
                {
                    currentBehaviourState = DEFECTIVE;
                }
                else
                {
                    currentBehaviourState = DEATH;
                }
            }
        }
        if(health <= defectiveDeath)
        {
            currentBehaviourState = DEATH;
        }
        

        if (isShooting)
        {
            shootingTimer += Time.deltaTime;
            if(shootingTimer >= shootingDelay)
            {
                Shoot();
            }
            
        }

        switch (currentBehaviourState)
        {

            case IDLESTATE:
                isShooting = true;
                if (isSwitchingState)
                {
                }
                else
                {
                    StartCoroutine(SwitchStateWithDelay(GETNEWWAYPOINTSTATE, timeInIdleState));
                }
                break;

            case MOVESTATE:
                MoveToNewPosition();
                if (moveIntoBounds && !isSwitchingState)
                {
                    StartCoroutine(SwitchStateWithDelay(IDLESTATE, (timeInMoveState * 2)));
                }
                else if (!isSwitchingState)
                {
                    StartCoroutine(SwitchStateWithDelay(IDLESTATE, timeInMoveState));
                }
                break;

            case GETNEWWAYPOINTSTATE:
                getWaypoint();
                currentBehaviourState = MOVESTATE;
                break;

            case DEFECTIVE:
                if(health <= defectiveDeath)
                {
                    currentBehaviourState = DEATH;
                }
                isShooting = false;
                s_Enemy.defective = true;
                RamIntoPlayer();
                if (playsound) {
                    s_audiomanager.Play("enemydefective");
                    playsound = false;
                }
                break;

            case DEATH:
                isShooting = false;
                if (!destroySelfActive)
                {
                    electroboom.gameObject.SetActive(true);
                    Invoke("DestroySelf", 0.8f);
                    destroySelfActive = true;
                }
                break;

            case WARPINGIN:
                warpingParticle.gameObject.SetActive(true);
                break;

            default:
                Debug.Log("DEFAULT STATE");
                break;

        }

    }

    public void WarpInEnemy()
    {
        
        hitCollider.enabled = true;
        enemy.SetActive(true);
        if (Random.Range(1,101) <= defectiveChangePercentOnWarp)
        {
            currentBehaviourState = DEFECTIVE;
            inDeathState = true;
        }
        else
        {
            currentBehaviourState = IDLESTATE;
        }
    }

    public void MoveToNewPosition()
    {
            dir = waypoint.position - transform.position;
            transform.Translate(dir.normalized * speedmove * Time.deltaTime);
        
    }

    public void getWaypoint()
    {
        waypoint.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) + Random.insideUnitSphere * 0.5f;
        if (waypoint.position.x < minx || waypoint.position.x > maxx || waypoint.position.y < miny || waypoint.position.y > maxy || waypoint.position.z < minz || waypoint.position.z > maxz)
        {
            moveIntoBounds = true;
            waypoint.position = new Vector3(midx, midy, midz) + Random.insideUnitSphere * 0.5f;
        }
        way = waypoint.position;
    }

    IEnumerator SwitchStateWithDelay(byte state, float delay)
    {
        isSwitchingState = true;
        yield return new WaitForSecondsRealtime(delay);
        currentBehaviourState = state;
        isSwitchingState = false;
    }
    
    public void Shoot()
    {
        s_audiomanager.Play("enemyshoot");
        spawnedProjectile = Instantiate(shootProjectile, projectileSpawnLocation.position,
            projectileSpawnLocation.rotation);
        spawnedProjectile.GetComponent<Rigidbody>()
            .AddForce(projectileSpawnLocation.forward * projectileSpeed, ForceMode.Impulse);
        spawnedProjectile.GetComponent<Projectile>().dammage = projectileDammage;

        shootingTimer = 0;
    }

    private void RamIntoPlayer()
    {
        isShooting = false;
        if (!haveRamLocation)
        {
            if (defectiveRamleftOrRight == 0)
            {
                waypoint = s_HandControllerR.transform;
            }
            else
            {
                waypoint = s_HandControllerL.transform;
            }
            s_Enemy.defective = true;
            dir = waypoint.position - transform.position;
            haveRamLocation = true;
            StartCoroutine(SwitchStateWithDelay(DEATH, ramTilDeathDelay));
        }
        speedmove += 0.02f;
        transform.Translate(dir.normalized * speedmove * Time.deltaTime);
        
    }

    public void DestroySelf()
    {
        s_gamemanager.AddDroneDestroyed();
        s_audiomanager.Play("death");
        Instantiate(fireboom, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ProjectilePlayer"))
        {
            Instantiate(hitParticle, transform.position, transform.rotation);
            health -= collision.gameObject.GetComponent<Projectile>().dammage;
        }
        
    }
    public void UpdateHealth(int value)
    {
        health += value;
    }

    public void VariationAdjustment(int vhealth, int vdammage, int vdefectivespawn, int vdefectivedeath, float vspeed, float vidletime, float vprojectilespeed, float vshootingdelay)
    {
        health = vhealth;
        projectileDammage = vdammage;
        defectiveChangePercentOnWarp = vdefectivespawn;
        defectiveChangePercentOnDeath = vdefectivedeath;
        timeInIdleState = vidletime;
        speedmove = vspeed;
        projectileSpeed = vprojectilespeed;
        shootingDelay = vshootingdelay;
    }

}



