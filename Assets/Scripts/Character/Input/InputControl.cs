using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputControl
{
    public InputController input;//InputController��InputSystem���ɵĽű�
    public bool IsWalk => playerInputVec.magnitude >= 1 ? true : false;
    public Vector2 playerInputVec => input.Player.Move.ReadValue<Vector2>().normalized;
    public bool IsRun;
    public bool Equiping => input.Player.Equip.ReadValue<float>() > 0 ? true : false;
    public bool IsJump => input.Player.Jump.ReadValue<float>() > 0 ? true : false;
    public bool IsAttack => input.Player.Attack.WasPressedThisFrame();
    public bool IsDefense => input.Player.Defense.IsPressed();
    public bool IsSlide => input.Player.Slide.WasPerformedThisFrame();//Slide��Ҫ����˫��shift
    public InputControl()
    {
        input = new InputController();

        input.Player.Run.performed += (a) =>//�ܲ�״̬��Ҫ����0.4s �Ͳ��ܼ򵥵�ͨ���Ϸ��ļ�ⷽʽ�������������InputController�ļ���
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
