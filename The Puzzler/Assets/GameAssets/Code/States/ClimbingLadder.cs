using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingLadder : BasicState
{
    private float m_speed = 3.0f;
    private float m_climbSpeed = 6.5f;
    private float m_jumpSpeed = 12.5f;

    private int m_enableGroundCollisionFrames = 2;
    private int m_enableGroundCollisionCount = 2;

    public override void Enter()
    {
        m_enableGroundCollisionCount = m_enableGroundCollisionFrames;
    }

    public override E_PLAYER_STATES Cycle()
    {
        MoveHorzontal(m_speed);

        if (m_inputs.GetInput(E_INPUTS.DOWN))
            m_data.m_velocityY = -m_climbSpeed;

        if (m_inputs.GetInput(E_INPUTS.UP))
            m_data.m_velocityY = m_climbSpeed;
        
        if (m_inputs.GetInput(E_INPUTS.JUMP))
        {
            m_data.m_velocityY = m_jumpSpeed;

            return E_PLAYER_STATES.IN_AIR;
        }

        return E_PLAYER_STATES.USEING_LADDER;
    }

    public override E_PLAYER_STATES PhysCycle()
    {
        if (m_enableGroundCollisionCount > 0)
        {
            m_enableGroundCollisionCount--;
        }

        return E_PLAYER_STATES.USEING_LADDER;
    }

    public override E_PLAYER_STATES Colide(E_DIRECTIONS _dir, string _tag)
    {
        if (_dir == E_DIRECTIONS.BOTTOM && m_enableGroundCollisionCount == 0)
        {
            return E_PLAYER_STATES.ON_GROUND;
        }

        return E_PLAYER_STATES.USEING_LADDER;
    }

    public override E_PLAYER_STATES LeaveTrigger(string _tag)
    {
        if (_tag == "Ladder")
        {
            // if the player left an area marked as "Ladder"
            return E_PLAYER_STATES.IN_AIR;
        }

        return E_PLAYER_STATES.USEING_LADDER;
    }
}
