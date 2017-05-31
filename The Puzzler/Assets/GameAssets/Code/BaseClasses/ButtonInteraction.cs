using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    protected bool m_activated;

    public float m_activateDelayTime = 0.1f;
    protected bool m_transitioning = false;
    protected Timer m_delayTimer;

    public virtual void Start()
    {
        m_delayTimer = new Timer();
        m_delayTimer.m_time = m_activateDelayTime;

        m_delayTimer.Play();
        m_delayTimer.m_playing = false;
    }

    public virtual void OnInteract()
    {
        m_delayTimer.Play();


    }

    public virtual void Update()
    {
        m_delayTimer.Cycle();

        if (m_delayTimer.m_completed)
        {
            m_delayTimer.m_playing = false;
            m_delayTimer.m_completed = false;
            Activate();
        }
    }

    public virtual void Activate()
    {
        m_activated = !m_activated;
    }
}
