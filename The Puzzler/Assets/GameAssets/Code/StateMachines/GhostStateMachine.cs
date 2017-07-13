using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GhostStateMachine : BaseStateMachine
{
    public GhostInputs m_inputs = null;
    private Renderer[] m_matirialRenderers;
    private bool m_overrideRecording = false;

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
            m_inputs.m_recorded = false;
        }

        for (int z = 0; z < m_matirialRenderers.Length; z++)
        {
            m_matirialRenderers[z].material.color = new Color(0.1f, 1.0f, 1.0f, 0.5f);
        }
        
        // alows the ghost to animate again
        m_data.m_anim.SetBool("Stopped", false);

        if (m_inputs.m_recorded)
        {
            m_inputs.m_arrayPosition = 0;

            m_inputs.m_playing = true;
        }
        else
        {
            gameObject.transform.position = _transform.position;
            m_data.m_rotation = _transform.rotation;
            m_inputs.m_startingRotation = _transform.rotation;
            m_data.m_left_right = _left_right;

            m_data.m_use3D = use3d;
            m_inputs.m_arrayPosition = 0;
            m_inputs.m_recording = true;
            m_inputs.m_consumingInputs = true;
        }
    }

    public override void Start()
    {
        base.Start();

        m_matirialRenderers = gameObject.GetComponentsInChildren<Renderer>();

        for (int z = 0; z < m_matirialRenderers.Length; z++)
        {
            m_matirialRenderers[z].material.color = new Color(0.5f, 0.2f, 0.2f, 0.5f);
        }

        m_inputs = gameObject.GetComponent<GhostInputs>();

        m_states2D[0] = gameObject.AddComponent<OnGround>();
        m_states2D[1] = gameObject.AddComponent<InAIr>();
        m_states2D[2] = gameObject.AddComponent<MoveingBox>();
        m_states2D[3] = gameObject.AddComponent<ClimbingLadder>();

        m_states2D[0].Initialize(m_rigb, m_data, m_inputs);
        m_states2D[1].Initialize(m_rigb, m_data, m_inputs);
        m_states2D[2].Initialize(m_rigb, m_data, m_inputs);
        m_states2D[3].Initialize(m_rigb, m_data, m_inputs);


        m_states3D[0] = gameObject.AddComponent<OnGround3D>();
        m_states3D[1] = gameObject.AddComponent<InAir3D>();
        //m_states3D[2] = gameObject.AddComponent<MoveingBox>();
        m_states3D[2] = m_states2D[2];
        m_states3D[3] = m_states2D[3];
        //m_states3D[7] = gameObject.AddComponent<WallSlide>();

        m_states3D[0].Initialize(m_rigb, m_data, m_inputs);
        m_states3D[1].Initialize(m_rigb, m_data, m_inputs);
        //m_states3D[2].Initialize(m_rigb, m_data, m_inputs);
        //m_states3D[3].Initialize(m_rigb, m_data, m_inputs);
        //m_states3D[7].Initialize(m_rigb, m_data, m_inputs);

        m_data.m_anim.SetBool("Stopped", true);
    }

    public override void Update()
    {
        if (transform.position.y < -10.0f)
        {
            gameObject.tag = "Ghost";
            // runs if the recording has finished and the ghost is not playing
            m_data.m_anim.SetBool("Stopped", true);
            m_inputs.m_pauseInputs = true;
            m_data.m_squished = false;

            if (!m_inputs.m_recorded)
            {
                m_overrideRecording = true;
            }

            m_inputs.Stop();
            
            m_inputs.m_recorded = true;
        }

        m_inputs.Cycle();

        m_data.m_pressingButton = m_inputs.GetInput(E_INPUTS.PRESS_BUTTON);

        base.Update();

        if (!m_inputs.m_recorded && !m_inputs.m_recording && !m_inputs.m_playing)
        {
            m_data.m_squished = false;
        }

        if (m_inputs.m_recorded && !m_inputs.m_playing)
        {
            gameObject.tag = "Ghost";
            // runs if the recording has finished and the ghost is not playing
            m_data.m_anim.SetBool("Stopped", true);
            m_inputs.m_pauseInputs = true;
            m_inputs.m_consumingInputs = false;
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        if (m_inputs.m_recorded && other.gameObject.tag == "Attack")
        {
            m_data.m_squished = true;
        }

        base.OnTriggerStay(other);
    }
}
