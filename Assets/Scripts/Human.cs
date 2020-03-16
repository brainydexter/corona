using System;
using UnityEngine;

public enum Symptoms
{
    cCough,
    cSneeze,
    cBreathless,
    cFever,
}

public class Human : MonoBehaviour
{
    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_state = HealthState.cHealthy;
        Health = 100;

        m_symptoms = null; // no symptoms exist on a new healthy human
    }

    enum HealthState
    {
        cHealthy, // never been infected before
        cInfected,
        cRecovered // human recovered after being infected 
    };
    private HealthState m_state;

    
    private Symptoms[] m_symptoms;

    private int m_health;
    public int Health
    {
        get { return m_health; }
        set
        {
            if (m_health != value)
            {
                m_health = Mathf.Clamp(value, 0, 100);

                Gradient gradient = new Gradient();
                GradientColorKey[] colorKeys = new GradientColorKey[2];
                colorKeys[0].color = Color.red;
                colorKeys[0].time = 0.0f;
                colorKeys[1].color = Color.green;
                colorKeys[1].time = 1.0f;

                GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                alphaKeys[0].alpha = 1.0f;
                alphaKeys[0].time = 0.0f;
                alphaKeys[1].alpha = 1.0f;
                alphaKeys[1].time = 1.0f;

                gradient.SetKeys(colorKeys, alphaKeys);

                m_spriteRenderer.color = gradient.Evaluate(m_health / 100f);
            }
        }
    }

    internal void Infect(Symptoms symptom)
    {
        // if a human has recovered, lets assume he can't be infected again
        if (m_state == HealthState.cRecovered)
            return;

        if(m_symptoms == null)
        {
            m_symptoms = new Symptoms[4];
        }

        m_state = HealthState.cInfected;
    }

    private SpriteRenderer m_spriteRenderer;
}
