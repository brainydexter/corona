using UnityEngine;

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
        cHealthy,
        cInfected,
        cRecovered // human recovered after being infected 
    };
    private HealthState m_state;

    enum Symptoms
    {
        cNone, // human is healthy
        cCough,
        cSneeze,
        cBreathless,
        cFever
    }
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

    private SpriteRenderer m_spriteRenderer;
}
