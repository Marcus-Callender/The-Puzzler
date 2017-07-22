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

    CONTROLING_GHOST,

    DOUBLE_JUMPING,
    WALL_SLIDEING,

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
    public PlayerInputs m_inputs;
    private GhostList m_ghostList;
    private SaveData m_saveData;

    public override void Start()
    {
        base.Start();

        m_inputs = gameObject.AddComponent<PlayerInputs>();
        m_ghostList = gameObject.AddComponent<GhostList>();
        m_ghostList.m_ghostTemplate = m_data.m_ghost;

        m_states2D[0] = gameObject.AddComponent<OnGround>();
        m_states2D[1] = gameObject.AddComponent<InAIr>();
        m_states2D[2] = gameObject.AddComponent<MoveingBox>();
        m_states2D[3] = gameObject.AddComponent<ClimbingLadder>();
        m_states2D[4] = gameObject.AddComponent<KO>();
        //ControlingGhost temp = gameObject.AddComponent<ControlingGhost>();
        //temp.m_ghostList = m_ghostList;
        //m_states2D[5] = temp;
        //m_states2D[7] = gameObject.AddComponent<WallSlide>();

        m_states2D[0].Initialize(m_rigb, m_data, m_inputs);
        m_states2D[1].Initialize(m_rigb, m_data, m_inputs);
        m_states2D[2].Initialize(m_rigb, m_data, m_inputs);
        m_states2D[3].Initialize(m_rigb, m_data, m_inputs);
        m_states2D[4].Initialize(m_rigb, m_data, m_inputs);
        //m_states2D[5].Initialize(m_rigb, m_data, m_inputs);
        //m_states2D[7].Initialize(m_rigb, m_data, m_inputs);


        m_states3D[0] = gameObject.AddComponent<OnGround3D>();
        m_states3D[1] = gameObject.AddComponent<InAir3D>();
        m_states3D[2] = m_states2D[2];
        m_states3D[3] = m_states2D[3];
        m_states3D[4] = m_states2D[4];

        // the controling ghost state is 2d/3d agnostic so the 2d/3d arrays can have a pointer to the same object
        //m_states3D[5] = m_states2D[5];

        //m_states3D[7] = gameObject.AddComponent<WallSlide>();

        m_states3D[0].Initialize(m_rigb, m_data, m_inputs);
        m_states3D[1].Initialize(m_rigb, m_data, m_inputs);

        //m_states3D[7].Initialize(m_rigb, m_data, m_inputs);

        m_saveData = GetComponent<SaveData>();
        m_saveData.Initialize();

        if (m_saveData.m_upgradeArray[(int)E_UPGRADES.MOVE_CRATE])
        {

        }

        if (m_saveData.m_upgradeArray[(int)E_UPGRADES.GHOST_1])
        {
            if (!m_states2D[5])
            {
                ControlingGhost temp = gameObject.AddComponent<ControlingGhost>();
                temp.m_ghostList = m_ghostList;
                m_states2D[5] = temp;

                m_states2D[5].Initialize(m_rigb, m_data, m_inputs);

                m_states3D[5] = m_states2D[5];
            }
        }

        if (m_saveData.m_upgradeArray[(int)E_UPGRADES.GHOST_2])
        {

        }
    }

    public override void Update()
    {
        if (transform.position.y < -10.0f)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        m_inputs.Cycle();

        m_data.m_pressingButton = m_inputs.GetInput(E_INPUTS.PRESS_BUTTON);

        base.Update();

        if (m_inputs.GetInput(E_INPUTS.GHOST_BUTTON_PRESS))
        {
            m_newState = E_PLAYER_STATES.CONTROLING_GHOST;
            CheckState();
        }
        else if (m_inputs.GetInput(E_INPUTS.GHOST_BUTTON_HOLD))
        {
            m_newState = E_PLAYER_STATES.CONTROLING_GHOST;
            CheckState();
        }
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

        m_inputs.m_pause = paused;
        m_ghostList.Pause(paused);
    }

    public void Upgrade(E_UPGRADES type)
    {
        if (type == E_UPGRADES.GHOST_1)
        {
            if (!m_states2D[5])
            {
                ControlingGhost temp = gameObject.AddComponent<ControlingGhost>();
                temp.m_ghostList = m_ghostList;
                m_states2D[5] = temp;

                m_states2D[5].Initialize(m_rigb, m_data, m_inputs);

                m_states3D[5] = m_states2D[5];

                m_saveData.AddUpgrade(type);
            }
        }
    }
}
