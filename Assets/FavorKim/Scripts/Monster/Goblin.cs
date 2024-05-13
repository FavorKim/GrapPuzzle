using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monsters
{
    public override void InitSkill()
    {
        skill1 = new Skill("��� ��ų1", 7, Skill1);
        skill2 = new Skill("��� ��ų2", 10, Skill2);
    }

    public override void Move()
    {
        Debug.Log("��� �̵�");
    }

    public override void Attack()
    {
        Debug.Log("��� ����");
    }

    public override void Skill1()
    {
        Debug.Log("��� ��ų 1");

    }
    public override void Skill2()
    {
        Debug.Log("��� ��ų 2");
    }

    public override void SetSkill()
    {
        SkillManager.SetSkill(skill1, 1);
        SkillManager.SetSkill(skill2, 2);
    }
}
