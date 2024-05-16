using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    //protected float moveSpeed;
    //protected GameObject VFXPrefab;
    //protected Animator anim;

    // ����Ʈ, �̵��ӵ�, �ִϸ����� ���... 

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

    public Skill skill1;
    public Skill skill2;

    /// <summary>
    /// ��ų�� ��ų UI�� ���
    /// </summary>
    public void SetSkill() 
    {
        if (skill1 == null) return;
        SkillManager.SetSkill(skill1, 1);
        if(skill2 == null) return;
        SkillManager.SetSkill(skill2, 2);
    }

    //public void InitAnim(Animator anim)
    //{
    //    this.anim = anim;
    //}


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Obstacles"))
    //        GameManager.Instance.Player.SetState("Normal");
    //}
}
