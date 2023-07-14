using System;
using UnityEngine;

public class CharacterJumpState : CharacterBaseState
{
    public CharacterJumpState(CharacterStateMachine context, CharacterStateFactory factory) 
        : base(context, factory)
    {
    }

    public override void Enter()
    {
        Debug.Log("Jump state");
        Jump();
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
        if (Context.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitSubState()
    {
    }

    private void Jump()
    {
        var jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * 
            Context.Rigidbody.gravityScale * 10f);
        float cury = Context.Rigidbody.velocity.y + jumpSpeed;
        Context.Rigidbody.velocity = new Vector2(Context.Rigidbody.velocity.x, cury);
    }
}