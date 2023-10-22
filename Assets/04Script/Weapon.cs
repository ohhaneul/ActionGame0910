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
            Debug.Log("Wealk .cs-Awake() - pools ���� ����");



        InitWeapon();
    }

    public void InitWeapon()
    {
        Debug.Log("���� �ʱ�ȭ �Ϸ�");
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
            StopCoroutine(coroutine);    //���� �ϼ��ȵ� ���� ���߰�
        coroutine = StartCoroutine(RangeMotion());  //���ο� ���� ����
    }

    IEnumerator RangeMotion()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        //����Ʈ ���
        Debug.Log("���� ���δ� ����Ʈ ���");
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        Debug.Log("������Ÿ�� �߻�");
        yield return YieldInstructionCache.WaitForSeconds(0.3f);
        Debug.Log("���� ����");
    }


    public void SpawnProjectile()
    {
        Debug.Log("������Ÿ�� �߻�");
        obj = pools.SpawnProjectile();
        obj.transform.position = transform.position;    //  ���� ��ġ�� ����
        obj.transform.LookAt(transform.position + transform.root.forward);  // ĳ���� ���� �������� ȸ��

        Projectile_HitEffect hitEffect = pools.SpawnHitEffect().GetComponent<Projectile_HitEffect>();


        obj.GetComponent<Projectile>().InitProjectile(transform.root.forward,//�÷��̾� �������,  
                                                        12.0f,  //���ư��� �ӵ�
                                                        7f,    //  ������ Ÿ��
                                                        8,      //   ������
                                                        transform.root.tag, // ���� ĳ���� �±�
                                                        pools.gameObject,   // Ǯ �Ŵ��� ������Ʈ
                                                        hitEffect           // ���� ����Ʈ
                                                        );
    }
}









