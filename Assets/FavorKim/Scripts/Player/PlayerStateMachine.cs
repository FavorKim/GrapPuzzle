using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateMachine
{
    public PlayerStateMachine(PlayerController controller)
    {
        possessState = new PossessState(controller);
        states.Add("Normal", new NormalState(controller));
        states.Add("Possess", possessState);
        curState = states["Normal"];

        //OnJump += curState.Jump;
        //OnAttack += curState.Attack;
        //OnThrowHat += curState.Skill1;
    }
    private PlayerState curState; // �븻

    private Dictionary<string, PlayerState> states = new Dictionary<string, PlayerState>();


    protected PossessState possessState;


    public void StateUpdate()
    {
        curState.Move();
        // �븻����
        

        curState.StateUpdate();

    }

    //public void StateFixedUpdate()
    //{
    //}

    public void StateOnJump() { curState.Jump(); }
    public void StateOnAttack() { curState.Attack(); }
    public void StateOnHat() { curState.Shift(); }
    public void StateOnSkill1() { curState.Skill1(); }
    public void StateOnSkill2() { curState.Skill2(); }


    public void ChangeState(string nextState)
    {
        curState.Exit();
        states[nextState].Enter();
        curState = states[nextState];
    }

    public void ChangeState(Monsters mon)
    {
        curState.Exit();
        possessState.GetMonster(mon);
        curState = possessState;
    }
}


public abstract class PlayerState : IState
{
    public PlayerState(PlayerController player)
    {
        this.player = player;
        stateCC = player.GetCC();
        moveSpeed = player.moveSpeed;
        gravityScale = player.gravityScale;
        jumpForce = player.jumpForce;
        anim = player.GetAnimator();
        //isGround = player.isGround;
    }

    protected PlayerController player;
    protected CharacterController stateCC;
    protected Animator anim;
    protected Monsters mon;

    protected Vector3 moveDir => player.MoveDir;

    protected float moveSpeed;
    protected float gravityScale;
    protected float jumpForce;

    protected bool isGround => player.isGround;

    public abstract void Move();

    public abstract void StateUpdate();

    public abstract void Jump();
    public abstract void Attack();
    public abstract void Skill1();
    public abstract void Skill2();
    public abstract void Shift();

    public abstract void Enter();
    public abstract void Exit();
}

public class NormalState : PlayerState
{

    public NormalState(PlayerController controller) : base(controller) { orgJumpForce = jumpForce; }

    float orgJumpForce;
    bool isJumping = false;

    public override void Enter()
    {
        SkillManager.ResetSkill();
    }

    public override void Move()
    {
        stateCC.Move(player.transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);

        if (isJumping)
        {
            NormJump();
        }
    }

    public override void StateUpdate()
    {
        stateCC.SimpleMove(-player.transform.up * gravityScale * Time.deltaTime);
    }

    public override void Jump()
    {
        if (!isGround) return;
        anim.SetTrigger("Jump");
        isJumping = true;
    }

    public override void Attack()
    {
        Debug.Log("atk");
    }

    public override void Skill1()
    {
    }

    public override void Skill2()
    {
        
    }

    public override void Shift()
    {
        anim.SetTrigger("Throw");

    }

    public override void Exit()
    {

    }

    void NormJump()
    {
        
        stateCC.Move(player.transform.up * jumpForce * Time.deltaTime);
        jumpForce *= 0.99f;
        if (jumpForce < 7)
        {
            jumpForce = orgJumpForce;
            isJumping = false;
        }
    }
}



public class PossessState : PlayerState
{
    public PossessState(PlayerController controller) : base(controller) { durationGauge = player.GetDurationGauge(); }
    Slider durationGauge;
    // MonsterController monCon;
    // MonsterController �ٸ� ���͵��� ����� �޾�
    // virtual(abstract) Move()
    // ������Ʈ�ѷ� : ������Ʈ�ѷ�
    // override move(){};


    // �ൿ ���� �޼��带 �븮�ڷ� �����ؼ� 


    // ���ʹ� Input���� �������� �ʴ´�.
    // �÷��̾�� Input���� �����δ�.


