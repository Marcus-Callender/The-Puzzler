using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public bool m_completed = false;
    public bool m_reversed = false;
    public bool m_playing = false;

    public float m_time = 0.0f;
    private float m_timeCount = 0.0f;
    
    public void Cycle()
    {
        if (!m_completed && m_playing)
        {
            if (!m_reversed)
            {
                m_timeCount += Time.deltaTime;

                if (m_time <= m_timeCount)
                {
                    m_completed = true;
                }
            }
            if (m_reversed)
            {
                m_timeCount -= Time.deltaTime;

                if (m_timeCount <= 0.0f)
                {
                    m_completed = true;
                }
            }
        }
    }

    public void Play(bool reverse = false)
    {
        m_reversed = reverse;
        m_playing = true;
        m_completed = false;
        
        if (reverse)
        {
            m_timeCount = m_time;
        }
        else
        {
            m_timeCount = 0.0f;
        }
    }

    public float GetLerp()
    {
        return m_timeCount / m_time;
    }
}
