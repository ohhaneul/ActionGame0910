using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Projectile_HitEffect : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private string poolName;
    private ParticleSystem ps;
    private PoolManager pools;
    private bool doPlay;

    private void Awake()
    {
        if (!TryGetComponent<ParticleSystem>(out ps))
            Debug.Log("HitEffect.cs - Awake() - ps참조실패");
    }



    public void InitHitEffect(GameObject pool)
    {
        if (!pool.TryGetComponent<PoolManager>(out pools))
            Debug.Log("HitEffect.cs - Awake() - pools 참조실패");

    }

    private void PlayEffect()
    {
        ps.Play();
        doPlay = true;
    }

    private void Update()
    {
        if (doPlay && !ps.isPlaying)
        {
            doPlay = false;
            pools.TakeToPool<Projectile_HitEffect>(poolName, this);
        }
    }

    public void OnCreatedInPool()
    {

    }

    public void OnGettingFromPool()
    {
    }

}
