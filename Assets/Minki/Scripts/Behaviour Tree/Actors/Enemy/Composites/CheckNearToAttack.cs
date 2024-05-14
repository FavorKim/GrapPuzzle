using BehaviourTree;
using UnityEngine;

namespace Enemy
{
    public class CheckNearToAttack : Node
    {
        // ��(Enemy) Ŭ����
        private Enemy _enemy;

        // �ʵ�(Field)
        private float _attackRange; // ������ �����ϴ� ����

        // ������
        public CheckNearToAttack(Enemy enemy)
        {
            _enemy = enemy;
            _attackRange = enemy._attackRange;
        }

        public override NodeState Evaluate()
        {
            float dis = Vector3.Distance(_enemy._enemyTransform.position, _enemy.GetPlayerTransform().position);
            Debug.Log($"Distance = {dis}");
            Debug.Log($"Attack Range = {_attackRange}");

            // ���� �÷��̾� ������ �Ÿ��� ���� �̸�(���� ���� ��)�� ���,
            if (Vector3.Distance(_enemy._enemyTransform.position, _enemy.GetPlayerTransform().position) <= _attackRange)
            {
                Debug.Log("CheckNearToAttack = Success");

                // ���� ���� �����Ѵ�.
                state = NodeState.SUCCESS;
                return state;
            }
            // �ƴ� ���,
            else
            {
                Debug.Log("CheckNearToAttack = Failure");

                // �� ��带 ���� ó���Ѵ�.
                state = NodeState.FAILURE;
                return state;
            }
        }
    }
}