    /*
    �̵��� �ʿ��Ѱ� ����.

    - �̵�
    �÷��̾� : ���͸� �÷��̾�� ��ǲ���� ������ �ϰ� �̵�

    ���� : �÷��̾�� ������ ��ġ�� �������� ����� �ؼ� ����.
    
    Aió�� �� / �ൿ(���� ��)
    �ൿ �޼��尡 ���� ���� �Ű��� �޾�(�̵��̶��, ����)

    Move(vector3 dir);

    ��� + �̵�;
    
    MonsterAIMove() <- ���� �Ǳ� ��
    {
        MonsterMove(calcul());
    }

    ���ʹ� ���� �ǰ� ���� �� �� ���� <- ������

    ���ʹ� ���ǰ� ���� �ȵ��� ���� ��?
    -> �÷��̾ ���ڸ� ���Ϳ� ���߸� ���� ������Ʈ�� ��Ȱ��ȭ
      -> �̷��� ���ʹ� ���� �̵� �Լ��� ��?

    ��ǲ�� ��·�� �÷��̾� ���ο��� ���ͷ� �ٲ�� ��.

    vector3 calcul(); -> ���⼭ ������ ����

    void MonsterMove(Vector3 dir) -> �Ű��� ���޹��� ���Ͱ��� ���ؼ� �̵�
    
    
    ���� -> AI -> �ൿ


    void Attack(); <<< 

    �÷��̾�� �˻縦 �� �ص� ����?
    �˻� ����� Input
    �÷��̾�� ����Ű�� ������ �ִϸ��̼� ���ش�.
    
    �� Attack�� �ʿ��� ������. ��� ��� �ѱ����?
    animator << 

    ���Ͱ� �ڱ�Ŵϱ� �ڱⲬ �ִ�
    �÷��̾ �������׼� �������� ������ ������ �ۺ��̾����.
    
    ���� ������ �����̺��̰�, �Ѱ��ִ� �Լ��� �ۺ��̸�
    
    �÷��̾�� �Ұ����ѵ� ���ʹ� ������ �� (AI�� ���� �̵�)
     

    �÷��̾ ���Ϳ� ���� ������ �� �����ִٸ�, << ���� �Ŵ���(���Ϳ� ���� ������ �� �������ִ� �༮)
    �÷��̾�� ������ �ൿ ������ ����.
    �÷��̾ �����ϴ� ������Ʈ�� ��� �ٲ㼭 �ൿ�� �Ѵ�.

    
                  PlayerInputManager
    �÷��̾�� �÷��̾��� �ൿ�� ��ǲ��������� �ൿ�� �ϰ�
    ���ʹ� ������ �ൿ�� ��ǲ�� ������� �ൿ��Ű�� ��.
    
    ����� ħ ����
    ����Ŵ� �����̸� �ֵθ���.


    �÷��̾ ����� �����ϸ� �÷��̾�� ���ݹ�ư�� ������ ħ�� ��´�.
    ����Ÿ� �����̸� �ֵθ���.

    �÷��̾ ���� �� ȣ���� ���� ex)����ħ��� ������ ��ȭ�ؼ� �÷��̾�� �Ѱ��� ����.

    ���ʹ� ��� �÷��̾ �غ����� ���ϴ� �÷����� ������ �غ��� �� �־�
    �ٸ� �갡 �׷����� ���� ���� ��.
    �׷���? ������ �ִ� �÷��̾
    �׷��� �ϴ� ��.


    �� ó�� �������� ���ư��ڸ�

    ���ؾ��� ����.
    �÷��̾ ���ͷ� ������ ��
    ������ ����� ���ΰ�.

    �÷��̾� : ����, ��ų1, ��ų2, ����, �̵�, ���� ������(���� ��ų ��ư<- ���� ���� �� ���ѽð��� �� �����ʴ��� ���Ǹ� ������)
    ���� : ��׸� �� �� ������. �Ϻθ� �� ������ (�̵��� �׳� �÷��̾� �̵���� ��)

    �÷��̾ �������� ��. �÷��̾ ����� ������ �����?

    ����, ��ų1, ��ų2

    ������ �ൿ ������ �÷��̾ �˾Ƽ�

    Ŀ��
    �ڵ����� �Ǹ� ����, �ν���?
    ��� ������, ����

    ���� Ʋ. ����, ��ų1, ��ų2 (�߻�ȭ)
    ���� 1�� �߻�ȭ�� ���� �͵��� (��üȭ)
    
    �ٵ� �Ű��� ���� Ʋ
    �÷��̾�� ���Ϳ� ������ �ɾ �� ��ɵ��� ���°���
    
    






    
    �������ϵ� �ٸ� ��ũ��Ʈ
    �ʵ� ���ʹ� �ʵ� ���� ��ũ��Ʈ
    ���� ���ʹ� ���� ���� ����

    



     */

