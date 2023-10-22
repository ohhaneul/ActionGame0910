using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Projectile : MonoBehaviour, IPoolObject
{

    private Vector3 moveDir;
    private float moveSpeed;
    [SerializeField]
    private float lifeTime;
    private int attackDamage;

    [SerializeField]
    private string poolName;
    private PoolManager poolManager;
    private Projectile_HitEffect hitEffect;
    private string ownerTag;    // ������Ÿ�� �߻������ ����
    private bool isInit;

    public void InitProjectile(Vector3 dir, float newSpeed, float newLifeTime, int damage,
                                string tag, GameObject pools, Projectile_HitEffect hitEffect)
    {

        moveDir = dir;
        moveSpeed = newSpeed;
        lifeTime = Time.time + newLifeTime;
        attackDamage = damage;
        ownerTag = tag;
        this.hitEffect = hitEffect;
        //rodo ����Ʈ �� �� �ְ� �ʱ�ȭ �Լ� ȣ��
        this.hitEffect.InitHitEffect(pools);
        poolManager = pools.GetComponent<PoolManager>();
        isInit = true;
    }

    private void Update()
    {
        if(isInit)
        {
            transform.position += Time.deltaTime * moveSpeed * moveSpeed * moveDir;
            transform.Rotate(Time.deltaTime * 360f * -Vector3.forward); // ���ư��鼭 ȸ��
            if (Time.time > lifeTime)
                Explosion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ownerTag))
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        if (hitEffect != null)
        {
            hitEffect.transform.position = transform.position;
            //���� ����Ʈ �÷���

                //���� ���޴�
            //�ֺ��� ������
            poolManager.TakeToPool<Projectile>(poolName, this);
        }
    }

    IBattle iBatt;

    private void ApplyDamage()
    {
        Collider[] coliders = Physics.OverlapSphere(transform.position, 2f);

        for (int i = 0; i < coliders.Length; i++)
            if (!coliders[i].CompareTag(ownerTag)
                && coliders[i].TryGetComponent<IBattle>(out iBatt))
            {
                iBatt.TakeDamage(attackDamage);
            }

    }

    public void OnCreatedInPool()
    {
        throw new System.NotImplementedException();
    }

    public void OnGettingFromPool()
    {
        throw new System.NotImplementedException();
    }
}
