using UnityEngine;

using System;
using System.Collections;

/// <summary>
/// Timer class that triggers callback after time has elapsed
/// </summary>
public sealed class MyTimer : MonoBehaviour
{
    /// <summary>
    /// Initialize the timer with specified callback and time.
    /// </summary>
    /// <param name="callback">Callback.</param>
    /// <param name="time">Time.</param>
    /// <param name="runDuringGamePause">should this timer be running when game is paused </param>
    public void Initialize(Action callback, float time, bool runDuringGamePause = true, bool resetAfterExpiration = false)
    {
        m_resetAfterExpiration = resetAfterExpiration;
        m_runDuringGamePause = runDuringGamePause;
        Reset();
        OnTimerHit += callback;

        UpdateTimer(time);
    }

    /// <summary>
    /// Updates the timer with time value. if time = 0, timer is disabled
    /// </summary>
    /// <param name="time">Time.</param>
    public void UpdateTimer(float time)
    {
        if (time == 0f)
            enabled = false;
        else
        {
            enabled = true;
            m_duration = time;
        }
    }

    #region Mono Methods

    void OnEnable()
    {
        Reset();
    }

    void Update()
    {
        if (HasExpired())
        {
            OnTimerHit.Invoke();

            if (m_resetAfterExpiration)
                Reset();
        }
        else
        {
            m_dt += (m_runDuringGamePause ? Time.unscaledDeltaTime : Time.deltaTime * Time.timeScale);
        }
    }

    void OnDestroy()
    {
        OnTimerHit = null;
    }

    #endregion

    #region Members

    private Action OnTimerHit;

    /// <summary>
    /// Time accumulated since the timer started
    /// </summary>
    public float m_dt { get; private set; }

    [SerializeField]
    private float m_duration = 0.3f;

    /// <summary>
    /// If the timer uses scaled time or unscaled time (unscaled is not affected by pausing aka Time.timescale = 0)
    /// </summary>
    private bool m_runDuringGamePause;
    private bool m_resetAfterExpiration;

    internal bool HasExpired()
    {
        return m_dt >= m_duration;
    }

    internal void Reset()
    {
        m_dt = 0f;
    }

    #endregion
}