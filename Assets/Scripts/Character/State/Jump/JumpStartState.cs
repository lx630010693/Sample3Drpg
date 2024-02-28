using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStartState : BasePlayerState
{
    private string JumpStart;
    public JumpStartState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        JumpStart = player.para.isEquiped ? "AJumpStart" : "JumpStart";
        player.anim.CrossFade(JumpStart, 0.25f, 0);
        player.rig.AddForce(new Vector3(0, player.jumpForce, 0), ForceMode.Impulse);
        player.jumpForwardX = player.anim.velocity.x * player.maxJumpForwardLength;
        player.jumpForwardZ = player.anim.velocity.z * player.maxJumpForwardLength;
        
    }

    public override void OnExit()
    {
       
    }

    public override void OnFixedUpdate()
    {
        player.rig.velocity = new Vector3(player.jumpForwardX*Time.fixedDeltaTime, player.rig.velocity.y, player.jumpForwardZ*Time.fixedDeltaTime);
    }

    public override void OnUpdate()
    {
        AnimatorStateInfo info = player.anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(JumpStart) && player.para.isOverHead)
        {
            player.fsm.SwitchState(E_PlayerState.Jump);
        }
        if (info.IsName(JumpStart) && info.normalizedTime >= 0.95f)
        {
            player.fsm.SwitchState(E_PlayerState.Jump);
        }
        
    }
}
