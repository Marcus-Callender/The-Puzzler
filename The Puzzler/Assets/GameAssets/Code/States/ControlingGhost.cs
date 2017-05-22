﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlingGhost : BasicState
{
    public GameObject m_GhostObject;
    public GhostStateMachine m_ghostStateMachine;
    public GhostInputs m_ghostInputs;

    public override void Initialize(Rigidbody rigb, PlayerData data, PlayerInputs inputs)
    {
        m_rigb = rigb;
        m_data = data;
        m_inputs = inputs;

        m_GhostObject = Instantiate(m_data.m_ghost);
        m_GhostObject.SetActive(true);
        m_ghostStateMachine = m_GhostObject.GetComponent<GhostStateMachine>();
        m_ghostInputs = m_GhostObject.GetComponent<GhostInputs>();
    }

    public override void Enter()
    {
        if (m_ghostInputs == null)
        {
            m_ghostInputs = m_GhostObject.GetComponent<GhostInputs>();
        }

        if (m_ghostInputs.m_recorded)
        {
            m_inputs.m_pauseInputs = false;
            m_ghostStateMachine.Activate(gameObject.transform.position);
        }
        else
        {
            m_inputs.m_pauseInputs = true;
            m_ghostStateMachine.Activate(gameObject.transform.position);
        }
    }

    public override E_PLAYER_STATES Cycle()
    {
        if (m_ghostInputs.m_recorded)
        {
            m_inputs.m_pauseInputs = false;
            return E_PLAYER_STATES.IN_AIR;
        }
        
        return E_PLAYER_STATES.CONTROLING_GHOST;
    }
}