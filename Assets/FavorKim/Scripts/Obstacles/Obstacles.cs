using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Obstacles : MonoBehaviour, ITyped
{

    /*
    ������ ��ų ���� ������ Ÿ���� �����־
    �̰ɷ� �÷������� ��ֹ� ����� �غ��ϴ� ����
    ���� ��ֹ��� CUTTER�� FIRE�� �غ�
    FIRE, CUTTER >> LEAF
    ���� Ÿ���� �ϴ� ���Ŀ�.
    */

    ParticleSystem ps;
    [SerializeField] private int damage;
    [SerializeField] protected ITyped.Type myType;

    public ITyped.Type type { get { return myType; } set { } }
    
    public int Damage { get { return damage; } }

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        if (ps == null) return;
        ParticleSystem.CollisionModule col = ps.collision;
        col.enabled = true;
        col.type = ParticleSystemCollisionType.World;
        col.sendCollisionMessages = true;
    }

    
    private void OnParticleCollision(GameObject other)
    {
        GameManager.Instance.GetDamage(this, other);
    }

    public virtual void OnTypeAttacked(Obstacles attackedType) { }

    private void OnTriggerStay(Collider other)
    {
        GameManager.Instance.GetDamage(this, other.gameObject);
    }
}
