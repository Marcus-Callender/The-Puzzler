using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlingGhost : BasicState
{
    public GameObject m_GhostObject;
    public GhostStateMachine m_ghostStateMachine;
    public GhostInputs m_ghostInputs;

    public GhostList m_ghostList;

    public override void Initialize(Rigidbody rigb, PlayerData data, PlayerInputs inputs)
    {
        m_rigb = rigb;
        m_data = data;
        m_inputs = inputs;

        Debug.Log("###### Creating ghost ######");
        //m_GhostObject = Instantiate(m_data.m_ghost);
        //m_GhostObject.SetActive(true);
        m_GhostObject = m_ghostList.createGhost();
        m_ghostStateMachine = m_GhostObject.GetComponent<GhostStateMachine>();
        m_ghostInputs = m_GhostObject.GetComponent<GhostInputs>();
    }

    public override void Enter()
    {
        if (m_ghostInputs == null)
        {
            // ensures there is a valid refrence to m_ghostInputs
            m_ghostInputs = m_GhostObject.GetComponent<GhostInputs>();
        }

        if (m_inputs.GetInput(E_INPUTS.GHOST_BUTTON_HOLD) && m_ghostInputs.m_recorded)
        {
            // tels the ghostInputs to re-record there recording
            m_ghostInputs.Reset();
        }

        if (m_ghostInputs.m_recorded)
        {
            //m_inputs.m_pauseInputs = false;
            m_inputs.m_pause = false;
        }
        else
        {
            // sets the camra to follow the active ghost
            m_data.m_overideFollow = m_ghostStateMachine.m_data;
            //m_inputs.m_pauseInputs = true;
            m_inputs.m_pause = true;
        }

        m_data.m_velocityX = 0.0f;

        m_ghostStateMachine.Activate(gameObject.transform, m_data.m_use3D, m_data.m_left_right);
    }

    public override void Exit()
    {
        // returns the camera to following the player
        m_data.m_overideFollow = null;
    }

    public override E_PLAYER_STATES Cycle()
    {
        if (!m_ghostInputs.m_consumingInputs)
        {
            //m_inputs.m_pauseInputs = false;
            m_inputs.m_pause = false;
            return E_PLAYER_STATES.IN_AIR;
        }
        
        return E_PLAYER_STATES.NULL;
    }
}
