using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : BasePlayerState
{
    private string Run;
    public RunState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        Run = player.para.isEquiped ? "ARun" : "Run";
        player.anim.CrossFade(Run, 0.2f, 0);
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
          
        if (info.IsName(Run) && !player.inputControl.IsWalk)
        {
            player.fsm.SwitchState(E_PlayerState.RunEnd);
        }
        else if (info.IsName(Run) && !player.inputControl.IsRun)
        {
            player.fsm.SwitchState(E_PlayerState.Walk);
        }

        if (info.IsName(Run) && !player.para.isEquiped && player.inputControl.Equiping)
        {
            player.fsm.SwitchState(E_PlayerState.Equip);
        }
        if (info.IsName(Run) && player.para.isEquiped && player.inputControl.Equiping)
        {
            player.fsm.SwitchState(E_PlayerState.Unarm);
        }
        if (info.IsName(Run) && player.inputControl.IsJump)
        {
            player.fsm.SwitchState(E_PlayerState.JumpStart);
        }
        if (info.IsName(Run) && player.inputControl.IsAttack && player.para.isEquiped && player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.Attack);
        }
        if (info.IsName(Run) && player.inputControl.IsSlide)
        {
            player.fsm.SwitchState(E_PlayerState.Slide);
        }
        if (info.IsName(Run) && !player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.Jump);
        }
        if (info.IsName(Run) && player.para.isWounding)
        {
            player.fsm.SwitchState(E_PlayerState.Wound);
        }
        if (info.IsName(Run) && player.para.isWounding)
        {
            player.fsm.SwitchState(E_PlayerState.Wound);
        }
        if (info.IsName(Run) && player.inputControl.IsDefense && player.para.isEquiped)
        {
            player.fsm.SwitchState(E_PlayerState.DefenseStart);
        }
        player.RotatePlayer();
    }

    
}
