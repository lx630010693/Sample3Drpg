using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class TopDownContoller : MonoBehaviour
{
    private Animator animator;
    public Vector2 playerInputVec;
    public bool isRunning;
    public bool armedRifle;
    public Vector3 playerMovement;
    private float rotateSpeed = 1000;
    private Transform playerTrans;
    private CharacterController controller;

    float currentSpeed;
    float targetSpeed;
    float walkSpeed = 1.75f;
    float runSpeed = 3.5f;

    public GameObject rifleInHand;
    public GameObject rifleOnBack;

    public TwoBoneIKConstraint rightHandConstraint;
    public TwoBoneIKConstraint leftHandConstraint;

    //public Transform rightHandPos;
    //public Transform leftHandPos;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        playerTrans = this.transform;
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        MovePlayer();
        SetTwoHandsWeight();
    }
   /* private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandPos.rotation);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPos.rotation);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

    }*/
   public void GetPlayerAim(InputAction.CallbackContext ctx)
    {
        if (ctx.action.phase == InputActionPhase.Started)
        {
            animator.SetBool("IsAiming", true);
        }else if (ctx.action.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("IsAiming", false);
        }
    }

    public void SetTwoHandsWeight()
    {
        rightHandConstraint.weight= animator.GetFloat("RightHandWeight");
        leftHandConstraint.weight = animator.GetFloat("LetHandWeight");
    }
    /// <summary>
    /// 动画事件没法传递bool参数，就用int 1代表true，0代表false
    /// </summary>
    /// <param name="isOnBack"></param>
    public void PutGrabRifle(int isOnBack)
    {
        if (isOnBack==1)
        {
            rifleOnBack.SetActive(true);
            rifleInHand.SetActive(false);
        }
        else if(isOnBack==0)
        {
            rifleOnBack.SetActive(false);
            rifleInHand.SetActive(true);
        }
    }
    public void GetPlayerMoveInput(InputAction.CallbackContext ctx)
    {
        playerInputVec = ctx.ReadValue<Vector2>();
    }
    public void GEtPlayerRunInput(InputAction.CallbackContext ctx)
    {
        isRunning = ctx.ReadValue<float>() > 0 ? true : false;
    }
    public void RotatePlayer()
    {
        if (playerInputVec.Equals(Vector2.zero))
            return;
        //playerMovement.x = playerInputVec.x;
        //playerMovement.z = playerInputVec.y;
        Vector3 dir;
        Vector3 camForwardPro = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        dir = camForwardPro * playerInputVec.y + Camera.main.transform.right * playerInputVec.x;
        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
       
        playerTrans.rotation = Quaternion.RotateTowards(playerTrans.rotation, targetRotation,rotateSpeed*Time.deltaTime);
    }
    public void MovePlayer()
    {
        targetSpeed = isRunning ? runSpeed : walkSpeed;
        targetSpeed *= playerInputVec.magnitude;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 0.5f);
        animator.SetFloat("Vertical Speed", currentSpeed);
    }
    public void GetArmedRifleInput(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0)
        {
            armedRifle = !armedRifle;
            animator.SetBool("Rifle", armedRifle);
        }
    }
    private void OnAnimatorMove()
    {
        //controller.Move(animator.deltaPosition);
        controller.SimpleMove(animator.velocity);
    }
}
