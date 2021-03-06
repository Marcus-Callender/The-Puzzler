﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostList : MonoBehaviour
{
    public GameObject[] m_ghosts = new GameObject[4];
    public GhostStateMachine[] m_ghostStateMachines = new GhostStateMachine[4];
    //public GhostInputs[] m_ghostInputs = new GhostInputs[4];
    public bool[] m_ghostInUse = new bool[4];
    public int m_ghostsCreated = 0;

    public GameObject m_ghostTemplate;
    
    // creates a new ghost, records its data and returns a refrence to it
    public GameObject createGhost()
    {
        m_ghosts[m_ghostsCreated] = Instantiate(m_ghostTemplate);
        m_ghosts[m_ghostsCreated].SetActive(true);
        m_ghostStateMachines[m_ghostsCreated] = m_ghosts[m_ghostsCreated].GetComponent<GhostStateMachine>();
        m_ghostStateMachines[m_ghostsCreated].Initialize();
        //m_ghostInputs[m_ghostsCreated] = m_ghosts[m_ghostsCreated].GetComponent<GhostInputs>();

        m_ghostStateMachines[m_ghostsCreated].m_id = m_ghostsCreated;
        m_ghostsCreated++;

        return m_ghosts[m_ghostsCreated - 1];
    }

    public void Pause(bool paused)
    {
        // tells al the ghosts to pause
        for (int z = 0; z < m_ghostsCreated; z++)
        {
            //m_ghostInputs[z].m_pause = paused;
            m_ghostStateMachines[z].m_data.m_pause = paused;
        }
    }

    public void ResetGhostChain()
    {
        for (int z = 0; z < m_ghostsCreated; z++)
        {
            m_ghostInUse[z] = false;
        }
    }
}
