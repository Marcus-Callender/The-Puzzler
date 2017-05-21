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

    void Start()
    {
        m_inputs = gameObject.AddComponent<PlayerInputs>();

        m_states[0] = gameObject.AddComponent<OnGround>();
        m_states[1] = gameObject.AddComponent<InAIr>();
        m_states[2] = gameObject.AddComponent<MoveingBox>();
        m_states[3] = gameObject.AddComponent<ClimbingLadder>();
        m_states[5] = gameObject.AddComponent<ControlingGhost>();

        m_data = GetComponent<PlayerData>();
        m_rigb = GetComponent<Rigidbody>();

        m_states[0].Initialize(m_rigb, m_data, m_inputs);
        m_states[1].Initialize(m_rigb, m_data, m_inputs);
        m_states[2].Initialize(m_rigb, m_data, m_inputs);
        m_states[3].Initialize(m_rigb, m_data, m_inputs);
        m_states[5].Initialize(m_rigb, m_data, m_inputs);
    }

    public override void Update()
    {
        m_inputs.Cycle();
        
        m_data.m_pressingButton = m_inputs.GetInput(E_INPUTS.PRESS_BUTTON);

        base.Update();

        if (m_inputs.GetInput(E_INPUTS.START_GHOST))
        {
            m_newState = E_PLAYER_STATES.CONTROLING_GHOST;
            CheckState();
        }
    }
}
