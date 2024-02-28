using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QinFSM : BaseEnemyFSM
{

    public QinFSM(QinEnemy obj)
    {
        this.enemy = obj;
        InitFSM();
    }
    public override void InitFSM()
    {
        
    }

    public override void SwitchState(E_BaseEnemyState stateName)
    {
        if (curState != null)
        {
            curState.OnExit();
        }
        curState = enemyStates[stateName];
        curState.OnEnter();
        enemy.curStateEnum = stateName;
    }
}
