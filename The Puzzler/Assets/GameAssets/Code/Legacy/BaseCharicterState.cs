using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharicterState : MonoBehaviour
{
    protected CharicterData m_me;

    public void Initialize(CharicterData me)
    {
        m_me = me;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual CHARICTER_STATES GetInput()
    {
        return CHARICTER_STATES.STAND;
    }

    public virtual CHARICTER_STATES Cycle()
    {
        return CHARICTER_STATES.STAND;
    }

    public virtual CHARICTER_STATES Collision(DIRECTIONS direction, string tag)
    {
        return CHARICTER_STATES.STAND;
    }

    public virtual CHARICTER_STATES NotCollided(DIRECTIONS direction)
    {
        return CHARICTER_STATES.STAND;
    }

}

