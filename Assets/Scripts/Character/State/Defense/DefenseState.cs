using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseState : BasePlayerState
{
    public DefenseState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        player.anim.CrossFade("Defense", 0.1f, 0);
        player.para.isDefense = true;
    }

    public override void OnExit()
    {
        player.para.isDefense = false;
    }

    public override void OnFixedUpdate()
    {
        DefensePhysicsCheck();
    }

    public override void OnUpdate()
    {
        AnimatorStateInfo info = player.anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Defense")&&!player.inputControl.IsDefense)
        {
            player.fsm.SwitchState(E_PlayerState.Idle);
        }
        if (info.IsName("Defense") && player.inputControl.IsSlide)
        {
            player.fsm.SwitchState(E_PlayerState.Slide);
        }
    }
    public void DefensePhysicsCheck()
    {
        Collider[] colliders = Physics.OverlapBox(player.weapon.transform.position + player.defenseCenterOff, player.defenseCubeOff / 2, player.weapon.transform.rotation, 1 << LayerMask.NameToLayer("EWeapon"));
        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].gameObject.layer == LayerMask.NameToLayer("EWeapon"))
            {
                Debug.Log("µ¯·´£¡");
                colliders[i].gameObject.transform.GetComponentInParent<TestEnemy>().Wound();
                player.anim.CrossFade("NormalDefense", 0.1f, 0);
            }

        }
    }

}
