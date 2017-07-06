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

    public override E_PLAYER_STATES Cycle()
    {
        if (m_inputs.GetInput(E_INPUTS.UP))
        {
            m_data.m_anim.SetBool("Walking", true);

            m_data.m_velocityX = m_speed;
        }
        else
        {
            m_data.m_anim.SetBool("Walking", false);

            m_data.m_velocityX = 0.0f;
        }

        if (m_inputs.GetInput(E_INPUTS.LEFT))
        {
            gameObject.transform.Rotate(gameObject.transform.up, 180 * Time.deltaTime);

            m_data.m_rotation = gameObject.transform.rotation;
        }
        else if (m_inputs.GetInput(E_INPUTS.RIGHT))
        {
            gameObject.transform.Rotate(-gameObject.transform.up, 180 * Time.deltaTime);

            m_data.m_rotation = gameObject.transform.rotation;
        }

        m_data.m_velocityY = -9.81f;

        if (m_inputs.GetInput(E_INPUTS.MOVE_BOX))
        {
            m_data.m_moveingBox = true;

            return E_PLAYER_STATES.MOVEING_BLOCK;
        }

        if (m_inputs.GetInput(E_INPUTS.JUMP) && !m_data.m_moveingBox)
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

    public override E_PLAYER_STATES InTrigger(string _tag)
    {
        if (_tag == "Ladder" && m_inputs.GetInput(E_INPUTS.UP))
        {
            return E_PLAYER_STATES.USEING_LADDER;
        }

        return E_PLAYER_STATES.ON_GROUND;
    }
}
