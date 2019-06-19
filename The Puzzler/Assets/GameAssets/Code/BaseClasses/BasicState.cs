using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicState : MonoBehaviour
{
    protected PlayerData m_data;
    protected Rigidbody m_rigb;

    public bool m_useWallGravity = false;

    public virtual void Initialize(Rigidbody rigb, PlayerData data)
    {
        m_rigb = rigb;
        m_data = data;
    }

    public virtual void Enter()
    {

    }

    public virtual IGhostInteractable GhostSpecialEnter()
    {
        Debug.LogWarning("Ghost special enter has not been set up for this state.");
        return null;
    }

    public virtual void Exit()
    {

    }

    public virtual IGhostInteractable GhostSpecialExit()
    {
        Debug.LogWarning("Ghost special exit has not been set up for this state.");
        return null;
    }

    public virtual E_PLAYER_STATES Cycle(S_inputStruct inputs)
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES PhysCycle(S_inputStruct inputs)
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

    public virtual E_PLAYER_STATES InTrigger(string _tag, S_inputStruct inputs)
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES LeaveTrigger(string _tag)
    {
        return E_PLAYER_STATES.NULL;
    }

    protected void MoveHorzontal(float _speed, S_inputStruct inputs)
    {
        //m_data.m_velocity.x += _speed * /*Mathf.Abs*/(inputs.m_movementVector.x);
        m_data.AddVelocity(_speed * inputs.m_movementVector.x, 0.0f, 0.0f);

        if (inputs.m_movementVector.x != 0.0f)
        {
            if (inputs.m_movementVector.x > 0.0f == !m_data.m_left_right)
            {
                m_data.m_left_right = !m_data.m_left_right;
                ///gameObject.transform.Rotate(new Vector3(0.0f, 180.0f));
                ///m_data.SetRotation(gameObject.transform.rotation);

                //transform.rotation *= Quaternion.Euler(Vector3.up * 180.0f);
                m_data.SetRotation(gameObject.transform.rotation * Quaternion.Euler(Vector3.up * 180.0f));

                ///transform.rotation = charicterRot;
                ///m_data.SetRotation(charicterRot);
                ///m_data.m_velocity.x += _speed;
            }
        }
    }

    protected void Standard3DMovment(float _speed, S_inputStruct inputs)
    {
        Debug.DrawRay(transform.position + (-transform.up * 0.1f), -transform.up * 0.1f, Color.red);

        if (GetInput(E_INPUTS.RESET_CAMERA, inputs))
        {
            m_data.resetCameraDirection();
        }
        
        m_data.m_cameraRotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * 72.0f * inputs.m_cameraVector.x);

        Debug.DrawRay(transform.position, m_data.m_cameraRotation * Vector3.forward, Color.red);

        Quaternion charicterRot = m_data.m_cameraRotation;
        
        if (inputs.m_movementVector.x != 0 || inputs.m_movementVector.y != 0)
        {
            charicterRot *= Quaternion.Euler(Vector3.up * (Mathf.Atan2(inputs.m_movementVector.x, inputs.m_movementVector.y) * Mathf.Rad2Deg));

            m_data.m_anim.SetBool("Walking", true);
            //transform.rotation = charicterRot;
            m_data.SetRotation(charicterRot);
            //m_data.m_velocity.x += _speed;
            m_data.AddVelocity(_speed, 0.0f, 0.0f);
        }
        else
        {
            m_data.m_anim.SetBool("Walking", false);
        }
    }

    protected void ApplyGravity()
    {
        //m_data.m_velocity += (m_data.m_gravity * Time.deltaTime);
        m_data.AddVelocity(m_data.m_gravity * Time.deltaTime);
    }

    public virtual bool GetInput(E_INPUTS input, S_inputStruct inputs)
    {
        return (inputs.m_buttons & (char)InputToBit(input)) > 0;
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
