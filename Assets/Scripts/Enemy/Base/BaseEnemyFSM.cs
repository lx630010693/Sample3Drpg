using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyFSM 
{
    protected Dictionary< E_BaseEnemyState, BaseEnemyState> enemyStates;//存储状态，方便直接切换
    protected BaseEnemyObj enemy;
    public BaseEnemyState curState;//当前运行的状态
    

    public abstract void InitFSM();
    public abstract void SwitchState(E_BaseEnemyState stateName);
}

public enum E_BaseEnemyState
{
    Attack,Move,Wound,Dead
}
public abstract class BaseEnemyState//敌人状态的基类
{
    protected BaseEnemyObj enemy;
    protected HTNPlanBuilder htn;
    public abstract void InitHTN();
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}