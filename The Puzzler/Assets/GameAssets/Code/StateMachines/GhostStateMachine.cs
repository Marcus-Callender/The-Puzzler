using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GhostStateMachine : BaseStateMachine
{
    //public GhostInputs m_inputs = null;
    private Renderer[] m_matirialRenderers;
    public bool m_overrideRecording = false;

    public int m_id;

    public const int m_recordingSize = 60 * 6;

    //private char m_Inputs;
    private char[] m_recordedInputs = new char[m_recordingSize];
    private char[] m_recordedJoystickPositions = new char[m_recordingSize];
    public int m_arrayPosition = 0;
    public bool m_recorded = false;
    public bool m_recording = false;
    public bool m_playing = false;

    public Vector3 m_startingPosition;
    public Quaternion m_startingRotation;

    public UICountdown m_countdown;

    private List<IGhostInteractable> m_inteactions;

    // this boolean is set to true on entering the state and remains true 
    // while the ghost is coliding with any interactables that it isn't standing on
    //private bool m_colidedWithInteractable = false;
    //private bool m_reduceColisions = true;

    public void Activate(CharicterPosition posData)
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
            m_countdown.m_progress = 1.0f;
            m_data.m_pause = false;
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
            
            StartCoroutine(Play());
        }
        else
        {
            gameObject.transform.position = posData.pos;
            gameObject.transform.rotation = posData.rot;
            m_data.m_cameraRotation = posData.rot;
            m_data.m_rotation = posData.rot;
            m_data.m_left_right = posData.left_right;

            m_data.m_use3D = posData.use3D;
            m_arrayPosition = 0;
            m_recording = true;
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        m_inteactions = new List<IGhostInteractable>();

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
        m_states3D[2] = m_states2D[2];
        m_states3D[3] = m_states2D[3];

        m_states3D[0].Initialize(m_rigb, m_data);
        m_states3D[1].Initialize(m_rigb, m_data);

        m_data.m_anim.SetBool("Stopped", true);

        if (GameObject.Find("GhostCountdown"))
        {
            //Debug.Log("-- found GhostCountdown");

            m_countdown = GameObject.Find("GhostCountdown").GetComponent<UICountdown>();

            if (m_countdown)
            {
                //Debug.Log("-- found UICountdown");
            }
        }
    }

    public override void Cycle()
    {
        if (transform.position.y < -40.0f)
        {
            gameObject.tag = "Ghost";
            // runs if the recording has finished and the ghost is not playing
            m_data.m_anim.SetBool("Stopped", true);
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

                    foreach (IGhostInteractable interaction in m_inteactions)
                    {
                        if (interaction != null)
                            interaction.StopIntecation();
                    }

                    m_recorded = true;
                    m_recording = false;

                    m_countdown.m_progress = 0.0f;

                    gameObject.transform.position = m_startingPosition;
                    gameObject.transform.rotation = m_startingRotation;

                    m_data.m_cameraRotation = m_startingRotation;
                    m_data.m_rotation = m_startingRotation;
                }
                else if (m_arrayPosition < m_recordingSize)
                {
                    if (m_arrayPosition == 0)
                    {
                        // if the recording has just started mark the current position
                        m_startingPosition = gameObject.transform.position;
                        m_startingRotation = gameObject.transform.rotation;
                    }

                    m_recordedInputs[m_arrayPosition] = m_inputs;
                    m_recordedJoystickPositions[m_arrayPosition] = m_JoystickMovement;
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

                    foreach (IGhostInteractable interaction in m_inteactions)
                    {
                        if (interaction != null)
                            interaction.StopIntecation();
                    }

                    gameObject.transform.position = m_startingPosition;
                    gameObject.transform.rotation = m_startingRotation;

                    m_data.m_cameraRotation = m_startingRotation;
                    m_data.m_rotation = m_startingRotation;
                }
            }
            else if (m_playing)
            {
                m_data.m_pause = false;

                if (m_arrayPosition < m_recordingSize && (m_recordedInputs[m_arrayPosition] != (char)InputToBit(E_INPUTS.END)))
                {
                    m_inputs = m_recordedInputs[m_arrayPosition];
                    m_JoystickMovement = m_recordedJoystickPositions[m_arrayPosition];
                    m_arrayPosition++;
                }
                else
                {
                    // the recording has finished playing
                    m_playing = false;

                    foreach (IGhostInteractable interaction in m_inteactions)
                    {
                        if (interaction != null)
                            interaction.StopIntecation();
                    }

                    m_inteactions.Clear();

                    gameObject.transform.position = m_startingPosition;
                    gameObject.transform.rotation = m_startingRotation;

                    m_data.m_cameraRotation = m_startingRotation;
                    m_data.m_rotation = m_startingRotation;
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
                m_recordedJoystickPositions[z] = (char)0;
            }
        }

        m_arrayPosition = 0;

        m_recording = false;
        m_playing = false;

        gameObject.transform.position = m_startingPosition;
        gameObject.transform.rotation = m_startingRotation;

        m_data.m_cameraRotation = m_startingRotation;
        m_data.m_rotation = m_startingRotation;
    }

    public IEnumerator Play()
    {
        if (!m_data.m_pause)
        {
            if (m_arrayPosition > m_recordingSize)
            {
                m_inputs = m_recordedInputs[m_arrayPosition];
                m_JoystickMovement = m_recordedJoystickPositions[m_arrayPosition];
                m_arrayPosition++;
                yield return new WaitForSeconds(0.016f);
            }
        }

        // repetes tis function automatialy on the next frame
        yield return new WaitForSeconds(0.016f);
    }

    public void EndRecording()
    {
        // adds a symbol to the end of the recording so the playback knows when the recording ends
        m_recordedInputs[m_arrayPosition] = (char)InputToBit(E_INPUTS.END);

        foreach (IGhostInteractable interaction in m_inteactions)
        {
            if (interaction != null)
                interaction.StopIntecation();
        }

        m_inteactions.Clear();

        m_recorded = true;
        m_recording = false;

        m_countdown.m_progress = 0.0f;

        gameObject.transform.position = m_startingPosition;
        gameObject.transform.rotation = m_startingRotation;

        m_data.m_cameraRotation = m_startingRotation;
        m_data.m_rotation = m_startingRotation;
    }

    protected override void CheckState()
    {
        // prevents the state from changing
        if (!m_lockState)
        {
            m_data.m_anim.SetBool("KOd", m_data.m_squished);

            if (m_data.m_squished)
            {
                m_newState = E_PLAYER_STATES.KO;
            }

            if (m_newState != E_PLAYER_STATES.NULL && m_newState != m_currentState && m_states2D[(int)m_newState])
            {
                /*if (m_currentState == E_PLAYER_STATES.MOVEING_BLOCK)
                {
                    GetCurrentState().GhostSpecialExit();

                }*/
                
                // tells the old state is is being left and the new state is being entered
                GetCurrentState().Exit();
                m_states2D[(int)m_newState].Enter();
                
                if (m_newState == E_PLAYER_STATES.MOVEING_BLOCK)
                {
                    m_inteactions.Add(m_states2D[(int)m_newState].GhostSpecialEnter());
                }

                // shows the state transition that took place
                //Debug.Log(m_currentState + " -> " + m_newState);

                // sets the new state to be used
                m_currentState = m_newState;
            }
        }
    }
}
