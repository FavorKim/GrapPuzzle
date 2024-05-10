using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    //State�� ��ų�̶� ������ �з��ϰ�, 

    enum MonsterState
    { 
        Idle,
        Chase,
        Attack
    }

    public StateMachine stateMachine;

    private Dictionary<MonsterState, IState> dicState = new Dictionary<MonsterState, IState>();

    // Start is called before the first frame update
    void Start()
    {
        IState idle = new IdleState();

        dicState.Add(MonsterState.Idle, idle);
        dicState.Add(MonsterState.Chase, gameObject.AddComponent<ChaseState>());
        dicState.Add(MonsterState.Attack, gameObject.AddComponent<AttackState>());

        stateMachine = new StateMachine(idle);
    }
    /// <summary>
    /// ������ State�� �������� ��
    /// </summary>
    /// <param name="state"></param>
    public void setMonsterState(IState state)
    {
        stateMachine.SetState(state);
    }
}

public class StateMachine
{
    public IState currentState { get; private set; }

    /// <summary>
    /// �⺻ ���� �� �����ڸ� ����� ��.
    /// </summary>
    /// <param name="defaultState"></param>
    public StateMachine(IState defaultState)
    {
        currentState = defaultState;
    }

    public void SetState(IState state)
    {
        //���� ��ȭ ���� ��, ����ó��
        if (currentState == state)
        {
            Debug.Log("���� �̹� �ش� �����Դϴ�.");
            return;
        }

        currentState.StateEnd();

        currentState = state;

        currentState.StateEnter();
    }
}
public class IdleState : MonoBehaviour, IState
{
    //Idle���� ���ߴ� �Ŷ� ��Ʈ�ѱ��� ����
    public void StateEnter() { }
    public void StateUpdate() { }
    public void StateEnd() { }
}
public class ChaseState : MonoBehaviour, IState
{
    //Chase���� �߰ݸ�
    public void StateEnter() { }
    public void StateUpdate() { }
    public void StateEnd() { }
}

public class AttackState : MonoBehaviour, IState
{
    //Attack���� ����, ��ų1, ��ų2 �߿� ��Ȳ���� �� ��.
    public void StateEnter() { }
    public void StateUpdate() { }
    public void StateEnd() { }
}

public interface IState
{
    void StateEnter();
    void StateUpdate();
    void StateEnd();
}