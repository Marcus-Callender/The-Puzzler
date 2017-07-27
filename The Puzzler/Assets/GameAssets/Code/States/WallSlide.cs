using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : BasicState
{
    public float m_wallSlideGravity = 9.0f;
    private float m_speed = 12.0f;

    private bool m_collideLeftRight = true;

    private Timer m_StickTimer;

    void Start()
    {
        m_StickTimer = new Timer();
        m_StickTimer.m_time = 0.5f;
    }

    public override void Enter()
    {
        m_StickTimer.Play();
    }

    public override E_PLAYER_STATES Cycle(char inputs)
    {
        m_StickTimer.Cycle();

        if (m_StickTimer.m_completed && m_StickTimer.m_playing)
        {
            m_StickTimer.Stop();
            ApplyGravity(m_wallSlideGravity);
        }
        else
        {
            m_data.m_velocityY = 0.0f;
        }

        MoveHorzontal(m_speed);

        if (GetInput(E_INPUTS.JUMP))
        {
            return E_PLAYER_STATES.IN_AIR;
        }

        return E_PLAYER_STATES.NULL;
    }


    public override E_PLAYER_STATES PhysCycle()
    {
        if (!m_collideLeftRight)
        {
            return E_PLAYER_STATES.IN_AIR;
        }

        m_collideLeftRight = false;

        return E_PLAYER_STATES.NULL;
    }

    public override E_PLAYER_STATES Colide(E_DIRECTIONS _dir, string _tag)
    {
        if (_dir == E_DIRECTIONS.BOTTOM)
        {
            return E_PLAYER_STATES.ON_GROUND;
        }
        else if (_dir == E_DIRECTIONS.LEFT || _dir == E_DIRECTIONS.RIGHT)
        {
            m_collideLeftRight = true;
        }



        return E_PLAYER_STATES.NULL;
    }
}
