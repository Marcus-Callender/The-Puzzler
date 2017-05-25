using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GhostStateMachine : BaseStateMachine
{
    public GhostInputs m_inputs = null;
    private Renderer[] m_matirialRenderers;

    public void Activate(Vector3 pos)
    {
        gameObject.tag = "Player";

        for (int z = 0; z < m_matirialRenderers.Length; z++)
        {
            m_matirialRenderers[z].material.color = new Color(0.1f, 1.0f, 1.0f, 1.0f);
        }

        // alows the ghost to animate again
        m_data.m_anim.SetBool("Stopped", false);

        if (m_inputs.m_recorded)
        {
            m_inputs.m_arrayPosition = 0;
            //IEnumerator couroutine = m_inputs.Play();
            //StartCoroutine(couroutine);
            m_inputs.m_playing = true;
        }
        else
        {
            gameObject.transform.position = pos;
            m_inputs.m_arrayPosition = 0;
            m_inputs.m_recording = true;
        }
    }
    
    public override void Start()
    {
        base.Start();

        m_matirialRenderers = gameObject.GetComponentsInChildren<Renderer>();

        for (int z = 0; z < m_matirialRenderers.Length; z++)
        {
            m_matirialRenderers[z].material.color = new Color(0.5f, 0.2f, 0.2f, 1.0f);
        }

        m_inputs = gameObject.AddComponent<GhostInputs>();

        m_states[0] = gameObject.AddComponent<OnGround>();
        m_states[1] = gameObject.AddComponent<InAIr>();
        m_states[2] = gameObject.AddComponent<MoveingBox>();
        m_states[3] = gameObject.AddComponent<ClimbingLadder>();

        m_states[0].Initialize(m_rigb, m_data, m_inputs);
        m_states[1].Initialize(m_rigb, m_data, m_inputs);
        m_states[2].Initialize(m_rigb, m_data, m_inputs);
        m_states[3].Initialize(m_rigb, m_data, m_inputs);

        m_data.m_anim.SetBool("Stopped", true);
    }

    public override void Update()
    {
        m_inputs.Cycle();

        base.Update();

        if (m_inputs.m_recorded && m_inputs.m_arrayPosition == GhostInputs.m_recordingSize)
        {
            gameObject.tag = "Ghost";
            // runs if the recording has finished and the ghost is not playing
            m_data.m_anim.SetBool("Stopped", true);
            m_inputs.m_pauseInputs = true;
        }
    }
}
