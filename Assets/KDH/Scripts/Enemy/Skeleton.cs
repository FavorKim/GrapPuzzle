using System.Collections;
using UnityEngine;

public class Skeleton : BaseMonster
{

    #region Fields
    // MonsterPlant���� ���� ������Ʈ(Projectile)
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootSpeed = 800.0f;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private ParticleSystem stabAttack;

    // �ִϸ������� �ؽ�(Hash)
    private readonly int hashSkill1 = Animator.StringToHash("IsSkill1");
    private readonly int hashSkill2 = Animator.StringToHash("IsSkill2");

    #endregion Fields

    #region Override Methods

    // ��ų �ʱ�ȭ �Լ�
    protected override void InitSkills()
    {
        mstATK = 10.0f;
        mstSPD = 10.0f;

        attackCooltime = 1.0f;
        skill1Cooltime = 3.0f;
        skill2Cooltime = 3.0f;
        
        traceDistance = 10f;
        skillDistance = 8f;
        attackDistance = 2f;

        InitSkill(skill1Cooltime, skill2Cooltime);
    }

    // ��ų 1 �Լ�
    public override void Skill1()
    {
        skill1_curCooltime = skill1Cooltime;
        
        StartCoroutine(SlashAttack());

        IEnumerator SlashAttack()
        {
            if (agent.isActiveAndEnabled)
                agent.isStopped = true;

            animator.SetBool(hashSkill1, true);
            GameObject pd = Instantiate(projectile, spawnPosition.position, spawnPosition.rotation);

            if(transform.parent != null)
            {
                pd.transform.LookAt(GameManager.Instance.Player.GetLookAt());
            }
            //pd.transform.LookAt(playerTrf.localPosition);
            //addforce ��ġ ������� ��.
            pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed);
           
            yield return new WaitForSeconds(0.2f);
            animator.SetBool(hashSkill1, false);
        }
    }

    // ��ų 2 �Լ�
    public override void Skill2()
    {
        animator.SetBool(hashSkill2, true);
    }

    void SAttack()
    {
        StartCoroutine(StabAttack());
    }

    IEnumerator StabAttack()
    {
        if(agent.isActiveAndEnabled)
            agent.isStopped = true;

        ParticleSystem ps = Instantiate(stabAttack, this.transform);
        ps.transform.position = spawnPosition.position;
        
        rb.AddRelativeForce(Vector3.forward * 20f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.4f);

        Destroy(ps.gameObject);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        skill2_curCooltime = skill2Cooltime;
        
        animator.SetBool(hashSkill2, false);
    }

    #endregion Override Methods
}
