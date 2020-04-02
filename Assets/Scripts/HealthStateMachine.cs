﻿using System;
using UnityEngine;

class HealthyState : IState<HealthData>
{
    public HealthyState(HealthStateMachine stateMachine, ref HealthData data) : base(stateMachine, ref data)
    {
        m_data.m_duration = 0f;
    }
}

class CoughingState : IState<HealthData>
{
    public CoughingState(HealthStateMachine stateMachine, ref HealthData data) : base(stateMachine, ref data)
    {
        m_randomizer = new WeightedRandomizer<IState<HealthData>>();
        m_randomizer.AddWeight(new MenuEntry<IState<HealthData>, int>(new DeadState(stateMachine, ref data), m_data.m_config.cDeadWeight));
        m_randomizer.AddWeight(new MenuEntry<IState<HealthData>, int>(new RecoveredState(stateMachine, ref data), 100 - m_data.m_config.cDeadWeight));
    }

    public override void Update()
    {
        base.Update();

        m_data.m_duration += Time.deltaTime;

        if (m_data.m_duration > m_data.m_config.cCoughDuration)
        {
            // switch to next state
            ChangeState(m_randomizer.GetNextItem());
        }
    }

    WeightedRandomizer<IState<HealthData>> m_randomizer;
}

class DeadState : IState<HealthData>
{
    public DeadState(HealthStateMachine stateMachine, ref HealthData data) : base(stateMachine, ref data)
    {
    }
}

class RecoveredState : IState<HealthData>
{
    public RecoveredState(HealthStateMachine stateMachine, ref HealthData data) : base(stateMachine, ref data)
    {
    }
}

public class HealthConfig
{
    public readonly float cCoughDuration = 10f;
    public readonly int cDeadWeight = 20; // 0 - 100
}

public class HealthData
{
    public HealthData(HealthConfig config)
    {
        m_config = config;
    }

    public HealthConfig m_config;
    public float m_duration = 0f;
}

public partial class HealthStateMachine : StateMachine<HealthData>
{
    public HealthStateMachine()
    {
        m_data = new HealthData(new HealthConfig());

        ChangeState(new HealthyState(this, ref m_data));

    }

    public void InfectWithCough()
    {
        // infect only if the human is healthy
        if (m_currentState is HealthyState)
        {
            ChangeState(new CoughingState(this, ref m_data));
        }
    }

    internal void Infect(in HealthStateMachine otherHealthState)
    {
        /*
        * 1 - Healthy, 0 - Infected
        * M - This/Me
        * O - Other
        * 
        * M    1    0    1    0
        * O    1    0    0    1
        *      N    N    M0   Oo
        */

        // if I am not healthy, I cannot be infected again
        if ((m_currentState is HealthyState) && !(otherHealthState.m_currentState is HealthyState))
        {
            ChangeState(new CoughingState(this, ref m_data));
        }
    }
}



