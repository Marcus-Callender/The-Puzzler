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

    public override E_PLAYER_STATES Cycle()
    {
        MoveHorzontal(m_speed);

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.0f)
        {
            m_data.m_anim.SetBool("Walking", true);
        }
        else
        {
            m_data.m_anim.SetBool("Walking", false);
        }

        m_data.m_velocityY = -9.81f;

        if (m_data.m_closeToBox && Input.GetButton("MoveBox"))
        {
            m_data.m_moveingBox = true;
            return E_PLAYER_STATES.MOVEING_BLOCK;
        }

        if (Input.GetButtonDown("Jump") && !m_data.m_moveingBox)
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
        if (_tag == "Ladder" && Input.GetAxisRaw("Vertical") > 0.5f)
        {
            return E_PLAYER_STATES.USEING_LADDER;
        }

        return E_PLAYER_STATES.ON_GROUND;
    }
}
