using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITyped
{
    public enum Type
    {
        NONE, AQUA , LEAF = 10, FIRE = 20, CUTTER
    }
    Type type { get; }

    /// <summary>
    /// �Ӽ������� �޾��� ���� �ൿ
    /// </summary>
    /// <param name="type">������ �Ӽ�</param>
    public void OnTypeAttacked(Obstacles attacker);
}
