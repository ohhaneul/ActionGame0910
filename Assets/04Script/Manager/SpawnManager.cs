using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public enum SpawnType
{
    ST_Once,    // �� ���� �ƽ� ī��Ʈ���� �����ϴ� ��� ( ��ȸ��
    ST_Repeat, // �� ������ ���������� �ƽ�ī��Ʈ���� ����
    
}
    [RequireComponent(typeof(BoxCollider))]
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private int maxCount;   // �ش� ���� �ִ�� ��ġ�� �� �ִ� ���� ����
    private int curCount;   // �ش� ���� ���� ����ִ�  ������ ����
    private PoolManager poolManager;    // ������Ʈ Ǯ �Ŵ��� ������Ʈ
    private BoxCollider col;

    [SerializeField]
    private SpawnType spawnType;    //   �ش� ���� ���� ���
    [SerializeField]
    private int spawnMonsterTableID;

    private void Awake()
    {
        if (!TryGetComponent<PoolManager>(out poolManager))
            Debug.Log("SpawnManager.cs - Awake() - poolManager ���� ����");
        if (!TryGetComponent<BoxCollider>(out col))
            Debug.Log("SpawnManager.cs - Awake() - col ���� ����");
        else
        {
            col.isTrigger = true;
        }
    }

    //�����ϱ� ���� �غ� �۾�
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
        monsterBase = poolManager.GetFromPool<MonsterBase>(0);  // ������Ʈ Ǯ���� �ϳ� �����ڴ�.

        spawnPos = transform.position;
        spawnPos.x += Random.Range(-5f, 5f);
        spawnPos.y = 0f;
        spawnPos.z += Random.Range(-5f, 5f);

        monsterBase.transform.position = spawnPos;
        monsterBase.InitMonster(spawnMonsterTableID);
    }

    public void ReturnPool(MonsterBase monser)
    {
        poolManager.TakeToPool<MonsterBase>(monsterBase.POOLNAME, monsterBase); //���� ����
        curCount--;
    }
}

