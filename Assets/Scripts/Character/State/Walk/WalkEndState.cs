using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEndState : BasePlayerState
{
    private string WalkEnd;
    public WalkEndState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        WalkEnd = player.para.isEquiped ? "AWalkEnd" : "WalkEnd";
        player.anim.CrossFade(WalkEnd, 0.2f,0);
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
        if (info.IsName(WalkEnd) && info.normalizedTime >= 0.95)
        {
            player.fsm.SwitchState(E_PlayerState.Idle);
        }
        if (info.IsName(WalkEnd) && !player.para.isEquiped && player.inputControl.Equiping)
        {
            player.fsm.SwitchState(E_PlayerState.Equip);
        }
        if (info.IsName(WalkEnd) && player.para.isEquiped && player.inputControl.Equiping)
        {
            player.fsm.SwitchState(E_PlayerState.Unarm);
        }
        if (info.IsName(WalkEnd) && !player.inputControl.IsRun && player.inputControl.IsWalk)
        {
            player.fsm.SwitchState(E_PlayerState.Walk);
        }
        if (info.IsName(WalkEnd) && player.inputControl.IsRun && player.inputControl.IsWalk)
        {
            player.fsm.SwitchState(E_PlayerState.Run);
        }
        if (info.IsName(WalkEnd) && player.inputControl.IsAttack && player.para.isEquiped && player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.Attack);
        }
        if (info.IsName(WalkEnd) && player.inputControl.IsSlide)
        {
            player.fsm.SwitchState(E_PlayerState.Slide);
        }
        if (info.IsName(WalkEnd) && !player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.Jump);
        }
        if (info.IsName(WalkEnd) && player.para.isWounding)
        {
            player.fsm.SwitchState(E_PlayerState.Wound);
        }
        if (info.IsName(WalkEnd) && player.inputControl.IsDefense && player.para.isEquiped)
        {
            player.fsm.SwitchState(E_PlayerState.DefenseStart);
        }
        player.RotatePlayer();
    }

   
}
