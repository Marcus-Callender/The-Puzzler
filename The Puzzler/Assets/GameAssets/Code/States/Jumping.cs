using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : BaseCharicterState
{
    public override void Enter()
    {

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
