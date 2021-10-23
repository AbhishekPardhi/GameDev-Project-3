using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PumpkinHeadAI : MonoBehaviour
{
    public GameObject player;
    public Transform eyeLevel;
    public Transform headLevel;
    public Animator anim;
    public NavMeshAgent agent;
    [Header("Conditions")]
    public bool isVisible;
    public bool isAware;
    public bool inSight;
    public bool isWalking;
    public bool isRunning;
    public bool isAttacking;
    public bool inAttackRange;
    public bool isPuttingTrap;
    [Space(2)]
    [Header("Parameters")]
    public float FOV;
    public float moveSpeed;
    public float attackRange;
    public float idleTime;
    public float trapTime;
    [Space(2)]
    [Header("Vectors")]
    public Vector3 lastKnown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckConditions();
        UpdateBooleans();
        if (!isAware) Patrolling(); else StopCoroutine("IdleOrWalking");
        if (isAware && !inAttackRange) Chasing();
        if (isAware && inAttackRange) Attacking();
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
        Debug.Log("Attacked");
    }

    private void Chasing()
    {
        isRunning = true;
        if(agent.isStopped) agent.SetDestination(lastKnown);
        Debug.Log("Chasing");
    }
    private void Patrolling()
    {
        if (agent.isStopped)
        {
            isRunning = false;
            StartCoroutine("IdleOrWalking");
        }
        Debug.Log("Patrolling");
    }
    IEnumerator IdleOrWalking()
    {
        for (; ; )
        {
            isWalking = false;
            yield return new WaitForSeconds(idleTime);
            isWalking = true;
            if (isPuttingTrap)
            {
                yield return new WaitForSeconds(trapTime);
                isPuttingTrap = false;
            }
        }
    }

    private void CheckConditions()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(eyeLevel.position, headLevel.position - eyeLevel.position, out hit,layerMask)) isVisible = true;
        Vector3 vec = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        if (Vector3.Angle(transform.forward, vec) < FOV / 2) inSight = true; else inSight = false;
        if (inSight == true && isVisible == true) isAware = true; else isAware = false;
        if (Vector3.Distance(player.transform.position, transform.position) < attackRange) inAttackRange = true; else inAttackRange = false;
        if (isAware) lastKnown = player.transform.position;
    }
}
