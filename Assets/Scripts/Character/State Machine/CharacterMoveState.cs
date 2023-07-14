using UnityEngine;

public class CharacterMoveState : CharacterBaseState
{
    public CharacterMoveState(CharacterStateMachine context, CharacterStateFactory factory) : base(context, factory)
    {
    }

    public override void Enter()
    {
        Debug.Log("Move state");
    }

    public override void UpdateState()
    {
        CheckSwitches();
    }

    public override void Exit()
    {
        
    }

    public override void CheckSwitches()
    {
        if (Context.IsJumpPressed)
        {
            SwitchState(Factory.Jump());
        }
    }

    public override void InitSubState()
    {
        throw new System.NotImplementedException();
    }

    private void Move()
    {
        
    }
}