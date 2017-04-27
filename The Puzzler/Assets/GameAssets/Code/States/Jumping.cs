using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : BaseCharicterState
{
    float m_verticalVelocity = 0.0f;
    float m_gravity = -15.0f;

    public override void Enter()
    {
        m_verticalVelocity = 4.0f;
    }

    public override void Exit()
    {

    }

    public override CHARICTER_STATES GetInput()
    {
        return CHARICTER_STATES.JUMP;
    }

    public override CHARICTER_STATES Cycle()
    {
        m_verticalVelocity += m_gravity * Time.deltaTime;
        m_me.m_yVelocity = m_verticalVelocity;

        return CHARICTER_STATES.JUMP;
    }

    public override CHARICTER_STATES Collision(DIRECTIONS direction, string tag)
    {
        if (direction == DIRECTIONS.DOWN)
        {
            return CHARICTER_STATES.STAND;
        }

        return CHARICTER_STATES.JUMP;
    }

    public override CHARICTER_STATES NotCollided(DIRECTIONS direction)
    {
        return CHARICTER_STATES.JUMP;
    }
}
