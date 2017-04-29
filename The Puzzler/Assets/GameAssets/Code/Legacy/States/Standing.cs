using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standing : BaseCharicterState
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override CHARICTER_STATES GetInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        Debug.Log("Axis: " + horizontal);

        if (horizontal > 0.5f || horizontal < -0.5f)
        {
            return CHARICTER_STATES.WALK;
        }

        if (Input.GetAxisRaw("Vectical") > 0.5f)
        {
            return CHARICTER_STATES.JUMP;
        }

        return CHARICTER_STATES.STAND;
    }

    public override CHARICTER_STATES Cycle()
    {
        m_me.m_xVelocity = 0.0f;
        m_me.m_yVelocity = 0.0f;

        return CHARICTER_STATES.STAND;
    }

    public override CHARICTER_STATES Collision(DIRECTIONS direction, string tag)
    {
        return CHARICTER_STATES.STAND;
    }

    public override CHARICTER_STATES NotCollided(DIRECTIONS direction)
    {
        if (direction == DIRECTIONS.DOWN)
        {
            return CHARICTER_STATES.JUMP;
        }

        return CHARICTER_STATES.STAND;
    }
}
