using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BasePlayerState
{
    private string Jump;
    private string JumpEnd;
    public JumpState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        Jump = player.para.isEquiped ? "AJump" : "Jump";
        JumpEnd = player.para.isEquiped ? "AJumpEnd" : "JumpEnd";
        player.anim.CrossFade(Jump, 0.1f, 0);
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        player.rig.velocity = new Vector3(player.jumpForwardX * Time.fixedDeltaTime, player.rig.velocity.y, player.jumpForwardZ * Time.fixedDeltaTime);
    }

    public override void OnUpdate()
    {
        AnimatorStateInfo info = player.anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(Jump) && player.para.isOnGround)
        {
            player.fsm.SwitchState(E_PlayerState.JumpEnd);
        }
    }

}
