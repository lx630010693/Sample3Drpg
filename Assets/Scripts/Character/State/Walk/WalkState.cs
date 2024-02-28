using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BasePlayerState
{
    private string Walk;
    public WalkState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        Walk = player.para.isEquiped ? "AWalk" : "Walk";
        player.anim.CrossFade(Walk, 0.2f, 0);
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
        if (info.IsName(Walk) && !player.inputControl.IsWalk)
        {
            player.fsm.SwitchState(E_PlayerState.WalkEnd);
        }
        if (info.IsName(Walk) && player.inputControl.IsRun)
        {
            player.fsm.SwitchState(E_PlayerState.Run);
        }
        if (info.IsName(Walk) && !player.para.isEquiped && player.inputControl.Equiping)
        {
            player.fsm.SwitchState(E_PlayerState.Equip);
        }
        if (info.IsName(Walk) && player.para.isEquiped && player.inputControl.Equiping)
        {
            player.fsm.SwitchState(E_PlayerState.Unarm);
        }
        if (info.IsName(Walk) && player.inputControl.IsJump)
        {
            player.fsm.SwitchState(E_PlayerState.JumpStart);
        }
        if (info.IsName(Walk) && player.inputControl.IsAttack && player.para.isEquiped && player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.Attack);
        }
        if (info.IsName(Walk) && player.inputControl.IsSlide)
        {
            player.fsm.SwitchState(E_PlayerState.Slide);
        }
        if (info.IsName(Walk) && !player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.Jump);
        }
        if (info.IsName(Walk) && player.para.isWounding)
        {
            player.fsm.SwitchState(E_PlayerState.Wound);
        }
        if (info.IsName(Walk) && player.inputControl.IsDefense && player.para.isEquiped)
        {
            player.fsm.SwitchState(E_PlayerState.DefenseStart);
        }
        player.RotatePlayer();
    }
}
