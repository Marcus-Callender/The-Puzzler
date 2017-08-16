using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : MonoBehaviour
{
    private EnemyData m_data;
    private Rigidbody m_rigb;

    void Initialize()
    {

    }

    public virtual void Cycle()
    {

    }

    public virtual E_ENEMY_STATES EnterState()
    {
        return E_ENEMY_STATES.NULL;
    }

    public virtual E_ENEMY_STATES ExitState()
    {
        return E_ENEMY_STATES.NULL;
    }

    public virtual E_ENEMY_STATES Colide()
    {
        return E_ENEMY_STATES.NULL;
    }

    public virtual E_ENEMY_STATES LeaveColide()
    {
        return E_ENEMY_STATES.NULL;
    }

    public virtual E_ENEMY_STATES InTrigger()
    {
        return E_ENEMY_STATES.NULL;
    }

    public virtual bool GetInput(E_ENEMY_INPUTS input, char inputs)
    {
        return (inputs & (char)InputToBit(input)) > 0;
    }

    protected virtual int InputToBit(E_ENEMY_INPUTS input)
    {
        int bit = 1;

        for (int z = 0; z < (int)input; z++)
        {
            bit *= 2;
        }

        return bit;
    }
}
