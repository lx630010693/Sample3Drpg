using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyFSM 
{
    protected Dictionary< E_BaseEnemyState, BaseEnemyState> enemyStates;//�洢״̬������ֱ���л�
    protected BaseEnemyObj enemy;
    public BaseEnemyState curState;//��ǰ���е�״̬
    

    public abstract void InitFSM();
    public abstract void SwitchState(E_BaseEnemyState stateName);
}

public enum E_BaseEnemyState
{
    Attack,Move,Wound,Dead
}
public abstract class BaseEnemyState//����״̬�Ļ���
{
    protected BaseEnemyObj enemy;
    protected HTNPlanBuilder htn;
    public abstract void InitHTN();
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}