using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingBox : BasicState
{
    private Timer m_pauseTimer;
    private Timer m_dragingTimer;

    private float m_dragSpeed = 3.5f;

    private bool m_moveInput = false;

    public override void Enter()
    {
        m_pauseTimer = new Timer();
        m_dragingTimer = new Timer();

        m_pauseTimer.m_time = 0.2f;
        m_dragingTimer.m_time = 0.7f;

        m_data.m_velocityX = 0.0f;

        m_moveInput = false;
    }

    public override void Exit()
    {
        // stops the box from continuasly moving then un-links it
        m_data.m_linkedBox.Move(0.0f);
        m_data.m_linkedBox = null;

        m_pauseTimer.Play();
        m_pauseTimer.m_playing = false;

        m_dragingTimer.Play();
        m_dragingTimer.m_playing = false;

        m_data.m_moveingBox = false;
    }

    public override E_PLAYER_STATES Cycle()
    {
        bool getInput = m_inputs.GetInput(E_INPUTS.LEFT) || m_inputs.GetInput(E_INPUTS.RIGHT);

        if (getInput && !m_moveInput)
        {
            m_pauseTimer.Play();

            m_dragingTimer.Play();
            m_dragingTimer.m_playing = false;
        }

        m_moveInput = getInput;

        if (!m_data.m_closeToBox)
        {
            return E_PLAYER_STATES.IN_AIR;
        }

        if (getInput)
        {
            if (!m_pauseTimer.m_completed)
            {
                m_pauseTimer.Cycle();
            }
            else if (!m_dragingTimer.m_playing)
            {
                m_dragingTimer.Play();
            }
            else if (!m_dragingTimer.m_completed)
            {
                m_dragingTimer.Cycle();
                MoveHorzontal(m_dragSpeed);
            }
            else
            {
                m_data.m_linkedBox.Move(0.0f);
                m_data.m_velocityX = 0.0f;
                m_pauseTimer.Play();

                m_dragingTimer.m_playing = false;
            }
        }
        else
        {
            m_pauseTimer.Play();
            m_dragingTimer.Play();
            m_data.m_linkedBox.Move(0.0f);
        }
        
        if (!m_inputs.GetInput(E_INPUTS.MOVE_BOX))
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
