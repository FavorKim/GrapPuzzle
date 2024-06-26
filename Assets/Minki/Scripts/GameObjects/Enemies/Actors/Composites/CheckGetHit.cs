using BehaviourTree;
using UnityEngine;

namespace Enemy
{
    // 적이 플레이어에게 피격했는지를 판별하는 클래스
    public class CheckGetHit : Node
    {
        // 적(Enemy) 클래스
        private readonly Enemy _enemy;

        // 생성자
        public CheckGetHit(Enemy enemy)
        {
            _enemy = enemy;
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 피격 상태일 경우 성공 상태를, 아닐 경우 실패 상태를 반환한다.
            state = (_enemy.IsGetHit) ? NodeState.SUCCESS : NodeState.FAILURE;
            return state;
        }
    }
}
