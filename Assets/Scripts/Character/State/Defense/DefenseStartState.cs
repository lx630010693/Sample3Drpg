using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseStartState : BasePlayerState
{
    public DefenseStartState(PlayerObj obj)
    {
        this.player = obj;
    }
    public override void OnEnter()
    {
        player.anim.CrossFade("DefenseStart", 0.2f, 0);
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
        if (info.IsName("DefenseStart") && info.normalizedTime >= 0.95f)
        {
            player.fsm.SwitchState(E_PlayerState.Defense);
        }
    }
    public void DefensePhysicsCheck()
    {
        Collider[] colliders = Physics.OverlapBox(player.weapon.transform.position + player.defenseCenterOff,player.defenseCubeOff / 2, player.weapon.transform.rotation, 1 << LayerMask.NameToLayer("EWeapon"));
        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].gameObject.layer == LayerMask.NameToLayer("EWeapon"))
            {
                Debug.Log("ÍêÃÀµ¯·´£¡");
                colliders[i].gameObject.transform.GetComponentInParent<TestEnemy>().Wound();
                player.anim.CrossFade("PerfectDefense", 0.1f, 0);
                player.fsm.SwitchState(E_PlayerState.Defense);
            }

        }
    }

}
