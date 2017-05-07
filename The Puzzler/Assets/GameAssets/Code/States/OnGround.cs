using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : BasicState
{
    private float m_speed = 6.5f;
    private float m_jumpSpeed = 17.5f;

    void Start()
    {

    }

    public override E_PLAYER_STATES Cycle()
    {
        MoveHorzontal(m_speed);

        m_data.m_velocityY = -9.81f;

        if (Input.GetButtonDown("Jump"))
        {
            m_data.m_velocityY = m_jumpSpeed;

            return E_PLAYER_STATES.IN_AIR;
        }

        return E_PLAYER_STATES.ON_GROUND;
    }
}
