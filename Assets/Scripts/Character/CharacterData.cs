using UnityEngine;

public static class CharacterData
{
    public static class Animations
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int StartFall = Animator.StringToHash("StartFall");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int DashPreparation = Animator.StringToHash("DashPreparation");
        private static readonly int Dash = Animator.StringToHash("Dash");
        private static readonly int Slide = Animator.StringToHash("Slide");
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Grab = Animator.StringToHash("Grab");

        public static int GetAnimationHash(States state)
        {
            const States IdleState = States.Idle;
            const States DieState = States.Die;
            const States WalkState = States.Walk;
            const States JumpState = States.Jump;
            const States SlideState = States.Slide;
            const States FallState = States.Fall;
            const States GrabState = States.Grab;
            const States DashPreparationState = States.DashPreparation;
            const States DashState = States.Dash;

            return state switch
            {
                IdleState => Idle,
                DieState => Hurt,
                GrabState => Grab,
                DashPreparationState => DashPreparation,
                SlideState => Slide,
                DashState => Dash,
                JumpState => Jump,
                WalkState => Walk,
                FallState => StartFall,
                _ => Idle
            };
        }
    }
    
    public enum States
    {
        Idle = 1,
        Walk = 2,
        Jump = 3,
        Fall = 5,
        Slide = 6,
        DashPreparation = 7,
        Dash = 8,
        Die = 9,
        Grab = 10
    }
}