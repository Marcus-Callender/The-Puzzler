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
    SQUISHED,

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

    public override void Start()
    {
        base.Start();

        m_inputs = gameObject.AddComponent<PlayerInputs>();

        m_states[0] = gameObject.AddComponent<OnGround>();
        m_states[1] = gameObject.AddComponent<InAIr>();
        m_states[2] = gameObject.AddComponent<MoveingBox>();
        m_states[3] = gameObject.AddComponent<ClimbingLadder>();
        m_states[5] = gameObject.AddComponent<ControlingGhost>();

        m_states[0].Initialize(m_rigb, m_data, m_inputs);
        m_states[1].Initialize(m_rigb, m_data, m_inputs);
        m_states[2].Initialize(m_rigb, m_data, m_inputs);
        m_states[3].Initialize(m_rigb, m_data, m_inputs);
        m_states[5].Initialize(m_rigb, m_data, m_inputs);
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

        m_data.m_moveingBox = false;

        if (m_inputs.GetInput(E_INPUTS.MOVE_BOX))
        {
            m_data.m_moveingBox = true;
        }

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

    public Vector3 getFollowPos()
    {
        if (m_data.m_overideFollow)
        {
            return m_data.m_overideFollow.transform.position;
        }

        return gameObject.transform.position;
    }

    public override void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            m_data.m_squished = true;
        }

        base.OnTriggerStay(other);
    }
}
