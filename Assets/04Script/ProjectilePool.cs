using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class ProjectilePool : MonoBehaviour
{
    private PoolManager poolManager;

    private void Awake()
    {
        if (!TryGetComponent<PoolManager>(out poolManager))
            Debug.Log("ProjectilePool.cs - Awake() - poolManager 참조 실패");
    }

    public GameObject SpawnProjectile()
    {
        return poolManager.GetFromPool<Projectile>(0).gameObject;
    }

    public GameObject SpawnHitEffect()
    {
        return poolManager.GetFromPool<Projectile_HitEffect>(1).gameObject;
    }


}
