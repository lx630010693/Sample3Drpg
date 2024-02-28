using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class QinEnemy : BaseEnemyObj
{
    public bool isFindPlayer;

    private void Awake()
    {
        fsm = new QinFSM(this);
        agent = this.GetComponent<NavMeshAgent>();
    }
}
