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
