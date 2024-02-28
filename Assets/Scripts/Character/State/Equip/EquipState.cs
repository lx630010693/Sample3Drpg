using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipState : BasePlayerState
{
    public EquipState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        player.anim.CrossFade("Equip", 0.15f, 0);
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
        if (info.IsName("Equip") && info.normalizedTime >= 0.95f)
        {
            player.fsm.SwitchState(E_PlayerState.Idle);
        }
        if (info.IsName("Equip") && player.para.isWounding)
        {
            player.fsm.SwitchState(E_PlayerState.Wound);
        }
    }

}
