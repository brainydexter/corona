
class HealthyState : IState
{
}

class CoughingState : IState
{
}

class DeadState : IState
{
}

class RecoveredState : IState
{
}

public class HealthStateMachine : StateMachine
{
    public HealthStateMachine()
    {
        ChangeState(new HealthyState());
    }

    internal void InfectWithCough()
    {
        // infect only if the human is healthy
        if(m_currentState is HealthyState)
        {
            ChangeState(new CoughingState());
        }
    }
}
