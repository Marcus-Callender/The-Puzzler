using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround3D : BasicState
{
    private float m_speed = 6.5f;
    private float m_jumpSpeed = 9.5f;

    void Start()
    {

    }

    public override E_PLAYER_STATES Cycle(S_inputStruct inputs)
    {
        Standard3DMovment(m_speed, inputs);

        //m_data.m_anim.SetFloat("Horizontal Velocity", m_data.m_velocityZ);

        m_data.SetYVelocity(-9.81f);

        if (GetInput(E_INPUTS.MOVE_BOX, inputs))
        {
            return E_PLAYER_STATES.MOVEING_BLOCK;
        }

        if (GetInput(E_INPUTS.JUMP, inputs))
        {
            m_data.SetYVelocity(m_jumpSpeed);

            return E_PLAYER_STATES.IN_AIR;
        }

        return E_PLAYER_STATES.ON_GROUND;
    }

    public override E_PLAYER_STATES LeaveColision(string _tag)
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, new Vector3(0.0f, -1.0f, 0.0f) * 1.0f, Color.green);

        if (!Physics.Raycast(transform.position + new Vector3(0.0f, 0.1f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), out hit, 1.0f))
        {
            //Debug.Log("----- no collision ----");
            return E_PLAYER_STATES.IN_AIR;
        }
        else
        {
            //Debug.Log("----- collision " + hit.collider.name + " ----");
        }

        return E_PLAYER_STATES.ON_GROUND;
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
