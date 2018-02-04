using System.Collections;
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

    public virtual E_PLAYER_STATES Cycle(char inputs, char joystickMovement)
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES PhysCycle(char inputs, char joystickMovement)
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

    public virtual E_PLAYER_STATES InTrigger(string _tag, char inputs)
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES LeaveTrigger(string _tag)
    {
        return E_PLAYER_STATES.NULL;
    }

    protected void MoveHorzontal(float _speed, char inputs)
    {
        if (GetInput(E_INPUTS.LEFT, inputs))
        {
            m_data.m_velocityX += _speed;

            if (!m_data.m_left_right)
            {
                m_data.m_left_right = true;
                gameObject.transform.Rotate(new Vector3(0.0f, 180.0f));
                m_data.m_rotation = gameObject.transform.rotation;
            }
        }

        if (GetInput(E_INPUTS.RIGHT, inputs))
        {
            m_data.m_velocityX += -_speed;

            if (m_data.m_left_right)
            {
                m_data.m_left_right = false;
                gameObject.transform.Rotate(new Vector3(0.0f, 180.0f));
                m_data.m_rotation = gameObject.transform.rotation;
            }
        }
    }

    protected void Standard3DMovment(float _speed, char inputs, char joystickMovement)
    {
        Debug.DrawRay(transform.position + (-transform.up * 0.1f), -transform.up * 0.1f, Color.red);

        if (GetInput(E_INPUTS.RESET_CAMERA, inputs))
        {
            m_data.resetCameraDirection();
        }

        if (GetInput(E_INPUTS.LEFT_2, inputs))
        {
            m_data.m_cameraRotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * 72.0f * GetJoystickMovment(E_JOYSTICK_INPUTS.HORIZONTAL_2, inputs, joystickMovement));
        }
        else if (GetInput(E_INPUTS.RIGHT_2, inputs))
        {
            m_data.m_cameraRotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * 72.0f * -GetJoystickMovment(E_JOYSTICK_INPUTS.HORIZONTAL_2, inputs, joystickMovement));
        }

        Debug.DrawRay(transform.position, m_data.m_cameraRotation * Vector3.forward, Color.red);

        Quaternion charicterRot = m_data.m_cameraRotation;

        if (GetInput(E_INPUTS.UP, inputs) || GetInput(E_INPUTS.DOWN, inputs) || GetInput(E_INPUTS.LEFT, inputs) || GetInput(E_INPUTS.RIGHT, inputs))
        {
            charicterRot *= Quaternion.Euler(Vector3.up * (Mathf.Atan2(GetJoystickMovment(E_JOYSTICK_INPUTS.HORIZONTAL, inputs, joystickMovement), GetJoystickMovment(E_JOYSTICK_INPUTS.VERTICAL, inputs, joystickMovement)) * Mathf.Rad2Deg));

            m_data.m_anim.SetBool("Walking", true);
            transform.rotation = charicterRot;
            m_data.m_rotation = charicterRot;
            m_data.m_velocityX += _speed;
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

    public virtual bool GetInput(E_INPUTS input, char inputs)
    {
        return (inputs & (char)InputToBit(input)) > 0;
    }

    public virtual int GetJoystickMovment(E_JOYSTICK_INPUTS input, char inputs, char joystickMovement)
    {
        int magnitude = 0;

        magnitude += (joystickMovement & (int)input) > 0 ? 1 : 0;
        magnitude += (joystickMovement & (int)input * 2) > 0 ? 2 : 0;
        magnitude += (joystickMovement & (int)input * 4) > 0 ? 4 : 0;

        if (input == E_JOYSTICK_INPUTS.HORIZONTAL && GetInput(E_INPUTS.RIGHT, inputs) || input == E_JOYSTICK_INPUTS.VERTICAL && GetInput(E_INPUTS.DOWN, inputs))
        {
            magnitude *= -1;
        }

        return magnitude;
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
