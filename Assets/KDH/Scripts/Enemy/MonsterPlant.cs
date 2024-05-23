using System.Collections;
using UnityEngine;

public class MonsterPlant : BaseMonster
{

    #region Fields

    // MonsterPlant���� ���� ������Ʈ(Projectile)
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform spawnPosition;
    private float shootSpeed = 800.0f;

    // �ִϸ������� �ؽ�(Hash)
    private readonly int hashSkill2 = Animator.StringToHash("IsSkill2");

    #endregion Fields

    #region Override Methods

    // ��ų �ʱ�ȭ �Լ�
    protected override void InitSkills()
    {
        mstATK = 10.0f;
        mstSPD = 10.0f;

        attackCooltime = 2.0f;
        skill1Cooltime = 3.0f;
        skill2Cooltime = 3.0f;

        traceDistance = 20f;
        skillDistance = 10f;
        attackDistance = 2f;

        InitSkill(skill1Cooltime, skill2Cooltime);
    }

    // ��ų 1 �Լ�
    public override void Skill1()
    {
        StartCoroutine(ProjectileAttack());
        
    }
    private IEnumerator ProjectileAttack()
    {
        if (agent.isActiveAndEnabled)
            agent.isStopped = true;

        float distance;
        animator.SetBool(hashAttack, true);
        GameObject pd = Instantiate(projectile, spawnPosition.position, spawnPosition.rotation);

        if (!isPlayer)
        {
            distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
            pd.transform.LookAt(playerTrf.localPosition);
        }
        else
        {
            distance = 30.0f;
            pd.transform.LookAt(spawnPosition.position + new Vector3(0, 0, 10));
        }

        if (transform.parent != null)
        {
            pd.transform.LookAt(GameManager.Instance.Player.GetLookAt());
        }
        //pd.transform.LookAt(playerTrf.localPosition);
        //addforce ��ġ ������� ��.
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.up * distance * 15.5f);
        skill1_curCooltime = skill1Cooltime;

        yield return new WaitForSeconds(0.2f);
        animator.SetBool(hashAttack, false);
    }

    // ��ų 2 �Լ�
    public override void Skill2()
    {
        animator.SetTrigger(hashSkill2);
        skill2_curCooltime = skill2Cooltime;
    }

    #endregion Override Methods
}
