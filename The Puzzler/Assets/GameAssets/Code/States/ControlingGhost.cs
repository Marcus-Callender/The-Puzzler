using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlingGhost : BasicState
{
    public GameObject m_GhostObject;
    public GhostStateMachine m_ghostStateMachine;
    //public GhostInputs m_ghostInputs;

    public GhostList m_ghostList;

    public override void Initialize(Rigidbody rigb, PlayerData data)
    {
        m_rigb = rigb;
        m_data = data;

        Debug.Log("###### Creating ghost ######");
        //m_GhostObject = Instantiate(m_data.m_ghost);
        //m_GhostObject.SetActive(true);
        m_GhostObject = m_ghostList.createGhost();
        m_ghostStateMachine = m_GhostObject.GetComponent<GhostStateMachine>();
        //m_ghostInputs = m_GhostObject.GetComponent<GhostInputs>();
    }

    public override void Enter()
    {
        //if (m_ghostInputs.m_recorded)
        //{
        //    //m_inputs.m_pauseInputs = false;
        //}
        //else
        //{
        //    // sets the camra to follow the active ghost
        //    m_data.m_overideFollow = m_ghostStateMachine.m_data;
        //    //m_inputs.m_pauseInputs = true;
        //}

        m_data.m_velocityX = 0.0f;

        m_ghostStateMachine.Activate(m_data.getPositionData());
    }

    public override void Exit()
    {
        // returns the camera to following the player
        m_data.m_overideFollow = null;
    }

    public override E_PLAYER_STATES Cycle(char inputs)
    {
        //if (!m_ghostInputs.m_consumingInputs)
        if (!m_data.m_pause)
        {
            //m_inputs.m_pauseInputs = false;
            return E_PLAYER_STATES.IN_AIR;
        }
        
        return E_PLAYER_STATES.NULL;
    }
}
