﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicState : MonoBehaviour
{
    protected PlayerData m_data;
    protected Rigidbody m_rigb;
    //public char m_inputs;
    
    public bool m_useWallGravity = false;
    
    public virtual void Initialize(Rigidbody rigb, PlayerData data)
    {
        m_rigb = rigb;
        m_data = data;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual E_PLAYER_STATES Cycle(char inputs)
    {
        m_data.m_inputs = (char)0;
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES PhysCycle()
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES NoGround()
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES Colide(E_DIRECTIONS _dir, string _tag)
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES LeaveColision(string _tag)
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES InTrigger(string _tag)
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES LeaveTrigger(string _tag)
    {
        return E_PLAYER_STATES.NULL;
    }

    protected void MoveHorzontal(float _speed)
    {
        m_data.m_velocityX += 0.0f;

        if (GetInput(E_INPUTS.LEFT))
            m_data.m_velocityX += _speed;

        if (GetInput(E_INPUTS.RIGHT))
            m_data.m_velocityX += -_speed;
    }

    protected void Standard3DMovment(float _speed)
    {
        Debug.DrawRay(transform.position + (-transform.up * 0.1f), -transform.up * 0.1f, Color.red);

        if (GetInput(E_INPUTS.LEFT_2))
        {
            m_data.m_playerLookingDirection *= Quaternion.Euler(Vector3.up * Time.deltaTime * 60.0f * 5.0f);
        }
        else if (GetInput(E_INPUTS.RIGHT_2))
        {
            m_data.m_playerLookingDirection *= Quaternion.Euler(Vector3.up * Time.deltaTime * -60.0f * 5.0f);
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

            m_data.m_anim.SetBool("Walking", true);
            transform.rotation = charicterRot;
            m_data.m_rotation = charicterRot;
            m_data.m_velocityX = _speed;
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

            m_data.m_anim.SetBool("Walking", true);
            transform.rotation = charicterRot;
            m_data.m_rotation = charicterRot;
            m_data.m_velocityX = _speed;
        }
        else
        {
            m_data.m_anim.SetBool("Walking", false);
        }
    }

    protected void ApplyGravity(float _force)
    {
        m_data.m_velocityY -= (_force * Time.deltaTime);
    }

    public virtual bool GetInput(E_INPUTS input)
    {
        return (m_data.m_inputs & (char)InputToBit(input)) > 0;
    }

    protected virtual int InputToBit(E_INPUTS input)
    {
        int bit = 1;

        for (int z = 0; z < (int)input; z++)
        {
            bit *= 2;
        }

        return bit;
    }
}
