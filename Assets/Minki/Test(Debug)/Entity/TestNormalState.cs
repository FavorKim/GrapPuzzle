using UnityEngine;

public class TestNormalState : TestPlayerState
{
    public TestNormalState(TestPlayer playerController) : base(playerController) { }

    // �������� ���� ���·� ���� ��,
    public override void Enter()
    {
        // ��ų�� �ʱ�ȭ�Ѵ�.
        SkillManager.ResetSkill();

        playerController.MoveSpeed = moveSpeed;
        playerController.JumpPower = jumpPower;
    }

    public override void Exit()
    {
        playerOutfit.SetActive(false);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Move()
    {
        base.Move();
    }

    public override void Jump()
    {
        base.Jump();
    }

    public override void Shift()
    {
        playerController.GetComponent<Animator>().SetTrigger("Shift");
    }

    public override void Attack() { }

    public override void Skill01() { }

    public override void Skill02() { }
}
