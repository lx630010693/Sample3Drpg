using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����࣬��Ҫ��������һЩ״̬�������¼��ȵ�
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
    public InputControl inputControl;//���봦���࣬���������ȡ��������
    public PlayerFSM fsm;//���״̬����,���������л�״̬

    [Header("�۲����")]
    public Transform cameraTarget;
    public E_PlayerState curStateEnum;//�۲쵱ǰ��״̬
    public float rotateSpeed = 1000;//����ת������ʱ�л����ٶ�
    [Header("״̬����")]
    public PlayerStatePara para;

    [Header("����λ�����")]
    public Transform weaponHandler;//װ�������ϵ�λ��
    public Transform closeWeaponPos;//װ���ڽ�ϻ��λ��
    public GameObject weapon;//װ��

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rig;

    [Header("��Ծ���")]
    public float jumpForce;//��Ծʱ����������
    public float maxJumpForwardLength;//��Ծʱ�����ݵ�ǰ�������ǰ���ٶȣ��ܸ���������泯��λ��
    public float jumpForwardX;//��Ҫ���ܲο�JumpStartState״̬��
    public float jumpForwardZ;
    [Header("������")]
    public float onGroundRadius = 2f;//������
    public Vector3 onGroundPosOff;
    public float onOverHeadRadius = 2f;//ͷ�������û������
    public Vector3 onOverHeadPosOff;

    public Vector3 weaponCenterOff;//���������������ⷶΧ
    public Vector3 weaponCubeOff;
    public Vector3 defenseCenterOff;//����ʱ�������ⷶΧ
    public Vector3 defenseCubeOff;
    [Header("������ǰҡ����������ҡ")]
    public bool isOnWindUp;
    public bool isOnAction;
    public bool isOnFollowThrough;
    public int canAttackCount;//һ�ι��������ڿ���ɵ��˺��Ĵ�������ֹÿ֡�������˺���

    private void Awake()
    {
        para = new PlayerStatePara();
        inputControl = new InputControl();
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        inputControl.EnableInputControl();//���������
        fsm = new PlayerFSM(this);//��ʼ��״̬������
        fsm.SwitchState(E_PlayerState.Idle);//��Ĭ���е�Idle״̬
    }

    private void Update()
    {
        fsm.curState.OnUpdate();
    }

    private void FixedUpdate()
    {
        fsm.curState.OnFixedUpdate();
        PhyscisCheck();//���ڵ��桢ͷ����������
    }
    
    public void RotatePlayer()//��Ҫ�����ǣ������������������ϵ������ҵ�����������������ӽ�,����ʼ�ճ�������
    {
        if (inputControl.playerInputVec.Equals(Vector2.zero))
            return;
   
        Vector3 dir;
        Vector3 camForwardPro = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
       
        dir = camForwardPro * inputControl.playerInputVec.y + Camera.main.transform.right * inputControl.playerInputVec.x;
        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
    public void PhyscisCheck()//���棬ͷ��������
    {
        para.isOnGround = Physics.OverlapSphere(this.transform.position+onGroundPosOff, onGroundRadius,1<<LayerMask.NameToLayer("Ground")).Length>0?true:false;
        para.isOverHead = Physics.OverlapSphere(this.transform.position + onOverHeadPosOff, onOverHeadRadius, 1 << LayerMask.NameToLayer("Ground")).Length > 0 ? true : false;
    }
    public void AttackPhysicsCheck()//����ʱ���еĹ�����Χ���
    {
        Collider[] colliders = Physics.OverlapBox(weapon.transform.position + weaponCenterOff, weaponCubeOff / 2, weapon.transform.rotation,1<<LayerMask.NameToLayer("Enemy"));
        for (int i = 0; i < colliders.Length; i++)
        {
            
            if (colliders[i].gameObject.layer==LayerMask.NameToLayer("Enemy")&&canAttackCount>0)
            {
                Debug.Log("������");
                canAttackCount -= 1;
                colliders[i].gameObject.GetComponent<TestEnemy>().Wound();
            }

        }
    }
    private void OnDrawGizmos()//�༭���¿��ӻ�ȥ���Ƽ���λ��
    {
        //�����ж�
        Gizmos.DrawWireSphere(this.transform.position + onGroundPosOff, onGroundRadius);
        //ͷ���ж�
        Gizmos.DrawWireSphere(this.transform.position + onOverHeadPosOff, onOverHeadRadius);
        
        //�������ⷶΧ
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(weapon.transform.position, weapon.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(weaponCenterOff, weaponCubeOff);
        //����ʱ�������ⷶΧ
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
            print("������");
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
    #region �����¼�
    #region װ�����
    public void EquipEvent()//������װ����ж�����
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

    #region ������ǰҡ����������ҡ���
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

