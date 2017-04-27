using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTIONS
{
    UP,
    DOWN,
    LEFT,
    RIGHT,

    NUMBER_OF_DIRECTIONS,
    NULL
}

public class CharicterStateMachine : MonoBehaviour
{
    CHARICTER_STATES m_currentState;
    CHARICTER_STATES m_newState;
    BaseCharicterState[] m_states = new BaseCharicterState[(int) CHARICTER_STATES.NUMBER_OF_STATES];
    string[] m_collisions = new string[(int)DIRECTIONS.NUMBER_OF_DIRECTIONS];

    void Start()
    {
        m_currentState = CHARICTER_STATES.STAND;
        m_newState = CHARICTER_STATES.STAND;
    }

    void Update()
    {
        for (int z = 0; z < (int)DIRECTIONS.NUMBER_OF_DIRECTIONS; z++)
        {
            if (m_collisions[z] == "")
            {
                m_newState = m_states[(int)m_currentState].NotCollided((DIRECTIONS)z);
                CheckState();
            }
            else
            {
                m_newState = m_states[(int)m_currentState].Collision((DIRECTIONS)z, m_collisions[z]);
                CheckState();
            }
        }

        m_states[(int)m_currentState].Input();
        CheckState();

        m_states[(int)m_currentState].Cycle();
        CheckState();
    }

    private void FixedUpdate()
    {
        for (int z = 0; z < (int)DIRECTIONS.NUMBER_OF_DIRECTIONS; z++)
        {
            m_collisions[z] = "";
        }
    }

    private void OnCollision(Collision collision)
    {
        DIRECTIONS collisionDir = DIRECTIONS.NULL;

        if (Mathf.Abs(collision.contacts[0].point.x) > Mathf.Abs(collision.contacts[0].point.y))
        {
            if (collision.contacts[0].point.x > 0.0f)
            {
                collisionDir = DIRECTIONS.RIGHT;
            }
            else
            {
                collisionDir = DIRECTIONS.LEFT;
            }
        }
        else
        {
            if (collision.contacts[0].point.y > 0.0f)
            {
                collisionDir = DIRECTIONS.UP;
            }
            else
            {
                collisionDir = DIRECTIONS.DOWN;
            }
        }

        m_collisions[(int)collisionDir] = collision.gameObject.tag;
    }

    private void CheckState()
    {
        if (m_newState != m_currentState)
        {
            m_states[(int)m_currentState].Exit();
            m_states[(int)m_newState].Enter();
            m_currentState = m_newState;
        }
    }
}

