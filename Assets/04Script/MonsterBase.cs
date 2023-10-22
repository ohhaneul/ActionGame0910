using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using UnityEngine.AI;

public interface IBattle
{
    public void TakeDamage(int damage); // �������� �޾��� ��
    public void TakeStun(float time); // ���� �ɸ�
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
                Debug.Log("MonsterBase.cs - Awake() - anims - ��������");
            if (!TryGetComponent<NavMeshAgent>(out navAgent))
                Debug.Log("MonsterBase.cs - Awake() - navAgent - ��������");
            if (!TryGetComponent<MonsterAi>(out monsterAI))
                Debug.Log("MonsterBase.cs - Awake() - monsterAI - ��������");


            material = GetComponentInChildren<SkinnedMeshRenderer>().material;


            animHash_Run = Animator.StringToHash("Run Forward");
            animHash_Attack = Animator.StringToHash("Attack 01");

            InitMonster(101001);
        }

        private TableEntity_Monster monsterData;

        virtual public void InitMonster(int tableID)
        {
            if (!GameManager.Inst.GetMonsterData(tableID, out monsterData))
                Debug.Log("MonsterBase.cs - InitMonser() - monsterData - ��������");

            // ���� : ���� ����
            // ���̾� ���� ���

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
            //Ǯ�� ���� �����ɋ� ȣ��Ǵ� �̺�Ʈ
        }

        public void OnGettingFromPool()
        {
            // ���׼� ������ ���Ǳ� ������ ȣǮ�Ǵ� �̺�Ʈ
        }

        public void TakeDamage(int damage)
        {
            Debug.Log("������ ����");

        StartCoroutine(OnHit());
        }

        public void TakeStun(float time)
        {
            Debug.Log("���Ͽ� ���߽��ϴ�");
        }

        public void TakeStop()
        {
            Debug.Log("���� ��ų�� ���߽��ϴ�");
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

    IEnumerator OnDie() //�ִϸ��̼� ��� -> �����ð� ���� ���� �����
    {
        anims.SetTrigger("Die");
        gameObject.layer = LayerMask.NameToLayer("DieChar");
        material.color = Color.gray;

        yield return YieldInstructionCache.WaitForSeconds(2f);
        //Ǯ�� ��ȯ
    }

        public IBattle target;
        virtual public void AttackTarget(IBattle newTarget)
        {
            anims.SetTrigger(animHash_Attack);  //  ���� ��� ���
            target = newTarget;

            //Invoke("ApplyDamage, 0.1f");
        }

        private void ApplyDamage()
        {
            Debug.Log("�ƺ�Ʈ");
            target.TakeDamage(1);
        }

}
