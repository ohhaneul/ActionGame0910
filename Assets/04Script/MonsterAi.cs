using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum AI_State
{
    Idle,   // 제자리에 서있는 상태 (주변에 적들이 오는지 탐색)
    Roaming,    // 스폰 위치를 기준으로 일정 반경을 배회 상태
    ReturnHome, //  플레이어를 추적하다가, 일정거리 이상 멀어지면 혹은 플레이어가 사망하면 스폰 위치로 되돌아오는 상태
    chase,  //플레이어를 추적하는 상태
    Attack, // 플레이어를 공격하고 있는 상태,

}


public class MonsterAi : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private AI_State currentState;
    private MonsterBase monsterBase;

    private GameObject attackTarget;

    private Vector3 homePos;    // 스폰된 위치
    private Vector3 movePos;    // 이동 목표 좌표

    private bool isInit;        // AI 작동을 위해 세팅이 완료 되었는가





    private void Awake()
    {
        if (!TryGetComponent<NavMeshAgent>(out navAgent))
            Debug.Log("MonsterAi.cs - Awake() - nagAgent 참조 실패");

        if (!TryGetComponent<MonsterBase>(out monsterBase))
            Debug.Log("MonsterAi.cs - Awake() - monsterBase 참조 실패");

        isInit = false;
        //InitAi(); //테스트
    }


    //AI 작동에 필요한 처리가 완료된 후에
    //  MonsterBase에 의해 AI 작동을 시작하는 함수
    public void InitAi()
    {
        isInit = true;
        attackTarget = null;
        homePos = transform.position;   //  AI가 시작되는 현재 좌표를 홈 포지션으로 지정

        changeAIState(AI_State.Roaming);
    }

    private void SetMoveTarget(Vector3 targetPos)
    {
        navAgent.SetDestination(targetPos);     // 네비에이전트에 목적지 설정
    }


    //현재의 상태 AI 종료시키고 다음 상태 AI 시작
    private void changeAIState(AI_State newState)
    {
        Debug.Log(newState.ToString() + " changeAIState "  +  isInit); 
        if (isInit) // 버그방지
        {
            StopCoroutine(currentState.ToString()); //현재 AI 상태 종료
            currentState = newState;    // 신규 상태로 저장
            StartCoroutine(currentState.ToString());    //  신규 AI 상태 시작
        }
    }

    private float GetDisTanceToTarget()
    {
        if (attackTarget)
        {
            return Vector3.Distance(transform.position, attackTarget.transform.position);
        }

        return -1f; // 오류 검증 코드
    }



    // 공겨갈 대상을 찾았을 때 해당 대상을 AI 의 attackTarget으로 지정해주는 함수
    public void SetTarget(GameObject newTarget)
    {
        if (AI_State.Idle == currentState || AI_State.Roaming == currentState)
        {
            Debug.Log("타겟전환");
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
            // 특정 방향으로 5~7초 정도 걷다가
            movePos.x = Random.Range(-5f, 5f);
            movePos.y = 0f;
            movePos.z = Random.Range(-5f, 5f);
            SetMoveTarget(homePos + movePos);
            yield return YieldInstructionCache.WaitForSeconds(Random.Range(5f, 7f));

            // 5~7초 후에 방향 바ㅏ꿔서 걷돍
        }
    }


    // 플레이어 추적하다가, 플레이어 사망하면 홈으로 복귀
    IEnumerator ReturnHome()
    {
        yield return null;

        // 홈 포지션으로 타겟을 지정해서 이동
        SetMoveTarget(homePos);
        while (true)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);  //  1초에 한 번씩 홈 포지션에 도달했는지 체크
            if (navAgent.remainingDistance < 1f)       // 목표까지 가는 경로상에 남은 거리
                changeAIState(AI_State.Roaming);
        }

    }


    // 타겟을 추적하는 상태. 사거리보다 가깝게 접근하면 공격상태로, 상대방이 사망했다면 리턴 투 홈
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
                SetMoveTarget(attackTarget.transform.position); // 이동 목표 지점을 갱신
            }
            yield return YieldInstructionCache.WaitForSeconds(0.5f);    // 너무 자주 바꿔도 부자연스러워서 시간 정해줌
        }

        changeAIState(AI_State.ReturnHome); //  
    }

    IEnumerator Attack()
    {
        yield return null;

        while (attackTarget != null)
        {
            if (GetDisTanceToTarget() > 3f) //  사거리보다 상대방이 멀리 있다면
            {
                changeAIState(AI_State.chase);  //  추적상태로
            }

            transform.LookAt(attackTarget.transform);    //  캐릭터의 방향 화전
            monsterBase.AttackTarget(attackTarget.GetComponent<IBattle>());
            //todo : 공격모션과 데미지 판정
            yield return YieldInstructionCache.WaitForSeconds(2f);  // 투두 ㅣ 몬스터 테이블 참조해서 공격속도
        }
        changeAIState(AI_State.ReturnHome); // 타겟 사망하면 집으로
    }
}

//유한 상태 기계
