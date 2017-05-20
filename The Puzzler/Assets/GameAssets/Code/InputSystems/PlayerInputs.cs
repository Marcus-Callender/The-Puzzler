using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_INPUTS
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
    JUMP,
    MOVE_BOX,
    PRESS_BUTTON,

    END,

    NULL
}

public class PlayerInputs : MonoBehaviour
{
    private char m_Inputs;
    
    public virtual void Cycle()
    {
        m_Inputs = (char)0;
        
        if (Input.GetAxisRaw("Horizontal") > 0.0f)
        {
            m_Inputs |= (char)InputToBit(E_INPUTS.LEFT);
        }

        if (Input.GetAxisRaw("Horizontal") < 0.0f)
        {
            m_Inputs |= (char)InputToBit(E_INPUTS.RIGHT);
        }

        if (Input.GetAxisRaw("Vertical") > 0.0f)
        {
            m_Inputs |= (char)InputToBit(E_INPUTS.UP);
        }

        if (Input.GetAxisRaw("Vertical") < 0.0f)
        {
            m_Inputs |= (char)InputToBit(E_INPUTS.DOWN);
        }

        if (Input.GetButton("Jump"))
        {
            m_Inputs |= (char)InputToBit(E_INPUTS.JUMP);
        }

        if (Input.GetButton("MoveBox"))
        {
            m_Inputs |= (char)InputToBit(E_INPUTS.MOVE_BOX);
        }
        
        if (Input.GetButtonDown("PressButton"))
        {
            m_Inputs |= (char)InputToBit(E_INPUTS.PRESS_BUTTON);
        }
    }

    public virtual bool GetInput(E_INPUTS input)
    {
        return (m_Inputs & (char)InputToBit(input)) > 0;
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
