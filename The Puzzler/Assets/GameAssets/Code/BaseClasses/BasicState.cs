using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicState : MonoBehaviour
{
    protected PlayerData m_data;
    protected Rigidbody m_rigb;
    protected PlayerInputs m_inputs;
    
    public bool m_useWallGravity = false;


    public virtual void Initialize(Rigidbody rigb, PlayerData data, PlayerInputs inputs)
    {
        m_rigb = rigb;
        m_data = data;
        m_inputs = inputs;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual E_PLAYER_STATES Cycle()
    {
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
        //direction = Input.GetAxisRaw("Horizontal");
        //
        //m_data.m_velocityX = direction * _speed;

        m_data.m_velocityX = 0.0f;

        if (m_inputs.GetInput(E_INPUTS.LEFT))
            m_data.m_velocityX = _speed;

        if (m_inputs.GetInput(E_INPUTS.RIGHT))
            m_data.m_velocityX = -_speed;
    }

    protected void ApplyGravity(float _force)
    {
        m_data.m_velocityY -= (_force * Time.deltaTime);
    }
}
