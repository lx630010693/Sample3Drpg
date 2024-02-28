using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : BasePlayerState
{
    public SlideState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        player.anim.CrossFade("Slide", 0.2f, 0);
        player.para.isSlideing = true;
    }

    public override void OnExit()
    {
        player.para.isSlideing = false;
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        AnimatorStateInfo info = player.anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Slide") && info.normalizedTime >= 0.95)
        {
            player.fsm.SwitchState(E_PlayerState.Idle);
        }
    }
}
