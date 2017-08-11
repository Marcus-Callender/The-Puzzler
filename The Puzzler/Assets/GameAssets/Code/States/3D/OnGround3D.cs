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

    public override E_PLAYER_STATES Cycle(char inputs)
    {
        Debug.DrawRay(transform.position + (-transform.up * 0.1f), -transform.up * 0.1f, Color.red);

        if (GetInput(E_INPUTS.UP))
        {
            m_data.m_anim.SetBool("Walking", true);

            m_data.m_velocityX = m_speed;
        }
        else if (GetInput(E_INPUTS.DOWN))
        {
            m_data.m_anim.SetBool("WalkingBack", true);

            m_data.m_velocityX = -m_speed * 0.5f;
        }
        else
        {
            m_data.m_anim.SetBool("Walking", false);
            m_data.m_anim.SetBool("WalkingBack", false);

            m_data.m_velocityX = 0.0f;
        }

        if (GetInput(E_INPUTS.LEFT))
        {
            m_data.m_velocityZ = Mathf.Abs(m_data.m_velocityX);
        }
        else if (GetInput(E_INPUTS.RIGHT))
        {
            m_data.m_velocityZ = -Mathf.Abs(m_data.m_velocityX);
        }

        m_data.m_anim.SetFloat("Horizontal Velocity", m_data.m_velocityZ);

        if (GetInput(E_INPUTS.LEFT_2))
        {
            gameObject.transform.Rotate(gameObject.transform.up, 180 * Time.deltaTime);
        
            m_data.m_rotation = gameObject.transform.rotation;
        }
        else if (GetInput(E_INPUTS.RIGHT_2))
        {
            gameObject.transform.Rotate(-gameObject.transform.up, 180 * Time.deltaTime);
        
            m_data.m_rotation = gameObject.transform.rotation;
        }
        
        m_data.m_velocityY = -9.81f;

        if (GetInput(E_INPUTS.MOVE_BOX))
        {
            return E_PLAYER_STATES.MOVEING_BLOCK;
        }

        if (GetInput(E_INPUTS.JUMP) && !m_data.m_moveingBox)
        {
            m_data.m_velocityY = m_jumpSpeed;

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
            Debug.Log("----- no collision ----");
            return E_PLAYER_STATES.IN_AIR;
        }
        else
        {
            Debug.Log("----- collision " + hit.collider.name + " ----");
        }

        return E_PLAYER_STATES.ON_GROUND;
    }

    public override E_PLAYER_STATES InTrigger(string _tag)
    {
        if (_tag == "Ladder" && GetInput(E_INPUTS.UP))
        {
            return E_PLAYER_STATES.USEING_LADDER;
        }

        return E_PLAYER_STATES.ON_GROUND;
    }
}
