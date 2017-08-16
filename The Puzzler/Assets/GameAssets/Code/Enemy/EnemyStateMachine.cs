using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ENEMY_STATES
{
    GROUND,
    AIR,
    ATTACK,

    LENGTH,
    NULL
}

public enum E_ENEMY_INPUTS
{
    LEFT,
    RIGHT,
    JUMP,
    ATTACK
}

public class EnemyStateMachine : MonoBehaviour
{
    public E_ENEMY_STATES m_currentState;
    public E_ENEMY_STATES m_nextState;

    public EnemyBaseState[] m_states = new EnemyBaseState[(int)E_ENEMY_STATES.LENGTH];

    private EnemyData m_data;
    private Rigidbody m_rigb;

    void Start()
    {
        m_data = gameObject.AddComponent<EnemyData>();
        m_rigb = gameObject.GetComponent<Rigidbody>();

        m_currentState = E_ENEMY_STATES.AIR;
        m_nextState = E_ENEMY_STATES.NULL;

        m_states[0] = gameObject.AddComponent<EnemyGround>();
        m_states[1] = gameObject.AddComponent<EnemyAir>();
        m_states[2] = gameObject.AddComponent<EnemyAttack>();

        for (int z = 0; z < (int)E_ENEMY_STATES.LENGTH; z++)
        {
            m_states[z].Initialize(m_data, m_rigb);
        }
    }

    void Update()
    {
        
    }
}
