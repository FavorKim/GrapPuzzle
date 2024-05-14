using BehaviourTree;
using UnityEngine;

namespace Enemy
{
    public class CheckNearToChase : Node
    {
        // ��(Enemy) Ŭ����
        private Enemy _enemy;

        // �ʵ�(Field)
        private float detectRange; // �÷��̾ Ž���ϴ� ����

        // ������
        public CheckNearToChase(Enemy enemy)
        {
            _enemy = enemy;
            detectRange = enemy._detectRange;
        }

        public override NodeState Evaluate()
        {
            float dis = Vector3.Distance(_enemy._enemyTransform.position, _enemy.GetPlayerTransform().position);
            Debug.Log($"Distance = {dis}");
            Debug.Log($"Detect Range = {detectRange}");

            // ���� �÷��̾� ������ �Ÿ��� ���� �̸�(Ž�� ���� ��)�� ���,
            if (Vector3.Distance(_enemy._enemyTransform.position, _enemy.GetPlayerTransform().position) <= detectRange)
            {
                Debug.Log("CheckNearToChase = Success");

                // ���� ���� �����Ѵ�.
                state = NodeState.SUCCESS;
                return state;
            }
            // �ƴ� ���,
            else
            {
                Debug.Log("CheckNearToChase = Failure");

                // �� ��带 ���� ó���Ѵ�.
                state = NodeState.FAILURE;
                return state;
            }
        }
    }
}
