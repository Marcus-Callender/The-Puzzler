﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GhostStateMachine : BaseStateMachine
{
    //public GhostInputs m_inputs = null;
    private Renderer[] m_matirialRenderers;
    private bool m_overrideRecording = false;

    public int m_id;

    public const int m_recordingSize = 60 * 4;

    //private char m_Inputs;
    private char[] m_recordedInputs = new char[m_recordingSize];
    public int m_arrayPosition = 0;
    public bool m_recorded = false;
    public bool m_recording = false;
    public bool m_playing = false;

    public Vector3 m_startingPosition;
    public Quaternion m_startingRotation;

    public UICountdown m_countdown;

    // this boolean is set to true on entering the state and remains true 
    // while the ghost is coliding with any interactables that it isn't standing on
    //private bool m_colidedWithInteractable = false;
    //private bool m_reduceColisions = true;

    public void Activate(Transform _transform, bool use3d, bool _left_right)
    {
        gameObject.tag = "Player";

        // set layer to Ghost (reduced Collisions)
        //gameObject.layer = 11;

        //m_reduceColisions = true;
        //m_colidedWithInteractable = false;

        if (m_overrideRecording)
        {
            m_overrideRecording = false;
            m_recorded = false;
        }

        for (int z = 0; z < m_matirialRenderers.Length; z++)
        {
            m_matirialRenderers[z].material.color = new Color(0.1f, 1.0f, 1.0f, 0.5f);
        }

        // alows the ghost to animate again
        m_data.m_anim.SetBool("Stopped", false);

        if (m_recorded)
        {
            m_arrayPosition = 0;

            m_playing = true;
            m_data.m_pause = false;

            //Play();
            StartCoroutine(Play());
        }
        else
        {
            gameObject.transform.position = _transform.position;
            m_data.m_rotation = _transform.rotation;
            m_startingRotation = _transform.rotation;
            m_data.m_left_right = _left_right;

            m_data.m_use3D = use3d;
            m_arrayPosition = 0;
            m_recording = true;
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        m_matirialRenderers = gameObject.GetComponentsInChildren<Renderer>();

        for (int z = 0; z < m_matirialRenderers.Length; z++)
        {
            m_matirialRenderers[z].material.color = new Color(0.5f, 0.2f, 0.2f, 0.5f);
        }

        m_states2D[0] = gameObject.AddComponent<OnGround>();
        m_states2D[1] = gameObject.AddComponent<InAIr>();
        m_states2D[2] = gameObject.AddComponent<MoveingBox>();
        m_states2D[3] = gameObject.AddComponent<ClimbingLadder>();

        m_states2D[0].Initialize(m_rigb, m_data);
        m_states2D[1].Initialize(m_rigb, m_data);
        m_states2D[2].Initialize(m_rigb, m_data);
        m_states2D[3].Initialize(m_rigb, m_data);


        m_states3D[0] = gameObject.AddComponent<OnGround3D>();
        m_states3D[1] = gameObject.AddComponent<InAir3D>();
        //m_states3D[2] = gameObject.AddComponent<MoveingBox>();
        m_states3D[2] = m_states2D[2];
        m_states3D[3] = m_states2D[3];
        //m_states3D[7] = gameObject.AddComponent<WallSlide>();

        m_states3D[0].Initialize(m_rigb, m_data);
        m_states3D[1].Initialize(m_rigb, m_data);
        //m_states3D[2].Initialize(m_rigb, m_data, m_inputs);
        //m_states3D[3].Initialize(m_rigb, m_data, m_inputs);
        //m_states3D[7].Initialize(m_rigb, m_data, m_inputs);

        m_data.m_anim.SetBool("Stopped", true);

        if (GameObject.Find("GhostCountdown"))
        {
            Debug.Log("-- found GhostCountdown");

            m_countdown = GameObject.Find("GhostCountdown").GetComponent<UICountdown>();

            if (m_countdown)
            {
                Debug.Log("-- found UICountdown");
            }
        }
    }

    public override void Cycle()
    {
        if (transform.position.y < -10.0f)
        {
            gameObject.tag = "Ghost";
            // runs if the recording has finished and the ghost is not playing
            m_data.m_anim.SetBool("Stopped", true);
            //m_inputs.m_pauseInputs = true;
            m_data.m_squished = false;

            if (!m_recorded)
            {
                m_overrideRecording = true;
            }

            Stop();

            m_recorded = true;
        }

        if (!m_data.m_pause)
        {
            if (m_recording)
            {
                if (GetInput(E_INPUTS.GHOST_BUTTON_PRESS))
                {
                    m_recordedInputs[m_arrayPosition] = (char)InputToBit(E_INPUTS.END);

                    m_recorded = true;
                    m_recording = false;

                    m_countdown.m_progress = 0.0f;

                    gameObject.transform.position = m_startingPosition;
                }
                else if (m_arrayPosition < m_recordingSize)
                {
                    if (m_arrayPosition == 0)
                    {
                        // if the recording has just started mark the current position
                        m_startingPosition = gameObject.transform.position;
                        m_startingRotation = gameObject.transform.rotation;
                    }

                    m_recordedInputs[m_arrayPosition] = m_data.m_inputs;
                    m_arrayPosition++;

                    if (m_countdown)
                    {
                        m_countdown.m_progress = 1.0f - ((float)m_arrayPosition / (float)m_recordingSize);
                    }
                }
                else
                {
                    m_recorded = true;
                    m_recording = false;
                    m_data.m_pause = false;
    
                    gameObject.transform.position = m_startingPosition;
                    gameObject.transform.rotation = m_startingRotation;
                }
            }
            else if (m_playing)
            {
                m_data.m_pause = false;

                if (m_arrayPosition < m_recordingSize && (m_recordedInputs[m_arrayPosition] != (char)InputToBit(E_INPUTS.END)))
                {
                    m_data.m_inputs = m_recordedInputs[m_arrayPosition];
                    m_arrayPosition++;
                }
                else
                {
                    m_playing = false;

                    gameObject.transform.position = m_startingPosition;
                    gameObject.transform.rotation = m_startingRotation;
                }
            }
        }


        m_data.m_pressingButton = GetInput(E_INPUTS.PRESS_BUTTON);

        if (!m_recorded && !m_recording && !m_playing)
        {
            m_data.m_squished = false;
        }

        if (m_recorded && !m_playing)
        {
            gameObject.tag = "Ghost";
            // runs if the recording has finished and the ghost is not playing
            m_data.m_anim.SetBool("Stopped", true);
            //m_inputs.m_pauseInputs = true;
            //m_inputs.m_pause = true;
            m_data.m_pause = true;
        }

        base.Cycle();
    }

    public override void OnTriggerStay(Collider other)
    {
        if (m_recorded && other.gameObject.tag == "Attack")
        {
            m_data.m_squished = true;
        }

        base.OnTriggerStay(other);
    }

    public override void Pause(bool paused)
    {
        base.Pause(paused);
    }

    public void Stop()
    {
        if (m_recorded == false)
        {
            for (int z = 0; z < m_recordingSize; z++)
            {
                m_recordedInputs[z] = (char)0;
            }
        }

        m_arrayPosition = 0;

        m_recording = false;
        m_playing = false;

        gameObject.transform.position = m_startingPosition;
        gameObject.transform.rotation = m_startingRotation;
    }

    public IEnumerator Play()
    {
        if (!m_data.m_pause)
        {
            if (m_arrayPosition > m_recordingSize)
            {
                m_data.m_inputs = m_recordedInputs[m_arrayPosition];
                m_arrayPosition++;
                yield return new WaitForSeconds(0.016f);
            }
        }

        // repetes tis function automatialy on the next frame
        yield return new WaitForSeconds(0.016f);
    }
}
