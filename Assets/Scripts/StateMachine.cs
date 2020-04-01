using System;

public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

public class StateMachine
{
    protected IState m_currentState;

    public void ChangeState(IState newState)
    {
        if (m_currentState != null)
            m_currentState.Exit();

        m_currentState = newState;
        m_currentState.Enter();
    }

    public void Update()
    {
        if (m_currentState != null) 
            m_currentState.Update();
    }
}
