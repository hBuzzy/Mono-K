public class CharacterStateFactory
{
    private CharacterStateMachine _context;

    public CharacterStateFactory(CharacterStateMachine currentContext)
    {
        _context = currentContext;
    }

    public CharacterBaseState Idle()
    {
        return new CharacterIdleState(_context, this);
    }


    public CharacterBaseState Grounded()
    {
        return new CharacterGroundState(_context, this);
    }

    public CharacterBaseState Jump()
    {
        return new CharacterJumpState(_context, this);
    }

    public CharacterBaseState Move()
    {
        return new CharacterMoveState(_context, this);
    }
}