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
