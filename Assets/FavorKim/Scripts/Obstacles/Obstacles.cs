using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Obstacles : MonoBehaviour
{
    public enum Type
    {
        NONE, LEAF = 10, FIRE = 20, CUTTER
    }

    /*
    ������ ��ų ���� ������ Ÿ���� �����־
    �̰ɷ� �÷������� ��ֹ� ����� �غ��ϴ� ����
    ���� ��ֹ��� CUTTER�� FIRE�� �غ�
    FIRE, CUTTER >> LEAF
    ���� Ÿ���� �ϴ� ���Ŀ�.
    */

    ParticleSystem ps;
    [SerializeField] private int damage;
    [SerializeField] protected Type type;
    public int Damage { get { return damage; } }
    public Type GetObsType() { return type; }

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

    public virtual void OnTypeAttacked(Type attackedType) { }

}
