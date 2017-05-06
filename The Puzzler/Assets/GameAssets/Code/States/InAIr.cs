using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAIr : BasicState
{
    float m_gravity = 23.0f;

    void Start()
    {

    }

    public override E_PLAYER_STATES Cycle()
    {
        ApplyGravity(m_gravity);

        if (Input.GetButtonUp("Jump") & m_data.m_velocityY > 0.0f)
        {
            m_data.m_velocityY = 0.0f;
        }

        return E_PLAYER_STATES.IN_AIR;
    }
    
    public override E_PLAYER_STATES Colide(E_DIRECTIONS _dir, string _tag)
    {

        return E_PLAYER_STATES.IN_AIR;
    }
}
