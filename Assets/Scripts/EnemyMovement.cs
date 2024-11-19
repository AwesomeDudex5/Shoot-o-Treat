using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    UnityEngine.AI.NavMeshAgent agent;
    Transform target;
    public float radius = 5f;
    private bool dead;
    Collider m_Collider;


    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_Collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        dead = GetComponent<EnemyInfo>().dead;

        if (!dead) //death cheak
        {
            //move agent when target is not in within raidus
            if (Vector3.Distance(transform.position, target.position) > radius)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
                agent.destination = target.position;
               
            }

            else
            {
                agent.isStopped = true;
              
            }
        }
        else
        {
            agent.isStopped = true;
            m_Collider.enabled = false;
        }
    }
}
