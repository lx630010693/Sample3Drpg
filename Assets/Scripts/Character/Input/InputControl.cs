using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputControl
{
    public InputController input;//InputController是InputSystem生成的脚本
    public bool IsWalk => playerInputVec.magnitude >= 1 ? true : false;
    public Vector2 playerInputVec => input.Player.Move.ReadValue<Vector2>().normalized;
    public bool IsRun;
    public bool Equiping => input.Player.Equip.ReadValue<float>() > 0 ? true : false;
    public bool IsJump => input.Player.Jump.ReadValue<float>() > 0 ? true : false;
    public bool IsAttack => input.Player.Attack.WasPressedThisFrame();
    public bool IsDefense => input.Player.Defense.IsPressed();
    public bool IsSlide => input.Player.Slide.WasPerformedThisFrame();//Slide需要快速双击shift
    public InputControl()
    {
        input = new InputController();

        input.Player.Run.performed += (a) =>//跑步状态需要长按0.4s 就不能简单的通过上方的检测方式来做到（具体见InputController文件）
        {
            IsRun = true;
        };
        input.Player.Run.canceled += (a) =>
        {
            IsRun = false;
        };
    }
    public void EnableInputControl()
    {
        input.Enable();
    }    
    public void DisableInputControl()
    {
        input.Disable();
    }
    
    
}
