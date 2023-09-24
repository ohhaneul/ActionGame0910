using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class CharController : MonoBehaviour
{
    private float moveSpeed = 6f;
    private Vector3 moveDir = Vector3.zero;

    private FixedJoystick joystick;

    #region Anims
    private Animator anims;
    private static int animsParam_isWalk = Animator.StringToHash("isWalk");
    private static int animsParam_isRun = Animator.StringToHash("isRun");
    private static int animsParam_doAttack = Animator.StringToHash("doAttack");
    private static int animsParam_doSkill = Animator.StringToHash("doSkill");
    #endregion

    private GameObject obj; //���� ������ 

    private void Awake()
    {
        if (!TryGetComponent<Animator>(out anims))
            Debug.Log("CharController.cs - Awake() - anims ���� ����");

        obj = GameObject.Find("Fixed Joystick");
        if (obj != null)
        {
            if(!obj.TryGetComponent<FixedJoystick>(out joystick))
                Debug.Log("CharController.cs - Awake() - joystick ���� ����");
        }
    }

    private void Update()   //1�ʴ� 20~80 ȣ��
    {
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");
        moveDir.x += joystick.Horizontal;
        moveDir.z += joystick.Vertical;
        moveDir.y = 0f;
        moveDir.Normalize(); //����ȭ, ��� ������ ���� 1�� �ǵ���

        transform.position += moveSpeed * Time.deltaTime * moveDir;
        transform.LookAt(transform.position + moveDir); //�̵����� �ٶ󺸵���

        anims.SetBool(animsParam_isWalk, moveDir != Vector3.zero);   //�ؽ��ڵ带 ����ؼ� �޸� �Ƴ� //anims.SetBool("isWalk",~.. ���x
        //anims.SetBool() //�ؽ����̺� or �ؽ��ڵ��� �θ�

    }
}
