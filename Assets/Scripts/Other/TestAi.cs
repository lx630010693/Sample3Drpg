using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAi : MonoBehaviour
{
    public Transform[] posToGo;
    public NavMeshAgent agent;
    public Animator ani;
    private int index;
    private Rigidbody rig;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        index = 0;
    }

    public void GoToNextPos()
    {
        if (index >= posToGo.Length)
        {
            index = 0;
        }
        agent.isStopped = false;
        ani.SetFloat("Vertical Speed", 2f);
        agent.SetDestination(posToGo[index].position);
        index++;
    }

    private void Update()
    {
        
        if (agent.remainingDistance <= 0.1f)
        {
            agent.isStopped = true;
            ani.SetFloat("Vertical Speed", 0f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GoToNextPos();
        }
    }
  
}
