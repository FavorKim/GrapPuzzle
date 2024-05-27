using UnityEngine;

public class TestPossessingState : TestPlayerState
{
    private TestMonster monster;

    public TestPossessingState(TestPlayer playerController) : base(playerController) { }

    public override void Enter()
    {
        playerOutfit.SetActive(false);

        GameObject hatImg = playerController.GetHatManager().GetHatImg();
        hatImg.SetActive(true);

        if (monster.Skill01 != null)
        {
            SkillManager.SetSkill(monster.Skill01, 1);
        }
        else
        {
            SkillManager.socket1.gameObject.SetActive(false);
        }

        if (monster.Skill02 != null)
        {
            SkillManager.SetSkill(monster.Skill02, 2);
        }    
        else
        {
            SkillManager.socket2.gameObject.SetActive(false);
        }

        playerController.DurationGauge.gameObject.SetActive(true);
        playerController.DurationGauge.value = 1;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        // ���͸� �ڽĿ��� ������Ų��.
        monster.transform.parent = null;

        // ���� ������ ���� ����ϴ� UI�� �����Ѵ�.
        playerController.DurationGauge.gameObject.SetActive(false);
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
        // �������� ���� ���·� ��ȯ�Ѵ�.
        playerController.SetState("Normal");
    }

    public override void Attack()
    {
        monster.Attack();
    }

    public override void Skill01()
    {
        monster.Skill01?.UseSkill();
    }

    public override void Skill02()
    {
        monster.Skill02?.UseSkill();
    }

    



    public override void StateUpdate()
    {
        base.UpdateState();

        if (mon.GetHP() <= 0)
            player.SetState("Normal");
        if (mon.skill1 != null)
            mon.skill1.SetCurCD();
        if (mon.skill2 != null)
            mon.skill2.SetCurCD();
        SetDuration();
    }



    public void GetMonster(TestMonster monster)
    {
        this.monster = monster;

        monster.transform.SetParent(playerController.transform);
        monster.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        playerController.MoveSpeed = monster.MoveSpeed;
        playerController.JumpPower = monster.JumpPower;

        Enter();
    }
}
