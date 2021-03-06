﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_INPUTS
{
    //LEFT,
    //RIGHT,
    //LEFT_2,
    //RIGHT_2,
    //UP,
    //DOWN,
    JUMP,
    MOVE_BOX,
    MOVE_BOX_HOLD,
    PRESS_BUTTON,
    RESET_CAMERA,
    GHOST_BUTTON_PRESS,
    GHOST_BUTTON_HOLD,
    GHOST_BUTTON_1,
    GHOST_BUTTON_2,
    CHANGE_GRAVITY,
    CANCEL_GRAVITY_CHANGE,

    END,

    NULL
}

/*public enum E_JOYSTICK_INPUTS
{
    HORIZONTAL = 1,
    VERTICAL = 8,
    HORIZONTAL_2 = 64
}*/

public struct S_inputStruct
{
    public char m_buttons;
    public Vector2 m_movementVector;
    public Vector2 m_cameraVector;

    public S_inputStruct(int c)
    {
        m_buttons = (char)c;
        m_movementVector = new Vector2(0.0f, 0.0f);
        m_cameraVector = new Vector2(0.0f, 0.0f);
    }

    public S_inputStruct(float movementX, float movementY)
    {
        m_buttons = (char)0;
        m_movementVector = new Vector2(movementX, movementY);
        m_cameraVector = new Vector2(0.0f, 0.0f);
    }
}

public class InputSystem : MonoBehaviour
{
    ///protected char m_inputs;
    ///protected char m_JoystickMovement;
    protected S_inputStruct m_inputs;
    //public bool m_pauseInputs;
    public bool m_pause;

    public Timer m_ghostButtonTimer;
    public PlayerStateMachine m_player;
    public GhostList m_ghostList;

    private GhostStateMachine m_currentGhost;

    public virtual void Start()
    {
        m_ghostButtonTimer = new Timer();
        m_ghostButtonTimer.m_time = 0.75f;

        m_pause = false;
        m_player = GetComponent<PlayerStateMachine>();
        m_player.Initialize();
        m_ghostList = GetComponent<GhostList>();

        m_currentGhost = null;
    }

    public virtual void Update()
    {
        m_inputs = new S_inputStruct(0);

        if (!m_pause)
        {
            /*if (Input.GetAxisRaw("Horizontal") > 0.0f)
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.LEFT);
            }

            if (Input.GetAxisRaw("Horizontal") < 0.0f)
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.RIGHT);
            }*/

            //m_JoystickMovement |= (char)((int)E_JOYSTICK_INPUTS.HORIZONTAL * GetThirdOfAxis(Input.GetAxisRaw("Horizontal")));
            m_inputs.m_movementVector.x = Input.GetAxisRaw("Horizontal");

            /*if (Input.GetAxisRaw("Vertical") > 0.0f)
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.UP);
            }

            if (Input.GetAxisRaw("Vertical") < 0.0f)
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.DOWN);
            }*/

            //m_JoystickMovement |= (char)((int)E_JOYSTICK_INPUTS.VERTICAL * GetThirdOfAxis(Input.GetAxisRaw("Vertical")));
            m_inputs.m_movementVector.y = Input.GetAxisRaw("Vertical");

            // can't be consolidated into one axis as they will need diffrent behavior in 2D
            /*if ((Input.GetAxisRaw("Mouse X") + Input.GetAxisRaw("Right Stick X")) > 0.0f)
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.LEFT_2);
            }

            if ((Input.GetAxisRaw("Mouse X") + Input.GetAxisRaw("Right Stick X")) < 0.0f)
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.RIGHT_2);
            }*/

            //m_JoystickMovement |= (char)((int)E_JOYSTICK_INPUTS.HORIZONTAL_2 * GetThirdOfAxis(Input.GetAxisRaw("Mouse X") + Input.GetAxisRaw("Right Stick X")));
            m_inputs.m_cameraVector.x = Input.GetAxisRaw("Mouse X") + Input.GetAxisRaw("Right Stick X");
            //Debug.Log("Mouse: " + Input.GetAxisRaw("Mouse X"));

            if (Input.GetButton("Jump"))
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.JUMP);
            }

