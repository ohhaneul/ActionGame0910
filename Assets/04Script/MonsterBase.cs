using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using UnityEngine.AI;

public interface IBattle
{
    public void TakeDamage(int damage); // 데미지를 받았을 때
    public void TakeStun(float time); // 스턴 걸림
    public void TakeStop();

}

public class MonsterBase : MonoBehaviour, IPoolObject, IBattle
    {
        protected Animator anims;
        protected NavMeshAgent navAgent;
        private MonsterAi monsterAI;

        [SerializeField]
        private string poolName;
        public string POOLNAME
        {
            get => poolName;
        }

        private int animHash_Run;
        private int animHash_Attack;

    private Material material;

        private void Awake()
        {
            if (!TryGetComponent<Animator>(out anims))
                Debug.Log("MonsterBase.cs - Awake() - anims - 참조실패");
            if (!TryGetComponent<NavMeshAgent>(out navAgent))
                Debug.Log("MonsterBase.cs - Awake() - navAgent - 참조실패");
            if (!TryGetComponent<MonsterAi>(out monsterAI))
                Debug.Log("MonsterBase.cs - Awake() - monsterAI - 참조실패");


            material = GetComponentInChildren<SkinnedMeshRenderer>().material;


            animHash_Run = Animator.StringToHash("Run Forward");
            animHash_Attack = Animator.StringToHash("Attack 01");

            InitMonster(101001);
        }

        private TableEntity_Monster monsterData;

        virtual public void InitMonster(int tableID)
        {
            if (!GameManager.Inst.GetMonsterData(tableID, out monsterData))
                Debug.Log("MonsterBase.cs - InitMonser() - monsterData - 참조실패");

            // 투두 : 스텟 적용
            // 레이어 설정 등등

            monsterAI.InitAi();
        }

        private void Update()
        {
            Locomotion();
        }

        private void Locomotion()
        {
            if (navAgent.velocity.sqrMagnitude > 0.1f)
                anims.SetBool(animHash_Run, true);
            else
                anims.SetBool(animHash_Run, false);

        }




        public void OnCreatedInPool()
        {
            //풀에 최초 생성될떄 호출되는 이벤트
        }

        public void OnGettingFromPool()
        {
            // 폴테서 꺼내서 사용되기 직전에 호풀되는 이벤트
        }

        public void TakeDamage(int damage)
        {
            Debug.Log("데미지 받음");

        StartCoroutine(OnHit());
        }

        public void TakeStun(float time)
        {
            Debug.Log("스턴에 당했습니다");
        }

        public void TakeStop()
        {
            Debug.Log("정지 스킬에 당했습니다");
        }

    IEnumerator OnHit()
    {
        for (int i = 0; i < 3; i++)
        {
            material.color = Color.red;
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            material.color = Color.white;
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
    }

    IEnumerator OnDie() //애니메이션 재생 -> 일정시간 이후 몬스터 사라짐
    {
        anims.SetTrigger("Die");
        gameObject.layer = LayerMask.NameToLayer("DieChar");
        material.color = Color.gray;

        yield return YieldInstructionCache.WaitForSeconds(2f);
        //풀에 반환
    }

        public IBattle target;
        virtual public void AttackTarget(IBattle newTarget)
        {
            anims.SetTrigger(animHash_Attack);  //  공격 모션 재생
            target = newTarget;

            //Invoke("ApplyDamage, 0.1f");
        }

        private void ApplyDamage()
        {
            Debug.Log("아벤트");
            target.TakeDamage(1);
        }

}
