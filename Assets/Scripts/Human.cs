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

public class Human : MonoBehaviour
{
    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        //Get and store a reference to the Rigidbody2D component so that we can access it.
        m_rigidBody = GetComponent<Rigidbody2D>();

        m_healthStateMachine = new HealthStateMachine(this);

        Health = 100;
    }

    private void Update()
    {
        m_healthStateMachine.Update();
    }

    public void InfectWithCough()
    {
        m_healthStateMachine.InfectWithCough();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Human otherHuman = other.gameObject.GetComponent<Human>();

        if(otherHuman != null)
        {
            Infect(otherHuman.HealthState);
        }
    }

    private void Infect(in HealthStateMachine otherHealthState)
    {
        m_healthStateMachine.Infect(otherHealthState);
    }

    HealthStateMachine m_healthStateMachine;
    public HealthStateMachine HealthState { get { return m_healthStateMachine; } }

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

                if(m_health == 30) // hack for now
                {
                    // recovered
                    m_spriteRenderer.color = Color.white;
                    return;
                }


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
}
