using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Random : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public NavMeshAgent agent;
    public Transform pos;
    public bool x;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(pos.position);
        x = (agent.velocity.magnitude>0) ? true:false;
    }
}
