using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : BasicState
{
    private float m_speed = 6.5f;
    private float m_jumpSpeed = 9.5f;

    void Start()
    {
        
    }

    public override E_PLAYER_STATES Cycle(S_inputStruct inputs)
    {
        MoveHorzontal(m_speed, inputs);
        
        if (inputs.m_movementVector.x != 0.0f)
        {
            m_data.m_anim.SetBool("Walking", true);
        }
        else
        {
            m_data.m_anim.SetBool("Walking", false);
        }

        m_data.m_velocityY = -9.81f;

        if (GetInput(E_INPUTS.MOVE_BOX, inputs))
        {
            return E_PLAYER_STATES.MOVEING_BLOCK;
        }

        if (GetInput(E_INPUTS.JUMP, inputs))
        {
            m_data.m_velocityY = m_jumpSpeed;

            return E_PLAYER_STATES.IN_AIR;
        }

        return E_PLAYER_STATES.ON_GROUND;
    }

    public override E_PLAYER_STATES LeaveColision(string _tag)
    {
        return E_PLAYER_STATES.IN_AIR;
    }

    public override E_PLAYER_STATES InTrigger(string _tag, S_inputStruct inputs)
    {
        if (_tag == "Ladder" && inputs.m_movementVector.y > 0.0f)
        {
            return E_PLAYER_STATES.USEING_LADDER;
        }

        return E_PLAYER_STATES.ON_GROUND;
    }
}
