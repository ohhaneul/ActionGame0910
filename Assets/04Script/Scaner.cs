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
            Debug.Log("Scaner.cs - Awake() - col ���� ����");
        else
        {
            col.isTrigger = true;
            col.radius = 8f; // ���� : ���� ���̺� �����ؼ� ���� ����
        }

        if (!transform.parent.TryGetComponent<MonsterAi>(out monsterAI))
            Debug.Log("Scaner.cs - Awake() - monsterAI ���� ����");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�÷��̾� �߰�");
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� �߰�2");
            monsterAI.SetTarget(other.gameObject);
        }    
    }
}
