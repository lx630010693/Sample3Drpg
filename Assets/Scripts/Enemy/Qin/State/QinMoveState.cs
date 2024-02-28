using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QinMoveState : BaseEnemyState
{
    public QinMoveState(BaseEnemyObj obj)
    {
        this.enemy = obj;
        InitHTN();
    }

    public override void InitHTN()
    {
        
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        htn.RunPlan();
    }

}
