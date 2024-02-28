using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoundState : BasePlayerState
{
    private string Wound;
    public WoundState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        player.para.isWounding = false;
        Wound = player.attackIsOnFront ? "WoundFront" : "WoundBack";
        player.anim.CrossFade(Wound, 0.2f, 0);
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
        if (info.IsName(Wound) && info.normalizedTime >= 0.95)
        {
            player.fsm.SwitchState(E_PlayerState.Idle);
        }
        if (info.IsName(Wound) &&player.inputControl.IsWalk)
        {
            player.fsm.SwitchState(E_PlayerState.Idle);
        }
        if (info.IsName(Wound) && player.inputControl.IsSlide)
        {
            player.fsm.SwitchState(E_PlayerState.Slide);
        }
        if (player.para.isWounding)
        {
            player.fsm.SwitchState(E_PlayerState.Wound);
        }
    }
}
