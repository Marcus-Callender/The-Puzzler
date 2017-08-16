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

    void Start()
    {

    }

    void Update()
    {

    }
}
