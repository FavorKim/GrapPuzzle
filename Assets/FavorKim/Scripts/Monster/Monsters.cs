using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monsters : MonoBehaviour, ITyped
{
    [SerializeField] float curHP;
    [SerializeField] float maxHP;
    float invincibleTime = 1.0f;

    bool isInvincible = false;

    [SerializeField] Slider HPSlider;
    GameObject HPHUDObj;

    
    public ITyped.Type type { get; protected set; }

    public float GetHP() { return curHP; }


    private void Start()
    {
        HPHUDObj = Instantiate(Resources.Load<GameObject>("HP_HUD"), transform);
        HPSlider = HPHUDObj.GetComponentInChildren<Slider>();
        HPSlider.value = curHP/maxHP;
    }

    /// <summary>
    /// InitSkill ��üȭ(Skill1 ��Ÿ��, Skill2 ��Ÿ��) ��ų ������ Awake ����α�
    /// </summary>
    public abstract void Awake();

    /// <summary>
    /// ���� �߻��Լ�
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// ù��° ��ų �����Լ� (�θ�� �����̹Ƿ� ��ų�� ����� ���̶�� ä������ ��)
    /// </summary>
    public virtual void Skill1() { }

    /// <summary>
    /// �ι�° ��ų �����Լ� (�θ�� �����̹Ƿ� ��ų�� ����� ���̶�� ä������ ��)
    /// </summary>
    public virtual void Skill2() { }

    /// <summary>
    /// ��ų �ʱ�ȭ (��ų�� 2���� ���)
    /// </summary>
    /// <param name="firstCoolTime">ù��° ��ų�� ��Ÿ�� (��Ÿ�� ������ 0)</param>
    /// <param name="secondCoolTime">�ι�° ��ų�� ��Ÿ�� (��Ÿ�� ������ 0)</param>
    public void InitSkill(float firstCoolTime, float secondCoolTime)
    {
        skill1 = new Skill(firstCoolTime, Skill1);
        skill2 = new Skill(secondCoolTime, Skill2);
    }
    /// <summary>
    /// ��ų �ʱ�ȭ (��ų�� 1���� ���)
    /// </summary>
    /// <param name="firstCoolTime">��ų ��Ÿ�� (��Ÿ�� ������ 0)</param>
    public void InitSkill(float firstCoolTime)
    {
        skill1 = new Skill(firstCoolTime, Skill1);
    }

    /// <summary>
    /// ���� �ǰ� �Լ�
    /// </summary>
    /// <param name="dmg">������ ���ݷ�</param>
    public void GetDamage(int dmg)
    {
        if (!isInvincible)
        {
            curHP -= dmg;
            HPSlider.value = curHP / maxHP;
            StartCoroutine(CorInvincible());
            if (curHP <= 0)
                gameObject.SetActive(false);
        }
    }


    public virtual void OnTypeAttacked(Obstacles attacker)
    {
        if ((int)attacker.type > (int)type)
            GetDamage(attacker.Damage * 2);
        else if ((int)attacker.type == (int)type)
            GetDamage(attacker.Damage);
        else
            GetDamage(attacker.Damage / 2);
    }

    IEnumerator CorInvincible()
    {
        isInvincible = true;
        float org = invincibleTime;
        while (true)
        {
            yield return null;
            invincibleTime -= Time.deltaTime;
            if (invincibleTime < 0)
            {
                isInvincible = false;
                invincibleTime = org;
                StopCoroutine(CorInvincible());
                break;
            }
        }
    }
    public Skill skill1;
    public Skill skill2;

    /// <summary>
    /// ��ų�� ��ų UI�� ���
    /// </summary>
    public void SetSkill()
    {
        if (skill1 == null) return;
        SkillManager.SetSkill(skill1, 1);
        if (skill2 == null) return;
        SkillManager.SetSkill(skill2, 2);
    }
}
