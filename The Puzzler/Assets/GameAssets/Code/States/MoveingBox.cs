using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingBox : BasicState
{
    private Timer m_pauseTimer;
    private Timer m_dragingTimer;

    private float m_dragSpeed = 3.5f;

    bool m_debug = false;

    public override void Enter()
    {
        m_pauseTimer = new Timer();
        m_dragingTimer = new Timer();

        m_pauseTimer.m_time = 0.2f;
        m_dragingTimer.m_time = 0.7f;

        m_pauseTimer.Play();

        m_data.m_velocityX = 0.0f;

        m_debug = false;
    }

    public override void Exit()
    {
        m_pauseTimer.Play();
        m_pauseTimer.m_playing = false;

        m_dragingTimer.Play();
        m_dragingTimer.m_playing = false;

        m_data.m_moveingBox = false;
    }

    public override E_PLAYER_STATES Cycle()
    {
        if (!m_data.m_closeToBox)
        {
            return E_PLAYER_STATES.IN_AIR;
        }

        if (!m_pauseTimer.m_completed)
        {
            if (m_debug)
            {
                Debug.Log("Flag 1");
            }

            m_pauseTimer.Cycle();
        }
        else if (!m_dragingTimer.m_playing)
        {
            if (m_debug)
            {
                Debug.Log("Flag 2");
            }

            m_dragingTimer.Play();
        }
        else if (!m_dragingTimer.m_completed)
        {
            if (m_debug)
            {
                Debug.Log("Flag 3");
            }

            m_dragingTimer.Cycle();
            MoveHorzontal(m_dragSpeed);
            Debug.Log("Dragging speed: " + m_data.m_velocityX);
        }
        else
        {
            if (m_debug)
            {
                Debug.Log("Flag 4");
            }

            m_data.m_velocityX = 0.0f;
            m_pauseTimer.Play();

            m_debug = true;

            m_dragingTimer.m_playing = false;
        }

        if (!Input.GetButton("MoveBox"))
        {
            return E_PLAYER_STATES.ON_GROUND;
        }

        return E_PLAYER_STATES.MOVEING_BLOCK;
    }

    public override E_PLAYER_STATES LeaveColision(string _tag)
    {
        return base.LeaveColision(_tag);
    }
}
