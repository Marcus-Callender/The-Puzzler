﻿using System.Collections;
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
                    //m_Inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_HOLD);

                    if (m_currentGhost == null)
                    {
                        if (Input.GetButton("Ghost1"))
                        {
                            if (m_ghostList.m_ghostStateMachines[0])
                            {
                                m_currentGhost = m_ghostList.m_ghostStateMachines[0];
                            }
                        }
                        else if (Input.GetButton("Ghost2"))
                        {
                            if (m_ghostList.m_ghostStateMachines[1])
                            {
                                m_currentGhost = m_ghostList.m_ghostStateMachines[1];
                            }
                        }

                        if (m_currentGhost)
                        {
                            m_currentGhost.m_overrideRecording = true;
                            m_currentGhost.Activate(m_player.transform, m_player.m_data.m_use3D, m_player.m_data.m_left_right);
                        }
                    }
                    else
                    {
                        GhostStateMachine nextGhost = null;

                        if (Input.GetButton("Ghost1"))
                        {
                            if (m_ghostList.m_ghostStateMachines[0])
                            {
                                nextGhost = m_ghostList.m_ghostStateMachines[0];
                            }
                        }
                        else if (Input.GetButton("Ghost2"))
                        {
                            if (m_ghostList.m_ghostStateMachines[1])
                            {
                                nextGhost = m_ghostList.m_ghostStateMachines[1];
                            }
                        }

                        if (m_currentGhost == nextGhost && !m_ghostList.m_ghostInUse[nextGhost.m_id])
                        {
                            m_currentGhost.EndRecording();

                            nextGhost.m_overrideRecording = true;
                            nextGhost.Activate(m_currentGhost.transform, m_currentGhost.m_data.m_use3D, m_currentGhost.m_data.m_left_right);

                            m_ghostList.m_ghostInUse[m_currentGhost.m_id] = true;
                            m_currentGhost = nextGhost;
                        }
                    }
                }
                else if (Input.GetButtonUp("Ghost1") || Input.GetButtonUp("Ghost2"))
                {
                    m_ghostButtonTimer.m_playing = false;
                    //m_Inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_PRESS);

                    GhostStateMachine nextGhost = GetNextGhost();

                    if (m_currentGhost == null)
                    {
                        if (Input.GetButtonUp("Ghost1"))
                        {
                            if (m_ghostList.m_ghostStateMachines[0])
                            {
                                m_currentGhost = m_ghostList.m_ghostStateMachines[0];
                            }
                        }
                        else if (Input.GetButtonUp("Ghost2"))
                        {
                            if (m_ghostList.m_ghostStateMachines[1])
                            {
                                m_currentGhost = m_ghostList.m_ghostStateMachines[1];
                            }
                        }

                        if (m_currentGhost)
                        {
                            m_currentGhost.Activate(m_player.transform, m_player.m_data.m_use3D, m_player.m_data.m_left_right);
                        }
                    }
                    else
                    {
                        if (m_currentGhost.m_recording)
                        {
                            m_currentGhost.EndRecording();
                        }
                    }
                }

                //if (Input.GetButton("Ghost1"))
                //{
                //    m_Inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_1);
                //}
                //else if (Input.GetButton("Ghost2"))
                //{
                //    m_Inputs |= (char)InputToBit(E_INPUTS.GHOST_BUTTON_2);
                //}
            }
        }
        else
        {
            m_Inputs = (char)0;
        }

        //m_currentGhost = null;
        //
        //for (int z = 0; z < m_ghostList.m_ghostsCreated; z++)
        //{
        //    if (m_ghostList.m_ghostStateMachines[z].m_recording)
        //    {
        //        m_currentGhost = m_ghostList.m_ghostStateMachines[z];
        //    }
        //}

        if (m_currentGhost)
        {
            if (!m_currentGhost.m_recording)
            {
                m_currentGhost = null;
            }
        }

        m_player.GetInputs((char)0);

        for (int z = 0; z < m_ghostList.m_ghostsCreated; z++)
        {
            m_ghostList.m_ghostStateMachines[z].GetInputs((char)0);
        }

        if (m_currentGhost)
        {
            m_currentGhost.GetInputs(m_Inputs);
            m_player.m_data.m_overideFollow = m_currentGhost.m_data;
        }
        else
        {
            m_player.GetInputs(m_Inputs);
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
}
