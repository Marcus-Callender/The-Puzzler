using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingLadder : BasicState
{
    private float m_speed = 3.0f;
    private float m_climbSpeed = 6.5f;
    private float m_jumpSpeed = 12.5f;

    void Start()
    {

    }

    public override E_PLAYER_STATES Cycle()
    {
        MoveHorzontal(m_speed);
        
        m_data.m_velocityY = Input.GetAxisRaw("Vertical") * m_climbSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            m_data.m_velocityY = m_jumpSpeed;

            return E_PLAYER_STATES.IN_AIR;
        }

        return E_PLAYER_STATES.USEING_LADDER;
    }

    public override E_PLAYER_STATES LeaveTrigger(string _tag)
    {
        if (_tag == "Ladder")
        {
            return E_PLAYER_STATES.IN_AIR;
        }

        return E_PLAYER_STATES.USEING_LADDER;
    }
}
