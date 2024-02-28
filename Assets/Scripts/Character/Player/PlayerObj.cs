using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 玩家类，主要用来处理一些状态，动画事件等等
/// </summary>
public class PlayerObj : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStatePara
    {
        public bool isEquiped;
        public bool isOnGround;
        public bool isOverHead;
        public bool isSlideing;
        public bool isLocking;
        public bool isWounding;
        public bool isDefense;
    }

    [HideInInspector]
    public bool attackIsOnFront;
    public InputControl inputControl;//输入处理类，用来方便获取键的输入
    public PlayerFSM fsm;//玩家状态机类,用来管理切换状态

    [Header("观察相关")]
    public Transform cameraTarget;
    public E_PlayerState curStateEnum;//观察当前的状态
    public float rotateSpeed = 1000;//人物转动方向时切换的速度
    [Header("状态参数")]
    public PlayerStatePara para;

    [Header("武器位置相关")]
    public Transform weaponHandler;//装备在手上的位置
    public Transform closeWeaponPos;//装备在剑匣的位置
    public GameObject weapon;//装备

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rig;

    [Header("跳跃相关")]
    public float jumpForce;//跳跃时给予刚体的力
    public float maxJumpForwardLength;//跳跃时，根据当前人物的正前方速度，能给予的最大的面朝向位移
    public float jumpForwardX;//主要功能参考JumpStartState状态类
    public float jumpForwardZ;
    [Header("物理检测")]
    public float onGroundRadius = 2f;//地面检测
    public Vector3 onGroundPosOff;
    public float onOverHeadRadius = 2f;//头顶检测有没有物体
    public Vector3 onOverHeadPosOff;

    public Vector3 weaponCenterOff;//武器攻击的物理检测范围
    public Vector3 weaponCubeOff;
    public Vector3 defenseCenterOff;//防御时的物理检测范围
    public Vector3 defenseCubeOff;
    [Header("动画的前摇、触发、后摇")]
    public bool isOnWindUp;
    public bool isOnAction;
    public bool isOnFollowThrough;
    public int canAttackCount;//一段攻击动画内可造成的伤害的次数（防止每帧都触发伤害）

    private void Awake()
    {
        para = new PlayerStatePara();
        inputControl = new InputControl();
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        inputControl.EnableInputControl();//打开输入管理
        fsm = new PlayerFSM(this);//初始化状态机管理
        fsm.SwitchState(E_PlayerState.Idle);//先默认切到Idle状态
    }

    private void Update()
    {
        fsm.curState.OnUpdate();
    }

    private void FixedUpdate()
    {
        fsm.curState.OnFixedUpdate();
        PhyscisCheck();//关于地面、头顶等物理检测
    }
    
    public void RotatePlayer()//主要作用是，根据相机所处的坐标系，让玩家的输入符合玩家相机的视角,或者始终朝向索敌
    {
        if (inputControl.playerInputVec.Equals(Vector2.zero))
            return;
   
        Vector3 dir;
        Vector3 camForwardPro = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
       
        dir = camForwardPro * inputControl.playerInputVec.y + Camera.main.transform.right * inputControl.playerInputVec.x;
        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
    public void PhyscisCheck()//地面，头顶物理检测
    {
        para.isOnGround = Physics.OverlapSphere(this.transform.position+onGroundPosOff, onGroundRadius,1<<LayerMask.NameToLayer("Ground")).Length>0?true:false;
        para.isOverHead = Physics.OverlapSphere(this.transform.position + onOverHeadPosOff, onOverHeadRadius, 1 << LayerMask.NameToLayer("Ground")).Length > 0 ? true : false;
    }
    public void AttackPhysicsCheck()//攻击时会有的攻击范围检测
    {
        Collider[] colliders = Physics.OverlapBox(weapon.transform.position + weaponCenterOff, weaponCubeOff / 2, weapon.transform.rotation,1<<LayerMask.NameToLayer("Enemy"));
        for (int i = 0; i < colliders.Length; i++)
        {
            
            if (colliders[i].gameObject.layer==LayerMask.NameToLayer("Enemy")&&canAttackCount>0)
            {
                Debug.Log("打中了");
                canAttackCount -= 1;
                colliders[i].gameObject.GetComponent<TestEnemy>().Wound();
            }

        }
    }
    private void OnDrawGizmos()//编辑器下可视化去控制检测的位置
    {
        //地面判断
        Gizmos.DrawWireSphere(this.transform.position + onGroundPosOff, onGroundRadius);
        //头顶判断
        Gizmos.DrawWireSphere(this.transform.position + onOverHeadPosOff, onOverHeadRadius);
        
        //刀物理检测范围
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(weapon.transform.position, weapon.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(weaponCenterOff, weaponCubeOff);
        //防御时的物理检测范围
        //if (curStateEnum == E_PlayerState.Defense || curStateEnum == E_PlayerState.DefenseStart)
        {
            Gizmos.color = Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(weapon.transform.position, weapon.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(defenseCenterOff, defenseCubeOff);
        }
        

    }
  
    public void Wound(Transform enemyTransform)
    {
        if (!para.isSlideing)
        {
            print("被击中");
            Vector3 temp = (enemyTransform.position - this.transform.position).normalized;
            float dot = Vector3.Dot(temp, this.transform.forward);
            if (dot > 0)
            {
                attackIsOnFront = true;
                if (!para.isDefense)
                {
                    para.isWounding = true;
                }
            }
            else
            {
                attackIsOnFront = false;
                para.isWounding = true;
            }
           
        }
        
    }
    #region 动画事件
    #region 装备相关
    public void EquipEvent()//武器的装备与卸下相关
    {
        para.isEquiped = true;
        weapon.transform.SetParent(weaponHandler, false);
    }
    public void CloseWeaponEvent()
    {
        para.isEquiped = false;
        weapon.transform.SetParent(closeWeaponPos, false);
    }
    #endregion

    #region 动作的前摇、触发、后摇相关
    public void OnAnimationWindUp()
    {
        isOnWindUp = true;
    }
    public void EndAnimationWindUp()
    {
        isOnWindUp = false;
    }

    public void OnAnimationAction()
    {
        canAttackCount++;
        isOnAction = true;
    }
    public void EndAnimationAction()
    {
        canAttackCount = 0;
        isOnAction = false;
    }
    
    public void OnAnimationFollowThrough()
    {
        isOnFollowThrough = true;
    }
    public void EndAnimationFollowThrough()
    {
        isOnFollowThrough = false;
    }

    public void ClearAllAnimationState()
    {
        isOnAction = false;
        isOnWindUp = false;
        isOnFollowThrough = false;
    }
    #endregion
    #endregion
}

