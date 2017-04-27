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

        if (horizontal > 0.5f || horizontal < -0.5f)
        {
            return CHARICTER_STATES.WALK;
        }
        
        return CHARICTER_STATES.STAND;
    }

    public override CHARICTER_STATES Cycle()
    {
        m_me.m_rigb.velocity = new Vector3(0.0f, 0.1f);

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
