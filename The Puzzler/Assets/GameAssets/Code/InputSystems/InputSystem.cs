using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{

    protected char m_Inputs;
    //public bool m_pauseInputs;
    public bool m_pause;

    public Timer m_ghostButtonTimer;
    public PlayerStateMachine m_player;
    public GhostList m_ghostList;

    public BaseStateMachine m_currentlyControling;

    public virtual void Start()
    {
        m_ghostButtonTimer = new Timer();
        m_ghostButtonTimer.m_time = 0.75f;
        
        m_pause = false;
        m_player = GetComponent<PlayerStateMachine>();
        m_player.Initialize();
        m_ghostList = GetComponent<GhostList>();

        m_currentlyControling = m_player;
    }

    public virtual void Update()
    {
        m_Inputs = (char)0;

        if (!m_pause)
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

            if (Input.GetButtonDown("MoveBox"))
            {
                m_Inputs |= (char)InputToBit(E_INPUTS.MOVE_BOX);
            }

            if (Input.GetButton("MoveBox"))
            {
                m_Inputs |= (char)InputToBit(E_INPUTS.MOVE_BOX_HOLD);
            }

            if (Input.GetButtonDown("PressButton"))
            {
                m_Inputs |= (char)InputToBit(E_INPUTS.PRESS_BUTTON);
            }

            if (Input.GetButtonDown("Ghost1") || Input.GetButtonDown("Ghost2"))
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
                else if (Input.GetButtonUp("Ghost1") || Input.GetButtonUp("Ghost2"))
                {
                    m_ghostButtonTimer.m_playing = false;
                    m_Inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_PRESS);
                }

                if (Input.GetButton("Ghost1"))
                {
                    m_Inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_1);
                }
                else if (Input.GetButton("Ghost2"))
                {
                    m_Inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_2);
                }
            }
        }
        else
        {
            m_Inputs = (char)0;
        }

        m_currentlyControling.GetInputs(m_Inputs);
        m_currentlyControling.Cycle();


    }

    private void FixedUpdate()
    {
        m_currentlyControling.FixedCycle();
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
