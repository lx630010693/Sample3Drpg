using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class BaseEnemyObj : MonoBehaviour
{
    public BaseEnemyFSM fsm;
    public E_BaseEnemyState curStateEnum;
    public NavMeshAgent agent; 
    public Transform[] posToGo;
       
}
