using UnityEngine;

public abstract class CharacterBaseState
{
    protected CharacterStateMachine Context;
    protected CharacterStateFactory Factory;

    public CharacterBaseState(CharacterStateMachine context, CharacterStateFactory factory)
    {
        Context = context;
        Factory = factory;
    }
    
    public abstract void Enter();
    public abstract void UpdateState();
    public abstract void Exit();
    public abstract void CheckSwitches();
    public abstract void InitSubState();

    private void UpdateStates()
    {
    }

    protected void SwitchState(CharacterBaseState nextState)
    {
        Exit();
        nextState.Enter();
        Context.SetCurrentState(nextState);
    }

    protected void SetSuperState()
    {
    }

    protected void SetSubState()
    {
    }
}