            if (Input.GetButtonDown("MoveBox"))
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.MOVE_BOX);
            }

            if (Input.GetButton("MoveBox"))
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.MOVE_BOX_HOLD);
            }

            if (Input.GetButtonDown("PressButton"))
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.PRESS_BUTTON);
            }

            if (Input.GetButtonDown("ResetCamera"))
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.RESET_CAMERA);
            }

            if (Input.GetButtonDown("Ghost1") || Input.GetButtonDown("Ghost2"))
            {
                m_ghostButtonTimer.Play();
            }

            if (Input.GetAxis("Change Gravity") > 0.5f)
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.CHANGE_GRAVITY);
                Debug.Log("Change Gravity");
            }

            if (Input.GetAxis("Cancel Gravity Change") > 0.5f)
            {
                m_inputs.m_buttons |= (char)InputToBit(E_INPUTS.CANCEL_GRAVITY_CHANGE);
                Debug.Log("Cancel Gravity Change");
            }

            // this sometimes gives a null refrence error when reloading the game after falling from the world, though it dosen't seem to affect gameplay
            if (m_ghostButtonTimer.m_playing)
            {
                m_ghostButtonTimer.Cycle();

                if (m_ghostButtonTimer.m_completed)
                {
                    m_ghostButtonTimer.m_playing = false;
                    //m_inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_HOLD);

                    GhostStateMachine nextGhost = GetNextGhost();

                    if (nextGhost != m_currentGhost)
                    {
                        if (m_currentGhost)
                        {
                            m_currentGhost.EndRecording();
                        }

                        nextGhost.m_overrideRecording = true;
                        ActivateGhost(nextGhost);

                        if (m_currentGhost)
                        {
                            m_ghostList.m_ghostInUse[m_currentGhost.m_id] = true;
                        }

                        m_currentGhost = nextGhost;
                    }
                    else
                    {
                        nextGhost.m_overrideRecording = true;
                        ActivateGhost(nextGhost);
                    }

                    if (nextGhost)
                    {
                        m_currentGhost = nextGhost;
                    }
                }
                else if (Input.GetButtonUp("Ghost1") || Input.GetButtonUp("Ghost2"))
                {
                    m_ghostButtonTimer.m_playing = false;
                    //m_inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_PRESS);

                    GhostStateMachine nextGhost = GetNextGhost();

                    if (!m_currentGhost)
                    {
                        ActivateGhost(nextGhost);
                    }
                    else if (m_currentGhost != nextGhost && !nextGhost.m_recorded)
                    {
                        if (m_currentGhost)
                        {
                            m_currentGhost.EndRecording();
                        }

                        ActivateGhost(nextGhost);

                        m_ghostList.m_ghostInUse[m_currentGhost.m_id] = true;
                        m_currentGhost = nextGhost;
                    }
                    else if (m_currentGhost != nextGhost)
                    {
                        ActivateGhost(nextGhost);
                    }
                    else // m_currentGhost == nextGhost
                    {
                        m_currentGhost.EndRecording();
                    }

                    if (nextGhost)
                    {
                        m_currentGhost = nextGhost;
                    }
                }
            }
        }
        else
        {
            m_inputs.m_buttons = (char)0;
        }

        if (m_currentGhost)
        {
            if (!m_currentGhost.m_recording)
            {
                m_currentGhost = null;
            }
        }

        m_player.GetInputs(new S_inputStruct(0));

        for (int z = 0; z < m_ghostList.m_ghostsCreated; z++)
        {
            m_ghostList.m_ghostStateMachines[z].GetInputs(new S_inputStruct(0));
        }

        if (m_currentGhost)
        {
            m_currentGhost.GetInputs(m_inputs);
            m_player.m_data.m_overideFollow = m_currentGhost.m_data;
        }
        else
        {
            m_player.GetInputs(m_inputs);
            m_player.m_data.m_overideFollow = null;
        }

        m_player.Cycle();

        for (int z = 0; z < m_ghostList.m_ghostsCreated; z++)
        {
            m_ghostList.m_ghostStateMachines[z].Cycle();
        }
    }

    private void FixedUpdate()
    {
        m_player.FixedCycle();

        for (int z = 0; z < m_ghostList.m_ghostsCreated; z++)
        {
            m_ghostList.m_ghostStateMachines[z].FixedCycle();
        }
    }

    public virtual bool GetInput(E_INPUTS input)
    {
        return (m_inputs.m_buttons & (char)InputToBit(input)) > 0;
    }

    /*public virtual int GetJoystickMovment(E_JOYSTICK_INPUTS input)
    {
        int magnitude = 0;

        magnitude += (m_JoystickMovement & (int)input) > 0 ? 1 : 0;
        magnitude += (m_JoystickMovement & (int)input * 2) > 0 ? 2 : 0;
        magnitude += (m_JoystickMovement & (int)input * 4) > 0 ? 4 : 0;

        return magnitude;
    }*/

    protected virtual int InputToBit(E_INPUTS input)
    {
        int bit = 1;

        for (int z = 0; z < (int)input; z++)
        {
            bit *= 2;
        }

        return bit;
    }

    private GhostStateMachine GetNextGhost()
    {
        // checks for all button up events BEFORE checking for button
        if (Input.GetButtonUp("Ghost1"))
        {
            if (m_ghostList.m_ghostStateMachines[0])
            {
                return m_ghostList.m_ghostStateMachines[0];
            }
        }

        if (Input.GetButtonUp("Ghost2"))
        {
            if (m_ghostList.m_ghostStateMachines[1])
            {
                return m_ghostList.m_ghostStateMachines[1];
            }
        }

        if (Input.GetButton("Ghost1"))
        {
            if (m_ghostList.m_ghostStateMachines[0])
            {
                return m_ghostList.m_ghostStateMachines[0];
            }
        }

        if (Input.GetButton("Ghost2"))
        {
            if (m_ghostList.m_ghostStateMachines[1])
            {
                return m_ghostList.m_ghostStateMachines[1];
            }
        }

        return null;
    }

    private void ActivateGhost(GhostStateMachine ghost)
    {
        if (m_currentGhost)
        {
            ghost.Activate(m_currentGhost.m_data.getPositionData());
        }
        else
        {
            ghost.Activate(m_player.m_data.getPositionData());
        }
    }

    private int GetThirdOfAxis(float ammount)
    {
        return Mathf.Min((int)(Mathf.Abs(ammount) / 0.3f), 7);
    }
}
