using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossDryad : MonoBehaviour, IDamagable
{
    public enum BossState
    {
        IDLE,
        PATTERN,
        ATTACK,
        DEAD
    }

    private bool isInvincible = false; // ���� ����

    [SerializeField] private float curHP = 500f; // ������ ���� ü��
    [SerializeField] private float maxHP = 500f; // ������ �ִ� ü��

    protected Transform enemyTrf; // ����(�ڽ�)�� ��ġ(Transform)
    protected Transform playerTrf; // �÷��̾��� ��ġ(Transform)
    protected NavMeshAgent agent; // �׺���̼�(NavMesh)
    protected Animator animator; // �ִϸ�����(Animator)
    protected Rigidbody rb; // ������ٵ�(Rigidbody)

    [SerializeField] PlayerController player;

    public BossState state = BossState.IDLE;


    [SerializeField] bool EnfPhased = false;

    //public ParticleSystem rollAttack;
    public StateMachine stateMachine;

    [SerializeField] public GameObject projectile;
    [SerializeField] public GameObject bullets;
    [SerializeField] public GameObject windStorm;

    public Transform[] spawnPositions;
    [SerializeField] public float shootSpeed = 800.0f;
    [SerializeField] public float spreadRange = 100.0f;

    private GameObject HP_HUD_Obj; // ������ ü���� ��Ÿ���� �г�(Panel)
    [SerializeField] private Slider HPSlider; // �г� ���� �����̴�

    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashSkill = Animator.StringToHash("animation");
    readonly int hashGroggy = Animator.StringToHash("IsGroggy");
    readonly int hashDie = Animator.StringToHash("IsDie");

    #region ��ų ���
    [SerializeField]
    float mstATK = 10.0f;
    float mstSPD = 10.0f;

    public bool isDie = false;

    int i = 0;
    #endregion

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        playerTrf = player.transform;
        agent = GetComponent<NavMeshAgent>();
        enemyTrf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(BossState.IDLE, new IdleState(this));
        stateMachine.AddState(BossState.PATTERN, new PartternState(this));
        stateMachine.AddState(BossState.ATTACK, new AttackState(this));
        stateMachine.AddState(BossState.DEAD, new DeadState(this));
        stateMachine.InitState(state);

        agent.destination = playerTrf.position;
    }


    void Start()
    {
        // ü�� �г��� �ʱ�ȭ(����)�Ѵ�.
        HP_HUD_Obj = Instantiate(Resources.Load<GameObject>("HP_HUD"), transform);
        HPSlider = HP_HUD_Obj.GetComponentInChildren<UnityEngine.UI.Slider>();

        // �����̴��� ���� (���� ü�� / �ִ� ü��)���� �Ѵ�.
        //HPSlider.value = curHP / maxHP;

        StartCoroutine(BossPattern());
    }

    protected virtual IEnumerator BossPattern()
    {
        while (!isDie)
        {
            if (Input.GetKeyDown("1"))
            {
                i = 1;
                stateMachine.ChangeState(BossState.PATTERN);
            }
            else if (Input.GetKeyDown("2"))
            {
                i = 2;
                stateMachine.ChangeState(BossState.PATTERN);
            }
            else if (Input.GetKeyDown("3"))
            {
                i = 3;
                stateMachine.ChangeState(BossState.PATTERN);
            }
            else if (Input.GetKeyDown("4"))
            {
                stateMachine.ChangeState(BossState.ATTACK);
            }
            else if (Input.GetKeyDown("5"))
            {
                i = 5;
                stateMachine.ChangeState(BossState.IDLE);
            }
            else if (Input.GetKeyDown("6"))
            {
                i = 6;
                stateMachine.ChangeState(BossState.IDLE);
            }
            else
            {
                stateMachine.ChangeState(BossState.IDLE);
            }

            if (curHP <= 0)
            {
                stateMachine.ChangeState(BossState.DEAD);
            }
            yield return new WaitForSeconds(0.01f);
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
            // ������
            if (owner.i == 5)
            {
                owner.animator.SetBool(owner.hashAttack, false);
                owner.i = 0;
            }

            //�׷α�
            else if (owner.i == 6)
            {
                owner.animator.SetTrigger(owner.hashGroggy);
                owner.i = 0;
            }
            else
            {
                owner.agent.isStopped = false;
                owner.agent.SetDestination(owner.playerTrf.position);
            }
            //owner.animator.SetBool(owner.hashAttack, false);
        }
    }
    
    

    class AttackState : BaseEnemyState
    {
        public AttackState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.Attack();
        }
    }
    class PartternState : BaseEnemyState
    {
        public PartternState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            if (owner.i == 1)
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

            owner.agent.isStopped = true;
            owner.animator.SetBool(owner.hashAttack, false);
        }

    }

    class DeadState : BaseEnemyState
    {
        public DeadState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.animator.SetTrigger(owner.hashDie);
            owner.isDie = true;
        }
    }
    
    #region Attack
    void Attack()
    {
        animator.SetBool(hashAttack, true);
    }
    void Attack_Event()
    {
        StartCoroutine(Attack_crt());
    }
    IEnumerator Attack_crt()
    {
        float distance;

        distance = Vector3.Distance(playerTrf.position, enemyTrf.position);

        for (int i = 0; i < 2; i++)
        {
            GameObject pd1 = Instantiate(bullets, spawnPositions[0].position, Quaternion.identity);
            GameObject pd2 = Instantiate(bullets, spawnPositions[1].position, Quaternion.identity);
            pd1.transform.LookAt(playerTrf.localPosition);
            pd1.GetComponent<Rigidbody>().AddForce(pd1.transform.forward * shootSpeed);
            pd1.GetComponent<Rigidbody>().AddForce(pd1.transform.up * 10.0f);
            yield return new WaitForSeconds(0.2f);
            pd2.transform.LookAt(playerTrf.localPosition);
            pd2.GetComponent<Rigidbody>().AddForce(pd2.transform.forward * shootSpeed);
            pd2.GetComponent<Rigidbody>().AddForce(pd2.transform.up * 10.0f);

            yield return new WaitForSeconds(0.2f);
        }

        animator.SetBool(hashAttack, false);
        yield return new WaitForSeconds(1.0f);
    }
    #endregion

    
    #region skill1

    void Skill1()
    {
        animator.SetInteger(hashSkill, 1);
    }

    void Skill_1_Event()
    {
        StartCoroutine(Skill_1_crt());
    }

    IEnumerator Skill_1_crt()
    {
        Quaternion q = Quaternion.Euler(new Vector3(0, 20, 0));
        int percent = 0;
        if (EnfPhased)
        {
            percent = Random.Range(0, 100);
        }

        if (percent < 40)
        {
            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    float range = 1.0f;
                    float posX = Random.Range(-range, range);
                    float posY = Random.Range(-range, range);

                    GameObject pd1 = Instantiate(projectile, spawnPositions[0].position + new Vector3(posX * 2f, 0, posY * 2f), Quaternion.LookRotation(gameObject.transform.forward) * q);

                    pd1.GetComponent<Rigidbody>().AddForce(posX * spreadRange * pd1.transform.up);
                    pd1.GetComponent<Rigidbody>().AddForce(posY * spreadRange * pd1.transform.right);
                    pd1.GetComponent<Rigidbody>().AddForce(shootSpeed * Random.Range(0.5f, 1.8f) * pd1.transform.forward);
                }
                yield return new WaitForSeconds(0.1f);
            }
            animator.SetInteger(hashSkill, 0);
        }

        else 
        {
            GameObject pd = Instantiate(windStorm, transform.position, Quaternion.LookRotation(-gameObject.transform.right) * q);

            animator.SetInteger(hashSkill, 0);
            yield return new WaitForSeconds(5.0f);
            Destroy(pd);
        }   
    }

    #endregion

    #region Skill2

    void Skill2()
    {
        animator.SetInteger(hashSkill, 2);
    }
    void SKill_2_Event()
    {
        StartCoroutine(Skill_2_crt());
    }

    IEnumerator Skill_2_crt()
    {
        int percent = 0;
        if (EnfPhased)
        {
            percent = Random.Range(0, 100);
        }

        if (percent < 70)
        {
            // ���͸� ��ȯ����� ��... �̰� �־�� �÷�� �������� ������ ����.
            // ��ȭ������ ��Ÿ���� �־���� �ϴ°�? �ϴ� ��?
        }
        else
        {
            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < 10; i++)
                {
                    float range = 1.0f;
                    float posX = Random.Range(-range, range);
                    float posY = Random.Range(-range, range);

                    GameObject pd = Instantiate(projectile, spawnPositions[0].position + new Vector3(posX * 2f, 0, posY * 2f), Quaternion.identity) as GameObject;

                    pd.GetComponent<Rigidbody>().AddForce(posX * spreadRange * Vector3.forward);
                    pd.GetComponent<Rigidbody>().AddForce(posY * spreadRange * Vector3.right);
                    pd.GetComponent<Rigidbody>().AddForce(shootSpeed * Random.Range(0.8f, 1.2f) * Vector3.up);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        animator.SetInteger(hashSkill, 0);
    }
    #endregion

    #region Skill3

    public void Skill3()
    {
        animator.SetInteger(hashSkill, 3);
    }
    void SKill_3_Event()
    {
        StartCoroutine(Skill_3_crt());
    }

    IEnumerator Skill_3_crt()
    {
        Vector3[] vector3s = new Vector3[4];

        vector3s[0] = new Vector3(2, 1, 0);
        vector3s[1] = new Vector3(0, 1, -2);
        vector3s[2] = new Vector3(-2, 1, 0);
        vector3s[3] = new Vector3(0, 1, 2);

        List<GameObject> pds = new List<GameObject>();

        if (EnfPhased)
        {
            
            for (int i = 0; i < 8; i++)
            {
                pds.Add(Instantiate(windStorm, gameObject.transform.position, gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, i * 45, 0))));
            }
        }

        for (int k = 0; k < 10; k++)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject pd = Instantiate(bullets, gameObject.transform.position + vector3s[i], gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, i * 90, 0)));
                pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed * 0.4f);
            }
            yield return new WaitForSeconds(0.4f);
        }

        if (EnfPhased)
        {
            for (int i = 0; i < 8; i++)
            {
                Destroy(pds[i]);
            }
        }
        
        animator.SetInteger(hashSkill, 0);
    }
    #endregion

    // ������ �޴°� ����
    // ���� �� �� ����.

    public void GetDamage(int damage)
    {
        // ���� ���°� �ƴ� ���,
        if (!isInvincible)
        {
            // �������ŭ ü���� ���ҽ�Ų��.
            curHP -= damage;

            // ������ ü���� ü�� �гο� �����Ѵ�.
            HPSlider.value = curHP / maxHP;

            // ü���� 0 ������ ���(����), �� ���͸� ��Ȱ��ȭ�Ѵ�.
            
        }
    }
}
