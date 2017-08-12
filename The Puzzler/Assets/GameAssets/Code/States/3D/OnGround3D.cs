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

        if (GetInput(E_INPUTS.LEFT_2))
        {
            //gameObject.transform.Rotate(gameObject.transform.up, 180 * Time.deltaTime);

            m_data.m_playerLookingDirection *= Quaternion.Euler(Vector3.up * Time.deltaTime * 60.0f * 5.0f);
            //m_data.m_rotation = gameObject.transform.rotation;
        }
        else if (GetInput(E_INPUTS.RIGHT_2))
        {
            //gameObject.transform.Rotate(-gameObject.transform.up, 180 * Time.deltaTime);

            m_data.m_playerLookingDirection *= Quaternion.Euler(Vector3.up * Time.deltaTime * -60.0f * 5.0f);
            //m_data.m_rotation = gameObject.transform.rotation;
        }

        Debug.DrawRay(transform.position, m_data.m_playerLookingDirection * Vector3.forward, Color.red);

        Quaternion charicterRot = m_data.m_playerLookingDirection;

        if (GetInput(E_INPUTS.UP) || GetInput(E_INPUTS.DOWN))
        {
            if (GetInput(E_INPUTS.UP))
            {
                if (GetInput(E_INPUTS.LEFT))
                {
                    charicterRot *= Quaternion.Euler(Vector3.up * 45.0f);
                }
                else if (GetInput(E_INPUTS.RIGHT))
                {
                    charicterRot *= Quaternion.Euler(Vector3.up * -45.0f);
                }
            }
            if (GetInput(E_INPUTS.DOWN))
            {
                charicterRot *= Quaternion.Euler(Vector3.up * 180.0f);

                if (GetInput(E_INPUTS.LEFT))
                {
                    charicterRot *= Quaternion.Euler(Vector3.up * -45.0f);
                }
                else if (GetInput(E_INPUTS.RIGHT))
                {
                    charicterRot *= Quaternion.Euler(Vector3.up * 45.0f);
                }
            }


            //charicterRot *= Quaternion.Euler(Vector3.up * 90.0f);
            m_data.m_anim.SetBool("Walking", true);
            transform.rotation = charicterRot;
            m_data.m_rotation = charicterRot;
            m_data.m_velocityX = m_speed;
        }
        else if (GetInput(E_INPUTS.LEFT) || GetInput(E_INPUTS.RIGHT))
        {

            if (GetInput(E_INPUTS.LEFT))
            {
                charicterRot *= Quaternion.Euler(Vector3.up * 90.0f);
            }
            else if (GetInput(E_INPUTS.RIGHT))
            {
                charicterRot *= Quaternion.Euler(Vector3.up * -90.0f);
            }

            //charicterRot *= Quaternion.Euler(Vector3.up * 90.0f);
            m_data.m_anim.SetBool("Walking", true);
            transform.rotation = charicterRot;
            m_data.m_rotation = charicterRot;
            m_data.m_velocityX = m_speed;
        }
        else
        {
            m_data.m_anim.SetBool("Walking", false);
        }

        m_data.m_anim.SetFloat("Horizontal Velocity", m_data.m_velocityZ);

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
