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
        //InitAi(); //�׽�Ʈ
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
        Debug.Log(newState.ToString() + " changeAIState "  +  isInit); 
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
    public void SetTarget(GameObject newTarget)
    {
        if (AI_State.Idle == currentState || AI_State.Roaming == currentState)
        {
            Debug.Log("Ÿ����ȯ");
            attackTarget = newTarget;
            changeAIState(AI_State.chase);
        }
    }
    IEnumerator Idle()
    {
        yield return null;
    }

    IEnumerator Roaming()
    {
        Debug.Log("123");
        yield return null;
        Debug.Log("324");
        while (true)
        {
            Debug.Log("111");
            // Ư�� �������� 5~7�� ���� �ȴٰ�
            movePos.x = Random.Range(-5f, 5f);
            movePos.y = 0f;
            movePos.z = Random.Range(-5f, 5f);
            SetMoveTarget(homePos + movePos);
            yield return YieldInstructionCache.WaitForSeconds(Random.Range(5f, 7f));

            // 5~7�� �Ŀ� ���� �٤��㼭 �ȉ�
        }
    }


    // �÷��̾� �����ϴٰ�, �÷��̾� ����ϸ� Ȩ���� ����
    IEnumerator ReturnHome()
    {
        yield return null;

        // Ȩ ���������� Ÿ���� �����ؼ� �̵�
        SetMoveTarget(homePos);
        while (true)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);  //  1�ʿ� �� ���� Ȩ �����ǿ� �����ߴ��� üũ
            if (navAgent.remainingDistance < 1f)       // ��ǥ���� ���� ��λ� ���� �Ÿ�
                changeAIState(AI_State.Roaming);
        }

    }


    // Ÿ���� �����ϴ� ����. ��Ÿ����� ������ �����ϸ� ���ݻ��·�, ������ ����ߴٸ� ���� �� Ȩ
    IEnumerator chase()
    {
        yield return null;
        while (attackTarget != null)
        {
            if (GetDisTanceToTarget() < 3f)
            {
                changeAIState(AI_State.Attack);
            }
            else
            {
                SetMoveTarget(attackTarget.transform.position); // �̵� ��ǥ ������ ����
            }
            yield return YieldInstructionCache.WaitForSeconds(0.5f);    // �ʹ� ���� �ٲ㵵 ���ڿ��������� �ð� ������
        }

        changeAIState(AI_State.ReturnHome); //  
    }

    IEnumerator Attack()
    {
        yield return null;

        while (attackTarget != null)
        {
            if (GetDisTanceToTarget() > 3f) //  ��Ÿ����� ������ �ָ� �ִٸ�
            {
                changeAIState(AI_State.chase);  //  �������·�
            }

            transform.LookAt(attackTarget.transform);    //  ĳ������ ���� ȭ��
            monsterBase.AttackTarget(attackTarget.GetComponent<IBattle>());
            //todo : ���ݸ�ǰ� ������ ����
            yield return YieldInstructionCache.WaitForSeconds(2f);  // ���� �� ���� ���̺� �����ؼ� ���ݼӵ�
        }
        changeAIState(AI_State.ReturnHome); // Ÿ�� ����ϸ� ������
    }
}

//���� ���� ���
