using UnityEngine;

public class CharacterGroundState : CharacterBaseState
{
    public CharacterGroundState(CharacterStateMachine context, CharacterStateFactory factory) 
        : base(context, factory) { }
    
    public override void Enter()
    {
        Debug.Log("Ground state enter");
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
        if (Context.DirectionX != 0)
        {
            SwitchState(Factory.Move());
        }
        if (Context.IsJumpPressed)
        {
            SwitchState(Factory.Jump());
        }
    }

    public override void InitSubState()
    {
    }
    
}