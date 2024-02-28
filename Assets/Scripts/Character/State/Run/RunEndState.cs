using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEndState : BasePlayerState
{
    private string RunEnd;
    public RunEndState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        RunEnd = player.para.isEquiped ? "ARunEnd" : "RunEnd";
        player.anim.CrossFade(RunEnd, 0.2f, 0);
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        AnimatorStateInfo info = player.anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(RunEnd) && info.normalizedTime >= 0.95f)
        {
            player.fsm.SwitchState(E_PlayerState.Idle);
        }
        if (info.IsName(RunEnd) && !player.para.isEquiped && player.inputControl.Equiping)
        {
            player.fsm.SwitchState(E_PlayerState.Equip);
        }
        if (info.IsName(RunEnd) && player.para.isEquiped && player.inputControl.Equiping)
        {
            player.fsm.SwitchState(E_PlayerState.Unarm);
        }
        if (info.IsName(RunEnd) && !player.inputControl.IsRun && player.inputControl.IsWalk)
        {
            player.fsm.SwitchState(E_PlayerState.Walk);
        }
        if (info.IsName(RunEnd) && player.inputControl.IsRun && player.inputControl.IsWalk)
        {
            player.fsm.SwitchState(E_PlayerState.Run);
        }
        if (info.IsName(RunEnd) && player.inputControl.IsAttack && player.para.isEquiped && player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.Attack);
        }
        if (info.IsName(RunEnd) && player.inputControl.IsSlide)
        {
            player.fsm.SwitchState(E_PlayerState.Slide);
        }
        if (info.IsName(RunEnd) && !player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.Jump);
        }
        if (info.IsName(RunEnd) && player.para.isWounding)
        {
            player.fsm.SwitchState(E_PlayerState.Wound);
        }
        if (info.IsName(RunEnd) && player.inputControl.IsDefense && player.para.isEquiped)
        {
            player.fsm.SwitchState(E_PlayerState.DefenseStart);
        }
        player.RotatePlayer();
    }

    
}
