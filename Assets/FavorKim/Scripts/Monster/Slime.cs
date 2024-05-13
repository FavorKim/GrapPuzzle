using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monsters
{

    private void Awake()
    {
        skill1 = new Skill("������ ��ų1", 5, Skill1);
        skill2 = new Skill("������ ��ų2", 3, Skill2);
    }

    public override void Move()
    {
        Debug.Log("������ �̵�");
    }

    public override void Attack()
    {
        Debug.Log("������ ����");
    }

    public override void Skill1()
    {
        Debug.Log("������ ��ų 1");

    }
    public override void Skill2()
    {
        Debug.Log("������ ��ų 2");
    }
    

}
