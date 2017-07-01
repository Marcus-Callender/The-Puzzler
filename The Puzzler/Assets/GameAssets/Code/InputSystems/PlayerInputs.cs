﻿using System.Collections;
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
    GHOST_BUTTON_PRESS,
    GHOST_BUTTON_HOLD,

    END,

    NULL
}

public class PlayerInputs : MonoBehaviour
{
    private char m_Inputs;
    public bool m_pauseInputs = false;

    public Timer m_ghostButtonTimer;

    public virtual void Start()
    {
        m_ghostButtonTimer = new Timer();
        m_ghostButtonTimer.m_time = 0.75f;
    }

    public virtual void Cycle()
    {
        m_Inputs = (char)0;

        if (!m_pauseInputs)
        {
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
            
            if (Input.GetButtonDown("StartGhost"))
            {
                m_ghostButtonTimer.Play();
            }

            // this sometimes gives a null refrence error when reloading the game after falling from the world, though it dosen't seem to affect gameplay
            if (m_ghostButtonTimer.m_playing)
            {
                m_ghostButtonTimer.Cycle();

                if (m_ghostButtonTimer.m_completed)
                {
                    m_ghostButtonTimer.m_playing = false;
                    m_Inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_HOLD);
                }
                else if (Input.GetButtonUp("StartGhost"))
                {
                    m_ghostButtonTimer.m_playing = false;
                    m_Inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_PRESS);
                }
            }
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
