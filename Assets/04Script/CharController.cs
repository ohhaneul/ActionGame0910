using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class CharController : MonoBehaviour, IBattle
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

    private GameObject obj; //공용 참조용 

    [SerializeField]
    private Weapon weapon;




    private void Awake()
    {
        if (!TryGetComponent<Animator>(out anims))
            Debug.Log("CharController.cs - Awake() - anims 참조 실패");

        obj = GameObject.Find("Fixed Joystick");
        if (obj != null)
        {
            if(!obj.TryGetComponent<FixedJoystick>(out joystick))
                Debug.Log("CharController.cs - Awake() - joystick 참조 실패");
        }
    }

    private void Update()   //1초당 20~80 호출
    {
        if (isController)
        {
            moveDir.x = Input.GetAxis("Horizontal");
            moveDir.z = Input.GetAxis("Vertical");
            moveDir.x += joystick.Horizontal;
            moveDir.z += joystick.Vertical;
            moveDir.y = 0f;
            moveDir.Normalize(); //정규화, 모든 성분의 합이 1이 되도록
        }

        else
        {
            moveDir = Vector3.zero;
            isAttack = false;
        }




        transform.position += moveSpeed * Time.deltaTime * moveDir;
        transform.LookAt(transform.position + moveDir); //이동방향 바라보도록

        anims.SetBool(animsParam_isWalk, moveDir != Vector3.zero);   //해쉬코드를 사용해서 메모리 아낌 //anims.SetBool("isWalk",~.. 사용x
        //anims.SetBool() //해쉬테이블 or 해쉬코드라고 부름

        TryAttack();

    }

    private bool isDelay = true;
    private bool isAttack = false;
    private float attackTime = 0f;  //  마지막 공격 시점
    private float attackRate = 1f;
    private bool isController;      // 캐릭터 조작을 못하도록 제한 하는 변수
    public bool ISCOUNTROLLER
    {
        set => isController = true;
    }


    private void TryAttack()
    {
        isDelay = Time.time < attackTime;

        if (isAttack && !isDelay && weapon != null)
        {
            anims.SetTrigger(animsParam_doAttack);
            weapon.Attack();
            attackTime = Time.time + attackRate;    // 딜레이 시간 처리
            GameManager.Inst.ChangeMP(-5);
        }
    }

    public void AnimEvent_SpawnProjectile()
    {
        weapon.SpawnProjectile();
    }


    public void TakeDamage(int damage)
    {
        Debug.Log("플레이어가 공격에 당했습니다" + damage);
        GameManager.Inst.ChangeHP(-damage);
    }

    public void TakeStun(float time)
    {
        Debug.Log(" ");
    }

    public void TakeStop()
    {
        Debug.Log(" ");
    }
}
