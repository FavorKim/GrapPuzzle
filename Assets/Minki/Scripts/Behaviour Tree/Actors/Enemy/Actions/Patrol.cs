using BehaviourTree;

namespace Enemy
{
    // ���� ������ �����ϴ� Ŭ����
    public class Patrol : Node
    {
        // ��(Enemy) Ŭ����
        private Enemy _enemy;

        // ������
        public Patrol(Enemy enemy)
        {
            _enemy = enemy;
        }

        // �� �Լ�
        public override NodeState Evaluate()
        {
            DoPatrol();

            // ������ ������ ��, �� ������ �ൿ�� �����Ѵ�.
            return NodeState.SUCCESS;
        }

        // ������ ����ϴ� �Լ�
        public void DoPatrol()
        {
            // ���� �Լ��� �����Ѵ�.
            _enemy.Patrol();
        }
    }
}
