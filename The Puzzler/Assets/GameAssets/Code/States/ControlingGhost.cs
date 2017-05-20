using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlingGhost : BasicState
{
    private bool m_ghostRecorded = false;
    private GameObject m_ghost = null;
    private GhostData m_ghostData = null;

    public GameObject m_GhostObject;

    void Start()
    {
        //m_ghost = Instantiate(gameObject);
        //m_ghostData = m_ghost.AddComponent<GhostData>();


    }

    public override void Initialize(Rigidbody rigb, PlayerData data, PlayerInputs inputs)
    {
        m_rigb = rigb;
        m_data = data;

        m_GhostObject = m_data.m_ghost;
    }

    public override void Enter()
    {
        if (!m_ghostRecorded)
        {

        }


    }

    public override E_PLAYER_STATES Cycle()
    {
        if (!m_ghostRecorded)
        {
            m_ghostData.StartRecording();

            //if (Input.GetButtonUp("GhostControl"))
            {
                m_ghostRecorded = true;
                return E_PLAYER_STATES.IN_AIR;
            }
        }
        else
        {

        }

        return E_PLAYER_STATES.CONTROLING_GHOST;
    }
}
