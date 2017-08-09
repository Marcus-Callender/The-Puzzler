using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAIr : BasicState
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

    public override E_PLAYER_STATES Cycle(char inputs)
    {
        Debug.DrawRay(transform.position + (transform.up * 1.5f), transform.up * 0.1f, Color.blue);

        ApplyGravity(m_gravity);

        MoveHorzontal(m_speed);

        if (!GetInput(E_INPUTS.JUMP) & m_data.m_velocityY > 0.0f)
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
            RaycastHit hit;

            //if (Physics.Raycast(transform.position, transform.up * 0.6f, out hit))
            if (Physics.Raycast(transform.position + (transform.up * 1.5f), transform.up * 0.1f, out hit))
            {
                if (hit.collider.tag != "Player")
                {
                    Debug.Log("Hit ceiling");
                    //m_data.m_velocityY = 0.0f;
                }
            }
        }
        else if (_dir == E_DIRECTIONS.BOTTOM && m_enableGroundCollisionCount == 0)
        {
            return E_PLAYER_STATES.ON_GROUND;
        }
        //else if (( _dir == E_DIRECTIONS.LEFT || _dir == E_DIRECTIONS.RIGHT) && !m_inputs.GetInput(E_INPUTS.JUMP))
        //{
        //    return E_PLAYER_STATES.WALL_SLIDEING;
        //}

        return E_PLAYER_STATES.IN_AIR;
    }

    public override E_PLAYER_STATES InTrigger(string _tag)
    {
        if (_tag == "Ladder" && GetInput(E_INPUTS.UP))
        {
            return E_PLAYER_STATES.USEING_LADDER;
        }

        return E_PLAYER_STATES.IN_AIR;
    }
}
