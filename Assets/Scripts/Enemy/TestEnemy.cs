using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private Animator anim;
    
    public Transform weaponHandler;//装备在手上的位置
    public Transform closeWeaponPos;//装备在剑匣的位置
    public GameObject weapon;//装备

    public Vector3 attackOffset;
    public float attackRange;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Wound()
    {
        anim.SetTrigger("Wound");
    }
    public void StartAttack()
    {
        anim.Play("Attack1_0");
    }
    public void EquipEvent()//武器的装备与卸下相关
    {
        weapon.transform.SetParent(weaponHandler, false);
    }
    public void AttackEvent()
    {
        Collider[] colliders= Physics.OverlapSphere(this.transform.position+attackOffset,attackRange,1<<LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                colliders[i].gameObject.GetComponent<PlayerObj>().Wound(this.transform);
            }

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position + attackOffset, attackRange);
    }

}
