using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������״̬
/// </summary>
public class PlayerFSM 
{
    private Dictionary<E_PlayerState, BasePlayerState> playerStates;//�洢״̬������ֱ���л�
    private PlayerObj player;
    public BasePlayerState curState;//��ǰ���е�״̬
    
    public PlayerFSM(PlayerObj obj)
    {
        player = obj;
        InitFSM();
    }
    public void InitFSM( )//�����ｫ���е�״̬��ʼ��
    {
        playerStates = new Dictionary<E_PlayerState, BasePlayerState>();
        playerStates.Add(E_PlayerState.Idle, new IdleState(player));
        playerStates.Add(E_PlayerState.Walk, new WalkState(player));
        playerStates.Add(E_PlayerState.WalkEnd, new WalkEndState(player));
        playerStates.Add(E_PlayerState.Run, new RunState(player));
        playerStates.Add(E_PlayerState.RunEnd, new RunEndState(player));
        playerStates.Add(E_PlayerState.Equip, new EquipState(player));
        playerStates.Add(E_PlayerState.Unarm, new UnarmState(player));
        playerStates.Add(E_PlayerState.JumpStart, new JumpStartState(player));
        playerStates.Add(E_PlayerState.Jump, new JumpState(player));
        playerStates.Add(E_PlayerState.JumpEnd, new JumpEndState(player));
        playerStates.Add(E_PlayerState.Attack, new AttackState(player));
        playerStates.Add(E_PlayerState.DefenseStart, new DefenseStartState(player));
        playerStates.Add(E_PlayerState.Defense, new DefenseState(player));
        playerStates.Add(E_PlayerState.Slide, new SlideState(player));
        playerStates.Add(E_PlayerState.Wound, new WoundState(player));
    }
    public void SwitchState(E_PlayerState stateName)//�л�״̬
    {
        if (curState != null)
        {
            curState.OnExit();
        }
        curState = playerStates[stateName];
        curState.OnEnter();
        player.curStateEnum = stateName;
    }
}
public enum E_PlayerState
{
    Idle,
    Walk,WalkEnd,
    Run,RunEnd,
    Equip,Unarm,
    JumpStart,Jump,JumpEnd,
    Attack,
    DefenseStart,Defense,
    Slide,
    Wound
}
public abstract class BasePlayerState//���״̬�Ļ���
{
    protected PlayerObj player;
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}