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
    CharicterData m_me;
    CHARICTER_STATES m_currentState;
    CHARICTER_STATES m_newState;
    BaseCharicterState[] m_states = new BaseCharicterState[(int)CHARICTER_STATES.NUMBER_OF_STATES];
    string[] m_collisions = new string[(int)DIRECTIONS.NUMBER_OF_DIRECTIONS];

    void Start()
    {
        m_me = GetComponent<CharicterData>();

        m_currentState = CHARICTER_STATES.STAND;
        m_newState = CHARICTER_STATES.STAND;

        m_states[(int)CHARICTER_STATES.STAND] = gameObject.AddComponent<Standing>();
        m_states[(int)CHARICTER_STATES.WALK] = gameObject.AddComponent<Walking>();
        m_states[(int)CHARICTER_STATES.JUMP] = gameObject.AddComponent<Jumping>();

        for (int z = 0; z < (int)CHARICTER_STATES.NUMBER_OF_STATES; z++)
        {
            m_states[z].Initialize(m_me);
        }
    }

    void Update()
    {
        CheckCollision();

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

        m_newState = m_states[(int)m_currentState].GetInput();
        CheckState();

        m_newState = m_states[(int)m_currentState].Cycle();
        CheckState();
    }

    void CheckCollision()
    {
        for (int z = 0; z < (int)DIRECTIONS.NUMBER_OF_DIRECTIONS; z++)
        {
            m_collisions[z] = "";
        }
        
        RaycastHit hit;

        var up = transform.TransformDirection(Vector3.up);
        var left = transform.TransformDirection(Vector3.left);

        Debug.DrawRay(transform.position, -up * 0.5f, Color.green);
        Debug.DrawRay(transform.position, up * 0.5f, Color.green);
        Debug.DrawRay(transform.position, left * 0.5f, Color.green);
        Debug.DrawRay(transform.position, -left * 0.5f, Color.green);

        if (Physics.Raycast(transform.position, -up, out hit, 0.5f))
        {
            m_collisions[(int)DIRECTIONS.DOWN] = hit.collider.gameObject.name;
            Debug.Log("Hit Down");
        }

        if (Physics.Raycast(transform.position, up, out hit, 0.5f))
        {
            m_collisions[(int)DIRECTIONS.UP] = hit.collider.gameObject.name;
            Debug.Log("Hit Up");
        }

        if (Physics.Raycast(transform.position, left, out hit, 0.5f))
        {
            m_collisions[(int)DIRECTIONS.LEFT] = hit.collider.gameObject.name;
            Debug.Log("Hit Left");
        }

        if (Physics.Raycast(transform.position, -left, out hit, 0.5f))
        {
            m_collisions[(int)DIRECTIONS.RIGHT] = hit.collider.gameObject.name;
            Debug.Log("Hit Right");
        }

    }

    private void CheckState()
    {
        if (m_newState != m_currentState)
        {
            Debug.Log(m_currentState + " -> " + m_newState);
            m_states[(int)m_currentState].Exit();
            m_states[(int)m_newState].Enter();
            m_currentState = m_newState;
        }
    }
}

