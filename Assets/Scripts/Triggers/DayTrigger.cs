using System;

public class DayTrigger : Trigger
{
    public override event Action Triggered;

    public void Trigger()
    {
        Triggered?.Invoke();
    }
}