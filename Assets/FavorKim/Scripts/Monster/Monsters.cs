using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    protected float moveSpeed;
    protected GameObject VFXPrefab;
    protected Animator anim;

    // ����Ʈ, �̵��ӵ�, �ִϸ����� ���...

    private void Awake()
    {
        InitSkill();
    }

    public abstract void Attack();

    public abstract void Skill1();
    public abstract void Skill2();

    public abstract void Move();

    public abstract void InitSkill();

    public Skill skill1;
    public Skill skill2;

    public abstract void SetSkill();
}
