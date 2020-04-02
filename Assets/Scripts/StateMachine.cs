using System;

public abstract class IState<T_DATA>
{
    public IState(StateMachine<T_DATA> stateMachine, ref T_DATA data)
    {
        m_stateMachine = stateMachine;
        m_data = data;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

    protected void ChangeState(IState<T_DATA> nextState)
    {
        m_stateMachine.ChangeState(nextState);
    }

    private StateMachine<T_DATA> m_stateMachine;
    protected T_DATA m_data;
}

public class StateMachine<T_DATA>
{
    protected IState<T_DATA> m_currentState;

    public void ChangeState(IState<T_DATA> newState)
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

    protected T_DATA m_data;
}
