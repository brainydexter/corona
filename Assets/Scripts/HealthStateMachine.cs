using System;
using System.Collections.Generic;
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

public class HealthStateMachine : StateMachine<HealthData>
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
}

[System.Serializable]
public class MenuEntry<K, T> : IEquatable<K>
{
    public MenuEntry(K key, T value)
    {
        evnt = key;
        resource = value;
    }

    public K evnt;
    public T resource;

    public K KEY
    {
        get { return evnt; }
        set { evnt = value; }
    }

    public T VALUE
    {
        get { return resource; }
        set { resource = value; }
    }

    public MenuEntry<K, T> GetMenuEntry()
    {
        return this;
    }

    public override string ToString()
    {
        return string.Format("Key {0} value: {1}", KEY, VALUE);
    }

    #region IEquatable implementation

    public bool Equals(K other)
    {
        return evnt.Equals(other);
    }

    #endregion
}

public sealed class WeightedRandomizer<T>
{

    #region Public members

    public WeightedRandomizer(List<MenuEntry<T, int>> weights)
    {
        m_weights = weights;

        m_totalWeight = 0;
        ComputeTotalWeight();
    }

    public WeightedRandomizer()
    {
    }

    ~WeightedRandomizer()
    {
        m_weights = null;
    }

    /// <summary>
    /// Computes the total weight
    /// </summary>
    /// <param name="weights"></param>
    private void ComputeTotalWeight()
    {
        m_totalWeight = 0;
        for (int i = 0; i < m_weights.Count; i++)
        {
            m_totalWeight += m_weights[i].VALUE;
        }
    }

    /// <summary>
    /// Gets the next item. based on weighted probability.
    /// Use dynamicWeights flag when the weights are constantly changing
    /// </summary>
    /// <returns>The next item.</returns>
    /// <param name="dynamicWeights">If set to <c>true</c> dynamic weights.</param>
    public T GetNextItem(bool dynamicWeights = false)
    {
        if (dynamicWeights)
        {
            ComputeTotalWeight();
        }

        int randomVal = UnityEngine.Random.Range(0, m_totalWeight + 1);

        for (int index = 0; index < m_weights.Count; index++)
        {
            if (randomVal < m_weights[index].VALUE)
                return m_weights[index].KEY;
            else
            {
                randomVal -= m_weights[index].resource;
            }
        }

        return m_weights[m_weights.Count - 1].KEY;
    }

    /// <summary>
    /// Add a weight entry to the existing weights
    /// </summary>
    /// <param name="weight"></param>
    public void AddWeight(MenuEntry<T, int> weight)
    {
        Debug.Assert(weight != null, "[WeightedRandomizer]: menuEntry to AddWeights cannot be null");

        m_weights.Add(weight);
        ComputeTotalWeight();
    }

    /// <summary>
    /// Add or override weights to the existing weights
    /// </summary>
    /// <param name="weights"></param>
    /// <param name="overwrite"></param>
    public void AddWeights(List<MenuEntry<T, int>> weights, bool overwrite)
    {
        if (overwrite)
            m_weights = weights;
        else
            m_weights.AddRange(weights);

        ComputeTotalWeight();
    }

    #endregion

    private List<MenuEntry<T, int>> m_weights;
    private int m_totalWeight = 0;
}

