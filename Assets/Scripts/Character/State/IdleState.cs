using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BasePlayerState
{
    private string Idle;
    private string JumpStart;
    public IdleState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        Idle = player.para.isEquiped ? "AIdle" : "Idle";
        JumpStart = player.para.isEquiped ? "AJumpStart" : "JumpStart";
        player.anim.CrossFade(Idle, 0.2f,0);
        
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
        if (info.IsName(Idle))
        {
            player.RotatePlayer(); 
        }
        if (info.IsName(Idle) &&player.inputControl.IsRun&&player.inputControl.IsWalk)
        {
            player.fsm.SwitchState(E_PlayerState.Run);
        }
        if (info.IsName(Idle)&&!player.inputControl.IsRun&&player.inputControl.IsWalk)
        {
            player.fsm.SwitchState(E_PlayerState.Walk); 
        }
        if (info.IsName(Idle) &&!player.para.isEquiped&&player.inputControl.Equiping)
        {
            player.fsm.SwitchState(E_PlayerState.Equip);
        }
        if (info.IsName(Idle) &&player.para.isEquiped && player.inputControl.Equiping)
        {
            player.fsm.SwitchState(E_PlayerState.Unarm);
        }
        if (info.IsName(Idle)&&player.inputControl.IsJump)
        {
            player.fsm.SwitchState(E_PlayerState.JumpStart);
        }
        if (info.IsName(Idle) && player.inputControl.IsAttack&&player.para.isEquiped&&player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.Attack);
        }
        if (info.IsName(Idle) &&player.inputControl.IsDefense&&player.para.isEquiped)
        {
            player.fsm.SwitchState(E_PlayerState.DefenseStart);
        }
        if (info.IsName(Idle) && player.inputControl.IsSlide)
        {
            player.fsm.SwitchState(E_PlayerState.Slide);
        }
        if (info.IsName(Idle) && !player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.Jump);
        }
        if (info.IsName(Idle) && player.para.isWounding)
        {
            player.fsm.SwitchState(E_PlayerState.Wound);
        }

    }
}
