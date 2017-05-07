using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAIr : BasicState
{
    private float m_gravity = 23.0f;
    private bool m_newInState = false;
    private float m_speed = 6.5f;

    void Start()
    {

    }

    public override void Enter()
    {
        //Vector3 pos = gameObject.transform.position;
        //
        //pos.y += 0.001f;
        //
        //gameObject.transform.position = pos;

        m_newInState = true;
    }

    public override E_PLAYER_STATES Cycle()
    {
        if (!m_newInState)
        {
            ApplyGravity(m_gravity);
        }

        m_newInState = false;

        MoveHorzontal(m_speed);

        if (Input.GetButtonUp("Jump") & m_data.m_velocityY > 0.0f)
        {
            Debug.Log("Short Jump");
            m_data.m_velocityY = 0.0f;
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
        else if (_dir == E_DIRECTIONS.BOTTOM && !m_newInState)
        {
            return E_PLAYER_STATES.ON_GROUND;
        }

        return E_PLAYER_STATES.IN_AIR;
    }
}
