using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    private MonsterAi monsterAI;
    private SphereCollider col;

    private void Awake()
    {
        if (!TryGetComponent<SphereCollider>(out col))
            Debug.Log("Scaner.cs - Awake() - col 참조 실패");
        else
        {
            col.isTrigger = true;
            col.radius = 8f; // 투두 : 몬스터 테이블 참조해서 변경 가능
        }

        if (!transform.parent.TryGetComponent<MonsterAi>(out monsterAI))
            Debug.Log("Scaner.cs - Awake() - monsterAI 참조 실패");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("플레이어 발견");
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 발견2");
            monsterAI.SetTarget(other.gameObject);
        }    
    }
}
