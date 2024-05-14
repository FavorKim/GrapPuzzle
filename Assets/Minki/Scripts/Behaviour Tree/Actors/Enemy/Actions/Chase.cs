using BehaviourTree;

namespace Enemy
{
    // ���� ������ �����ϴ� Ŭ����
    public class Chase : Node
    {
        // ��(Enemy) Ŭ����
        private Enemy _enemy;

        // ������
        public Chase(Enemy enemy)
        {
            _enemy = enemy;
        }

        // �� �Լ�
        public override NodeState Evaluate()
        {
            DoChase();

            // ������ ������ ��, �� ������ �ൿ�� �����Ѵ�.
            return NodeState.SUCCESS;
        }

        // ������ ����ϴ� �Լ�
        public void DoChase()
        {
            // ���� �Լ��� �����Ѵ�.
            _enemy.Chase();
        }
    }
}
