using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BasePlayerState
{
    private List<string> clipsList;
    private int clipIndex;

    private string curClipName;

    public AttackState(PlayerObj obj)
    {
        clipsList = new List<string>();
        this.player = obj;
        for (int i = 0; i < 4; i++)
        {
            clipsList.Add("Attack1_"+ i);
        }
    }
    public override void OnEnter()
    {
        player.anim.CrossFade(clipsList[0], 0.2f, 0);
        clipIndex = 0;
        curClipName = clipsList[clipIndex++];
        
    }

    public override void OnExit()
    {
       
    }

    public override void OnFixedUpdate()
    {
        if (player.isOnAction)
        {
            player.AttackPhysicsCheck();
        }
    }

    public override void OnUpdate()
    {
        AnimatorStateInfo info = player.anim.GetCurrentAnimatorStateInfo(0);
        if (player.isOnWindUp)
        {
            player.RotatePlayer();
        }
        if (player.isOnFollowThrough&&player.inputControl.IsAttack&&player.para.isEquiped&&player.para.isOnGround)
        {
            player.EndAnimationFollowThrough();
            player.OnAnimationWindUp();
            player.anim.CrossFade(clipsList[clipIndex],0.2f,0);
            curClipName = clipsList[clipIndex];
            clipIndex++;
            if (clipIndex >= 4)
            {
                clipIndex = 0;
            }
        }
        if (info.IsName(curClipName) && info.normalizedTime >= 0.95)
        {
           
            player.ClearAllAnimationState();
            player.fsm.SwitchState(E_PlayerState.Idle);
        }
        if (player.isOnFollowThrough && player.inputControl.IsRun && player.inputControl.IsWalk)
        {
           
            player.ClearAllAnimationState();
            player.fsm.SwitchState(E_PlayerState.Run);
        }
        if (player.isOnFollowThrough&& !player.inputControl.IsRun && player.inputControl.IsWalk)
        {
            
            player.ClearAllAnimationState();
            player.fsm.SwitchState(E_PlayerState.Walk);
        }
        if (info.IsName(curClipName) &&player.isOnFollowThrough&&player.inputControl.IsSlide)
        {
            player.ClearAllAnimationState();
            player.fsm.SwitchState(E_PlayerState.Slide);
        }
        if (info.IsName(curClipName) && player.para.isWounding)
        {
            player.fsm.SwitchState(E_PlayerState.Wound);
        }

    }

}
