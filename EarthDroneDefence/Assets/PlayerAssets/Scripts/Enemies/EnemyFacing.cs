using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFacing : MonoBehaviour
{
    [SerializeField] private GameObject s_HandControllerR;
    [SerializeField] private GameObject s_HandControllerL;
    [SerializeField] public Transform target;
    private Quaternion _lookrotation;
    private Vector3 _direction;
    private float turnSpeed = 50f;

    private int targetSwitchTime;
    private int targetToFace;

    public bool defective = false;

    void Start()
    {
        s_HandControllerR = GameObject.Find("RightHand Controller");
        s_HandControllerL = GameObject.Find("LeftHand Controller");

        targetSwitchTime = Random.Range(5, 21);
        TargetToFace();
    }

    private void FixedUpdate()
    {
        if (!defective)
        {
            FaceTarget();
        }
    }

    private void TargetToFace()
    {
        if (!defective)
        {
            targetToFace = Random.Range(0, 2);
            Invoke("TargetToFace", targetSwitchTime);
        }
        
    }

    private void FaceTarget()
    {
        if (targetToFace == 0)
        {
            target = s_HandControllerR.transform;
            target.position += Random.insideUnitSphere * 0.2f;
            _direction = (target.position - transform.position);
            _lookrotation = Quaternion.LookRotation(_direction, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, _lookrotation, Time.deltaTime * turnSpeed);
        }
        else
        {
             target = s_HandControllerL.transform;
             _direction = (target.position - transform.position);
             _lookrotation = Quaternion.LookRotation(_direction, Vector3.up);

             transform.rotation = Quaternion.Slerp(transform.rotation, _lookrotation, Time.deltaTime * turnSpeed);
        }
    }

}
