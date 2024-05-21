using System.Collections;
using UnityEngine;

public class BossDryad : Monsters
{
    // ������ �������� ���¸� ������ ������ �ִ�. (MonsterState�� ������� �ʴ´�.)
    public enum BossState
    {
        IDLE,
        PATTERN,
        ATTACK,
        DEAD
    }

    // �÷��̾� ������ �޾ƾ� NevMesh�� ���� ������ ������.
    [SerializeField] PlayerController player;

    public BossState state = BossState.IDLE;

    float skill1_curCooltime = 0f;
    float skill2_curCooltime = 0f;

    Transform enemyTrf;
    Transform playerTrf;
    Animator animator;

    //public ParticleSystem rollAttack;
    public StateMachine stateMachine;

    [SerializeField] public GameObject projectile;
    public Transform[] spawnPositions;
    [SerializeField] public float shootSpeed = 800.0f;

    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashSkill = Animator.StringToHash("animation");

    Rigidbody rb;
    #region ��ų ���
    [SerializeField]
    float mstATK = 10.0f;
    float mstSPD = 10.0f;

    public float traceDistance = 10f;
    public float skillDistance = 10f;
    public float attackDistance = 2f;

    public bool isDie = false;

    int i = 0;
    #endregion

    protected override void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        playerTrf = player.transform;
        enemyTrf = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(BossState.IDLE, new IdleState(this));
        stateMachine.AddState(BossState.PATTERN, new TraceState(this));
        stateMachine.AddState(BossState.ATTACK, new AttackState(this));
        stateMachine.AddState(BossState.DEAD, new TraceState(this));
        stateMachine.InitState(BossState.IDLE);

    }
    protected virtual void Start()
    {
        StartCoroutine(BossPattern());
    }

    protected virtual IEnumerator BossPattern()
    {
        /* 
        1������
        �⺻ ���� - ź�� �߻� 

        �� ��ȯ 
        ���� ��ȯ���� ���� ����
        

        2������ ���ĳ���
        �⺻ ���� - ��ȭ ź�� �߻� 
        
        ��ź���� �߻� ����ü ������ �Ͷ߸�
        
        �߻� �߽����� �� �о��.
        +1���������

        2������ �� �� �ٱ��� ������� �ʴ� ���� �߰�.
       
        */
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            
            if (Input.GetKeyDown("1"))
            {
                i = 1;
                stateMachine.ChangeState(BossState.ATTACK);
            }
            else if (Input.GetKeyDown("2"))
            {
                i = 2;
                stateMachine.ChangeState(BossState.ATTACK);
            }
            else if (Input.GetKeyDown("3"))
            {
                i = 3;
                stateMachine.ChangeState(BossState.ATTACK);
            }

            /*if (state == MonsterState.DEAD)
            {
                stateMachine.ChangeState(MonsterState.DEAD);
                yield break;
            }*/

            bool isPatterned = false;
            // Ư�� ���ݰ��� ����
            /*if (isPatterned)
            {
                if(state != MonsterState.ATTACK)
                stateMachine.ChangeState(MonsterState.DEAD);
            }*/
        }
        /*stateMachine.ChangeState(MonsterState.DEAD);
        state = MonsterState.DEAD;*/
    }

    private void Update()
    {
        /*if (skill1_curCooltime > 0f)
        {
            skill1_curCooltime -= Time.deltaTime;
        }
        if (skill2_curCooltime > 0f)
        {
            skill2_curCooltime -= Time.deltaTime;
        }*/
    }

    class BaseEnemyState : BaseState
    {
        protected BossDryad owner;
        public BaseEnemyState(BossDryad owner)
        {
            this.owner = owner;
        }
    }

    class IdleState : BaseEnemyState
    {
        public IdleState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            
            owner.animator.SetBool(owner.hashAttack, false);
        }
    }

    class TraceState : BaseEnemyState
    {
        public TraceState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            owner.animator.SetBool(owner.hashAttack, false);
            
        }
    }

    class AttackState : BaseEnemyState
    {
        public AttackState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            if(owner.i == 1)
            {
                owner.Skill1();
            }
            else if (owner.i == 2)
            {
                owner.Skill2();
            }
            else if (owner.i == 3)
            {
                owner.Skill3();
            }
            else
            {
                owner.Attack();
            }
            /*float distance = Vector3.Distance(owner.playerTrf.position, owner.enemyTrf.position);

            if (distance >= owner.attackDistance && owner.skill1_curCooltime <= 0f)
            {
                owner.Skill1();
            }
            else if (owner.skill2_curCooltime <= 0f)
            {
                
                owner.Skill2();
            }
            else
            {
                owner.Attack();
            }*/
        }
    }

    class DeadState : BaseEnemyState
    {
        public DeadState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Dead");
        }

    }
    public override void Attack()
    {
        animator.SetBool(hashAttack, true);
    }
    public override void Skill1()
    {
        StartCoroutine(Skill_1());
        //����̸� �� ���ư��� ���� ��ų �ߵ�?
        /*float distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
        animator.SetBool(hashSkill1, true);*/
    }

    IEnumerator Skill_1()
    {
        float distance;
        
        distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
        
        GameObject pd = Instantiate(projectile, spawnPositions[0].position, Quaternion.identity) as GameObject;
        //GameObject pd2 = Instantiate(projectile, spawnPositions[0].position, Quaternion.identity) as GameObject;
        pd.transform.LookAt(playerTrf.localPosition);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.up * distance * 15.5f);


        animator.SetInteger(hashSkill, 1);
        yield return new WaitForSeconds(1.5f);
        animator.SetInteger(hashSkill, 0);
    }

    IEnumerator Skill_2()
    {
        animator.SetInteger(hashSkill, 2);
        yield return new WaitForSeconds(1.5f);
        animator.SetInteger(hashSkill, 0);
    }
    IEnumerator Skill_3()
    {
        animator.SetInteger(hashSkill, 3);
        yield return new WaitForSeconds(1.5f);
        animator.SetInteger(hashSkill, 0);
    }
    /*void RAttack()
    {
        StartCoroutine(RollingAttack());
    }
    IEnumerator RollingAttack()
    {
        agent.isStopped = true;
        ParticleSystem ps = Instantiate(rollAttack, this.transform); //, Quaternion.identity
        rb.AddRelativeForce(Vector3.forward * 20f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.8f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //skill1_curCooltime = mstSkill1Cooltime;
        Destroy(ps);
        animator.SetBool(hashSkill1, false);
    }*/

    public override void Skill2()
    {
        StartCoroutine(Skill_2());
    }

    public void Skill3()
    {
        StartCoroutine(Skill_3());
    }
}
