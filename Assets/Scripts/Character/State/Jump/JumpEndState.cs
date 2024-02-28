using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEndState : BasePlayerState
{
    private string JumpEnd;

    private string Idle;
    public JumpEndState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        JumpEnd = player.para.isEquiped ? "AJumpEnd" : "JumpEnd";
        Idle = player.para.isEquiped ? "AIdle" : "Idle";
        player.anim.CrossFade(JumpEnd, 0.2f, 0);
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
        if (info.IsName(JumpEnd) && info.normalizedTime >= 0.95f)
        {
            player.fsm.SwitchState(E_PlayerState.Idle);
        }
        if (info.IsName(JumpEnd) &&player.inputControl.IsSlide)
        {
            player.fsm.SwitchState(E_PlayerState.Slide);
        }
    }

    
}