    /*
    1. ���Ϳ��Լ� ���ǿ� �ʿ��� ������ �����´�.
    �÷��̾ ���Ͱ� �ȴ�.
    
    �� -> X
    ���� X

    ������ �� ���� ���Ͱ� ������.
    �÷��̾ ������ �ٲ۴�. <- �̰͵� �� �� ������ ��
    �÷��̾ ������ ���������� �ִ�.

    ���͸� ��Ʈ�� <- ���� �̰� ó�� �����߾� �̰� �ϰ�;�����? ���� �� ����
    �ٵ�? ������.

    �÷��̾ ���Ͱ� �ȴ�. 
     */



    /*
    ���� �� �ʹ� �Ӹ� ���� ���߿� �������� �ؾ���...
    �Ϻθ� �����ϴ°�? <- �ϴ� �� �� ��...
    ���ڸ� ���̴°�? <- ���� ��
    ���� �� �Ϸ��� �ð� �� �־����.

    ��ó�� ���ڸ� ������ ��

    �÷��̾�� ������ � ���� ����ұ��?

    ex) ����, �̵�

    �����ϴ� ��ĵ� ���͸��� �ٸ��� <- ��
    
    ���� �Ŵ��� <- �̰� ���� �͵� ���� �� ������
    ���� �Ŵ����� ��� ������ ������ ������ �־
    �÷��̾� -> ���� �Ŵ���
    �÷��̾� : �� �������� ������
    ���� �Ŵ��� : �׷� �׷��� ���������� (��ų, �̵�, ���� ...) �ٰ� �̰� ���� �� �������

    �÷��̾� ���� (���� ��ȣ�ۿ��� å������ �༮)
     - �÷��̾� (������) ��
     - ���� 1 (������ ����1�� �ൿ�� ���� ��ũ��) ����<��ũ��Ʈ x ���� ���� + �ִϸ�����
    

    ���� 1�� �ʵ忡 ���ƴٴϴ� ���� 1�� ���ڸ� ���� ������ <- ��� AI��� ������.

    �׷��� ���� 1�� �÷��̾ �̵��ϴ� �� ó�� �̵��ϴ� ��ũ��Ʈ�� <- �̰� �̹� �÷��̾ �����Ǿ��ִµ�?
    AI�� �̵��ϴ� ��ũ��Ʈ�� �� �� ���� ���������ϳ�?

     - ���� 2 (������ ����2�� �ൿ�� ���� ��ũ��) ����
     - 

    ���� 1�� Input���� �� ������.

    ���� ���� �� 



    

    */

    public override void Enter()
    {
        /*
        �ִϸ��̼� ȣ��

         */
        mon.SetSkill();
        durationGauge.gameObject.SetActive(true);
        durationGauge.value = 1;

    }


    public void GetMonster(Monsters mon)
    {
        this.mon = mon;
        Enter();
    }
    

    public override void Move()
    {
        mon.Move();
    }

    public override void StateUpdate()
    {
        mon.skill1.SetCurCD();
        mon.skill2.SetCurCD();
        SetDuration();
    }

    public override void Jump()
    {
        // mon.Jump();
    }
    public override void Attack()
    {
        mon.Attack();
    }
    public override void Skill1()
    {
        
        mon.skill1.UseSkill();
    }
    public override void Skill2()
    {
        mon.skill2.UseSkill();
    }

    public override void Shift()
    {
        player.SetState("Normal");
    }

    public override void Exit()
    {
        mon = null;
        durationGauge.gameObject.SetActive(false);
    }

    void SetDuration()
    {
        durationGauge.value -= Time.deltaTime / player.GetDuration();
        if (durationGauge.value <= 0) Shift();
    }


}

public interface IState
{
    void Move();
    void Jump();
    void Enter();
    void Exit();
    void Attack();
    void Skill1();
    void Skill2();
    void Shift();

}
