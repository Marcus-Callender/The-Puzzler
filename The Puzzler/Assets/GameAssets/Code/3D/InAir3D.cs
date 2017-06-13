using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAir3D : BasicState
{
    private float m_gravity = 23.0f;
    private float m_speed = 6.5f;

    private int m_enableGroundCollisionFrames = 2;
    private int m_enableGroundCollisionCount = 2;

    public override void Enter()
    {
        m_enableGroundCollisionCount = m_enableGroundCollisionFrames;

        m_data.m_anim.SetFloat("Vertical Velocity", m_data.m_velocityY);
        m_data.m_anim.SetBool("Airborn", true);
    }

    public override void Exit()
    {
        m_data.m_anim.SetBool("Airborn", false);
    }

    public override E_PLAYER_STATES Cycle()
    {
        ApplyGravity(m_gravity);

        if (m_inputs.GetInput(E_INPUTS.UP))
        {
            m_data.m_velocityX = m_speed;
        }
        else
        {
            m_data.m_velocityX = 0.0f;
        }

        if (m_inputs.GetInput(E_INPUTS.LEFT))
        {
            gameObject.transform.Rotate(gameObject.transform.up, 180 * Time.deltaTime);
            //m_data.m_rotation.y += 180 * Time.deltaTime;
            m_data.m_rotation = gameObject.transform.rotation;
        }
        else if (m_inputs.GetInput(E_INPUTS.RIGHT))
        {
            gameObject.transform.Rotate(-gameObject.transform.up, 180 * Time.deltaTime);
            //m_data.m_rotation.y -= 180 * Time.deltaTime;
            m_data.m_rotation = gameObject.transform.rotation;
        }

        if (!m_inputs.GetInput(E_INPUTS.JUMP) & m_data.m_velocityY > 0.0f)
        {
            Debug.Log("Short Jump");
            m_data.m_velocityY = 0.0f;
        }

        m_data.m_anim.SetFloat("Vertical Velocity", m_data.m_velocityY);

        return E_PLAYER_STATES.IN_AIR;
    }

    public override E_PLAYER_STATES PhysCycle()
    {
        if (m_enableGroundCollisionCount > 0)
        {
            m_enableGroundCollisionCount--;
        }

        return E_PLAYER_STATES.IN_AIR;
    }

    public override E_PLAYER_STATES Colide(E_DIRECTIONS _dir, string _tag)
    {
        if (_dir == E_DIRECTIONS.TOP && m_data.m_velocityY > 0.0f)
        {
            Debug.Log("Hit ceiling");
            m_data.m_velocityY = 0.0f;
        }
        else if (_dir == E_DIRECTIONS.BOTTOM && m_enableGroundCollisionCount == 0)
        {
            return E_PLAYER_STATES.ON_GROUND;
        }
        else if ((_dir == E_DIRECTIONS.LEFT || _dir == E_DIRECTIONS.RIGHT) && !m_inputs.GetInput(E_INPUTS.JUMP))
        {
            return E_PLAYER_STATES.WALL_SLIDEING;
        }

        return E_PLAYER_STATES.IN_AIR;
    }

    public override E_PLAYER_STATES InTrigger(string _tag)
    {
        if (_tag == "Ladder" && m_inputs.GetInput(E_INPUTS.UP))
        {
            return E_PLAYER_STATES.USEING_LADDER;
        }

        return E_PLAYER_STATES.IN_AIR;
    }
}
