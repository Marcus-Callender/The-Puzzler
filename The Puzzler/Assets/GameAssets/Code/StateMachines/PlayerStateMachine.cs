using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public enum E_PLAYER_STATES
{
    ON_GROUND,
    IN_AIR,

    MOVEING_BLOCK,
    USEING_LADDER,
    KO,

    SIZE,
    NULL
}

public enum E_DIRECTIONS
{
    TOP,
    BOTTOM,
    LEFT,
    RIGHT
}

public class PlayerStateMachine : BaseStateMachine
{
    private GhostList m_ghostList;
    private SaveData m_saveData;

    public override void Initialize()
    {
        base.Initialize();

        m_ghostList = gameObject.AddComponent<GhostList>();
        m_ghostList.m_ghostTemplate = m_data.m_ghost;

        m_states2D[0] = gameObject.AddComponent<OnGround>();
        m_states2D[1] = gameObject.AddComponent<InAIr>();
        m_states2D[3] = gameObject.AddComponent<ClimbingLadder>();
        m_states2D[4] = gameObject.AddComponent<KO>();

        m_states2D[0].Initialize(m_rigb, m_data);
        m_states2D[1].Initialize(m_rigb, m_data);
        m_states2D[3].Initialize(m_rigb, m_data);
        m_states2D[4].Initialize(m_rigb, m_data);


        m_states3D[0] = gameObject.AddComponent<OnGround3D>();
        m_states3D[1] = gameObject.AddComponent<InAir3D>();
        m_states3D[3] = m_states2D[3];
        m_states3D[4] = m_states2D[4];

        m_states3D[0].Initialize(m_rigb, m_data);
        m_states3D[1].Initialize(m_rigb, m_data);

        m_saveData = GetComponent<SaveData>();
        m_saveData.Initialize();

        if (m_saveData.m_upgradeArray[(int)E_UPGRADES.MOVE_CRATE])
        {
            if (!m_states2D[2])
            {
                m_states2D[2] = gameObject.AddComponent<MoveingBox>();
                m_states2D[2].Initialize(m_rigb, m_data);
                m_states3D[2] = m_states2D[2];
            }
        }

        if (m_saveData.m_upgradeArray[(int)E_UPGRADES.GHOST_1])
        {
            m_ghostList.createGhost();
        }

        if (m_saveData.m_upgradeArray[(int)E_UPGRADES.GHOST_2])
        {
            m_ghostList.createGhost();
        }
    }

    public override void Cycle()
    {
        if (transform.position.y < -40.0f)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        m_data.m_pressingButton = GetInput(E_INPUTS.PRESS_BUTTON);

        base.Cycle();
    }

    public PlayerData getFollowData()
    {
        if (m_data.m_overideFollow)
        {
            return m_data.m_overideFollow;
        }

        return m_data;
    }

    public override void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            m_data.m_squished = true;
        }

        base.OnTriggerStay(other);
    }

    public override void Pause(bool paused)
    {
        base.Pause(paused);

        m_ghostList.Pause(paused);
    }

    public void Upgrade(E_UPGRADES type)
    {
        if (type == E_UPGRADES.MOVE_CRATE)
        {
            if (!m_states2D[2])
            {
                m_states2D[2] = gameObject.AddComponent<MoveingBox>();
                m_states2D[2].Initialize(m_rigb, m_data);
                m_states3D[2] = m_states2D[2];

                m_saveData.AddUpgrade(type);
            }
        }
        else if (type == E_UPGRADES.GHOST_1)
        {
            // makes sure the player hasn't already got this upgrade
            if (m_ghostList.m_ghostsCreated < 1)
            {
                m_ghostList.createGhost();
            }
        }
        else if (type == E_UPGRADES.GHOST_2)
        {
            if (m_ghostList.m_ghostsCreated < 2)
            {
                m_ghostList.createGhost();
            }
        }
    }
}
