using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PumpkinHeadAI : MonoBehaviour
{
    public GameObject player;
    public GameObject trap;
    public Transform eyeLevel;
    public Transform headLevel;
    public Animator anim;
    public NavMeshAgent agent;

    IEnumerator currentMoveCoroutine;
    private LineRenderer line;
    private List<Vector3> point;

    [Header("Conditions")]
    public bool isVisible;
    public bool isAware;
    public bool inSight;
    public bool isWalking;
    public bool isRunning;
    public bool isAttacking;
    public bool inAttackRange;
    public bool isPuttingTrap;
    public bool lastKnownSet;
    public bool isPatrollingCoroutineRunning;
    [Space(2)]

    [Header("Parameters")]
    public float FOV;
    public float walkingSpeed;
    public float runningSpeed;
    public float moveSpeed;
    public float attackRange;
    public float idleTime;
    public float trapTime;
    [Space(2)]

    [Header("Vectors")]
    public Vector3 lastKnown;
    [Space(2)]

    [Header("Waypoints")]
    public Transform[] wayPoints = new Transform[7];
    public Vector3 currentWaypoint;

    void Start()
    {
        //transform.position = wayPoints[0].position;
        isPatrollingCoroutineRunning = false;
        lastKnownSet = false;
        line = GetComponent<LineRenderer>();
        isPuttingTrap = false;
    }

    void FixedUpdate()
    {
        CheckConditions();
        UpdateBooleans();
        if (!isAware) Patrolling();
        else
        {
            if(currentMoveCoroutine!=null) StopCoroutine(currentMoveCoroutine);
            isPatrollingCoroutineRunning = false;
        }
        if (!inAttackRange && lastKnownSet) Chasing();
        if (isAware && inAttackRange) Attacking();
        else isAttacking = false;
        //if(agent.hasPath) DisplayLineDestination(); 
        line.positionCount = agent.path.corners.Length;
        line.SetPositions(agent.path.corners);
    }

    private void UpdateBooleans()
    {
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("IsRunning", isRunning);
        anim.SetBool("IsPuttingTrap", isPuttingTrap);
        anim.SetBool("IsAttacking", isAttacking);
    }

    private void Attacking()
    {
        isRunning = false;
        isWalking = false;
        isAttacking = true;
        Debug.Log("Attacked");
    }

    private void Chasing()
    {
        isRunning = true;
        agent.SetDestination(lastKnown);
        transform.LookAt(player.transform.position);
        Debug.Log("Chasing");
    }
    private void Patrolling()
    {
        if (!isPatrollingCoroutineRunning)
        {
            currentMoveCoroutine= IdleOrWalking();
            StartCoroutine(currentMoveCoroutine);
        }
        //Debug.Log("Patrolling");
    }
    IEnumerator IdleOrWalking()
    {
        isPatrollingCoroutineRunning = true;
        while (true)
        {
            if (agent.velocity.magnitude==0)
            {
                isWalking = false;
                yield return new WaitForSeconds(idleTime);
                currentWaypoint = wayPoints[Mathf.RoundToInt(UnityEngine.Random.Range(0, wayPoints.Length - 1))].position;
                agent.SetDestination(currentWaypoint);
            }
            Debug.Log("lauda");
            if (agent.velocity.magnitude>0)
            {
                isWalking = true;
                yield return null;
            }
            if (isPuttingTrap)
            {
                Vector3 pos = transform.position + transform.forward - transform.up * 0.9f;
                GameObject clone = Instantiate(trap, pos, transform.rotation); 
                yield return new WaitForSeconds(trapTime);
                isPuttingTrap = false;
            }
        }
    }

    private void CheckConditions()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(eyeLevel.position, headLevel.position - eyeLevel.position, out hit, Mathf.Infinity,layerMask))
        {
            if (hit.transform.tag == "Player")
            {
                isVisible = true;
                if (!isAware) Debug.DrawRay(eyeLevel.position, headLevel.position - eyeLevel.position, Color.green);
            }
            else
            {
                isVisible = false;
                Debug.DrawRay(eyeLevel.position, headLevel.position - eyeLevel.position, Color.white);
            }
        }
        Vector3 vec = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        if (Vector3.Angle(transform.forward, vec-transform.position) < FOV / 2) inSight = true; else inSight = false;
        if (inSight == true && isVisible == true) isAware = true; else isAware = false;
        if (Vector3.Distance(player.transform.position, transform.position) < attackRange) inAttackRange = true; else inAttackRange = false;
        if (isAware)
        {
            lastKnown = player.transform.position;
            lastKnownSet = true;
            Debug.DrawRay(eyeLevel.position, headLevel.position - eyeLevel.position, Color.red);
        }
        if (agent.velocity.magnitude == 0 && agent.remainingDistance<agent.stoppingDistance+0.1f) lastKnownSet = false;
        if (!isRunning) agent.speed = walkingSpeed;
        else agent.speed = runningSpeed;
        moveSpeed = agent.velocity.magnitude;
    }
    private void DisplayLineDestination() //Delete this function when not in use
    {
        if (agent.path.corners.Length < 2) return;
        for(int i=1;i<agent.path.corners.Length;i++)
        {
            int length = agent.path.corners.Length;
            line.positionCount = length;
            point = agent.path.corners.ToList();
            for (int j = 0; j < point.Count; j++)
            {
                line.SetPosition(j, point[j]);
            }
        }
    }
}
