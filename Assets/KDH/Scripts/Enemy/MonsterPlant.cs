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

        traceDistance = 10f;
        skillDistance = 10f;
        attackDistance = 2f;

        InitSkill(skill1Cooltime, skill2Cooltime);
    }

    // ���� �Լ�
    public override void Attack()
    {
        animator.SetBool(hashAttack, true);
        attack_curCooltime = attackCooltime;
    }

    // ��ų 1 �Լ�
    public override void Skill1()
    {
        float distance;

        if (!isPlayer)
        {
            distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
        }
        else
        {
            distance = 3.0f;
        }

        GameObject pd = Instantiate(projectile, spawnPosition.position, Quaternion.identity) as GameObject;
        pd.transform.LookAt(playerTrf.localPosition);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.up * distance * 15.5f);

        //���⼭ ��ų�� �߻� ����� �ϰŵ�?
        skill1_curCooltime = skill1Cooltime;

        animator.SetBool(hashAttack, true);
    }

    // ��ų 2 �Լ�
    public override void Skill2()
    {
        animator.SetTrigger(hashSkill2);
        skill2_curCooltime = skill2Cooltime;
    }

    #endregion Override Methods
}
