using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum AI_State
{
    Idle,   // ���ڸ��� ���ִ� ���� (�ֺ��� ������ ������ Ž��)
    Roaming,    // ���� ��ġ�� �������� ���� �ݰ��� ��ȸ ����
    ReturnHome, //  �÷��̾ �����ϴٰ�, �����Ÿ� �̻� �־����� Ȥ�� �÷��̾ ����ϸ� ���� ��ġ�� �ǵ��ƿ��� ����
    chase,  //�÷��̾ �����ϴ� ����
    Attack, // �÷��̾ �����ϰ� �ִ� ����,
}


public class MonsterAi : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private AI_State currentState;
    private MonsterBase monsterBase;

    private GameObject attackTarget;

    private Vector3 homePos;    // ������ ��ġ
    private Vector3 movePos;    // �̵� ��ǥ ��ǥ

    private bool isInit;        // AI �۵��� ���� ������ �Ϸ� �Ǿ��°�





    private void Awake()
    {
        if (!TryGetComponent<NavMeshAgent>(out navAgent))
            Debug.Log("MonsterAi.cs - Awake() - nagAgent ���� ����");

        if (!TryGetComponent<MonsterBase>(out monsterBase))
            Debug.Log("MonsterAi.cs - Awake() - monsterBase ���� ����");

        isInit = false;

    }


    //AI �۵��� �ʿ��� ó���� �Ϸ�� �Ŀ�
    //  MonsterBase�� ���� AI �۵��� �����ϴ� �Լ�
    public void InitAi()
    {
        isInit = true;
        attackTarget = null;
        homePos = transform.position;   //  AI�� ���۵Ǵ� ���� ��ǥ�� Ȩ ���������� ����

        changeAIState(AI_State.Roaming);

    }

    private void SetMoveTarget(Vector3 targetPos)
    {
        navAgent.SetDestination(targetPos);     // �׺�����Ʈ�� ������ ����
    }


    //������ ���� AI �����Ű�� ���� ���� AI ����
    private void changeAIState(AI_State newState)
    {
        if (isInit) // ���׹���
        {
            StopCoroutine(currentState.ToString()); //���� AI ���� ����
            currentState = newState;    // �ű� ���·� ����
            StartCoroutine(currentState.ToString());    //  �ű� AI ���� ����
        }
    }

    private float GetDisTanceToTarget()
    {
        if (attackTarget)
        {
            return Vector3.Distance(transform.position, attackTarget.transform.position);
        }

        return -1f; // ���� ���� �ڵ�
    }



    // ���ܰ� ����� ã���� �� �ش� ����� AI �� attackTarget���� �������ִ� �Լ�
    private void SetTarget(GameObject newTarget)
    {
        if (AI_State.Idle == currentState || AI_State.Roaming == currentState)
        {
            attackTarget = newTarget;
            changeAIState(AI_State.chase);
        }
    }
    IEnumerable Idle()
    {
        yield return null;
    }

    IEnumerable Roaming()
    {
        yield return null;
    }

    IEnumerable ReturnHome()
    {
        yield return null;
    }

    IEnumerable chase()
    {
        yield return null;
    }

    IEnumerable Attack()
    {
        yield return null;
    }

}
