using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType
{
    WT_Melee,
    WT_Range,
}

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponType type;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float attackRate;

    public float ATTACKRATE
    {
        get => attackRate;
    }

    private ProjectilePool pools;

    private GameObject obj;

    private void Awake()
    {
        pools = GameObject.FindAnyObjectByType<ProjectilePool>();

        if (pools == null)
            Debug.Log("Wealk .cs-Awake() - pools 참조 실패");



        InitWeapon();
    }

    public void InitWeapon()
    {
        Debug.Log("무기 초기화 완료");
    }


    public void Attack()
    {
        switch (type)
        {
            case WeaponType.WT_Melee:
                MeeleeAttack();
                break;
            case WeaponType.WT_Range:
                RangeAttack();
                break;
        }
    }

    private Coroutine coroutine;

    private void MeeleeAttack()
    {

    }

    private void RangeAttack()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);    //기존 완서안된 공격 멈추고
        coroutine = StartCoroutine(RangeMotion());  //새로운 공격 시작
    }

    IEnumerator RangeMotion()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        //이펙트 재생
        Debug.Log("몸을 감싸는 이펙트 재생");
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        Debug.Log("프로젝타일 발사");
        yield return YieldInstructionCache.WaitForSeconds(0.3f);
        Debug.Log("마나 감소");
    }


    public void SpawnProjectile()
    {
        Debug.Log("프로젝타일 발사");
        obj = pools.SpawnProjectile();
        obj.transform.position = transform.position;    //  무기 위치에 스폰
        obj.transform.LookAt(transform.position + transform.root.forward);  // 캐릭터 정면 방향으로 회전

        Projectile_HitEffect hitEffect = pools.SpawnHitEffect().GetComponent<Projectile_HitEffect>();


        obj.GetComponent<Projectile>().InitProjectile(transform.root.forward,//플레이어 정면방향,  
                                                        12.0f,  //날아가는 속도
                                                        7f,    //  라이프 타임
                                                        8,      //   데미지
                                                        transform.root.tag, // 오너 캐릭터 태그
                                                        pools.gameObject,   // 풀 매니저 오브젝트
                                                        hitEffect           // 폭파 이펙트
                                                        );
    }
}









