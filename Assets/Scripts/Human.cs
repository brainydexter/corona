using System;
using UnityEngine;

public enum Symptoms
{
    cNone = 0,
    cCough,
    cSneeze,
    cBreathless,
    cFever,
}

public class HealthStateMachine : StateMachine
{

}

//public class HealthyState : IState
//{
//}

//public class CoughingState : IState
//{
//}

//public class DeadState : IState
//{
//}

//public class RecoveredState : IState
//{
//}

public class Human : MonoBehaviour
{
    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        //Get and store a reference to the Rigidbody2D component so that we can access it.
        m_rigidBody = GetComponent<Rigidbody2D>();

        m_healthStateMachine = new HealthStateMachine();
        //m_healthStateMachine.initialize(new HealthyState());

        m_state = HealthState.cHealthy;
        Health = 100;

        m_symptoms = null; // no symptoms exist on a new healthy human
    }

    HealthStateMachine m_healthStateMachine;

    enum HealthState
    {
        cHealthy, // never been infected before
        cInfected,
        cRecovered // human recovered after being infected 
    };

    [SerializeField]
    private HealthState m_state;

    [SerializeField]
    private Symptoms[] m_symptoms;

    [SerializeField]
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

    public float m_speed = 12.5f;             //Floating point variable to store the player's movement speed.

    private SpriteRenderer m_spriteRenderer;
    public Rigidbody2D m_rigidBody;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

    /// <summary>
    /// Infect iff human is healthy. Infected humans are not affected when they come in contact with another infected human
    /// Healthy -> Cough | Sneeze -> Breathlessnes -> Die | Recover
    /// </summary>
    /// <param name="symptom"></param>
    internal void Infect(Symptoms symptom)
    {
        Debug.Assert(symptom == Symptoms.cNone, "cannot be infected with no symptoms");

        // if a human has recovered, lets assume he can't be infected again
        if (m_state == HealthState.cRecovered)
            return;

        if (m_symptoms == null)
        {
            m_symptoms = new Symptoms[4];
        }

        // find if human is already infected with the same symptom
        bool symptomExists = Array.Find(m_symptoms, s => s == symptom) == symptom;

        Debug.Log("to infect: " + (symptomExists == false));

        // try to infect 
        for (int i = 0; !symptomExists && (i < m_symptoms.Length); ++i)
        {
            if(m_symptoms[i] == Symptoms.cNone)
            {
                m_symptoms[i] = symptom;
                break;
            }
        }

        m_state = HealthState.cInfected;

        Health = 50;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Human otherHuman = other.gameObject.GetComponent<Human>();

        if(otherHuman != null)
        {
            /*
             * 1 - Healthy, 0 - Infected
             * 
             * M    1    0    1    0
             * O    1    0    0    1
             *      N    N    M0   Oo
             */

            if( (
                (m_state == HealthState.cHealthy) ||
                (m_state == HealthState.cInfected))
                &&
                (m_state == otherHuman.m_state))
            {
                return;
            }

            if(m_state == HealthState.cHealthy)
            {
                Infect(otherHuman.m_symptoms[0]);
            }

            Debug.Log("I: " + name + " Collided with other human: " + otherHuman.name);

        }
    }
}
