using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public enum SpawnType
{
    ST_Once,    // 한 번에 맥스 카운트까지 스폰하는 방식 ( 일회성
    ST_Repeat, // 한 마리씩 지속적으로 맥스카운트까지 스폰
    
}
    [RequireComponent(typeof(BoxCollider))]
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private int maxCount;   // 해당 존에 최대로 배치될 수 있는 몬스터 숫자
    private int curCount;   // 해당 존에 현재 살아있느  몬스터의 숫자
    private PoolManager poolManager;    // 오브젝트 풀 매니저 컴포넌트
    private BoxCollider col;

    [SerializeField]
    private SpawnType spawnType;    //   해당 존의 스폰 방식
    [SerializeField]
    private int spawnMonsterTableID;

    private void Awake()
    {
        if (!TryGetComponent<PoolManager>(out poolManager))
            Debug.Log("SpawnManager.cs - Awake() - poolManager 참조 실패");
        if (!TryGetComponent<BoxCollider>(out col))
            Debug.Log("SpawnManager.cs - Awake() - col 참조 실패");
        else
        {
            col.isTrigger = true;
        }
    }

    //스폰하기 위한 준비 작업
    private void InitSpawnManager()
    {
        curCount = 0;
        spawnType = SpawnType.ST_Repeat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine("TrySpawn");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            StopCoroutine("TrySpawn");
    }

    IEnumerator TrySpawn()
    {
        yield return null;
        while (true)
        {
            yield return YieldInstructionCache.WaitForSeconds(2f);
            if (curCount < maxCount)
                SpawnUnit();
        }
    }

    private Vector3 spawnPos;
    private MonsterBase monsterBase;
    private void SpawnUnit()
    {
        curCount++;
        monsterBase = poolManager.GetFromPool<MonsterBase>(0);  // 오브젝트 풀에서 하나 꺼내겠다.

        spawnPos = transform.position;
        spawnPos.x += Random.Range(-5f, 5f);
        spawnPos.y = 0f;
        spawnPos.z += Random.Range(-5f, 5f);

        monsterBase.transform.position = spawnPos;
        monsterBase.InitMonster(spawnMonsterTableID);
    }

    public void ReturnPool(MonsterBase monser)
    {
        poolManager.TakeToPool<MonsterBase>(monsterBase.POOLNAME, monsterBase); //여기 못함
        curCount--;
    }
}

