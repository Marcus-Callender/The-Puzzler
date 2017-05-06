using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PLAYER_STATES
{
    ON_GROUND,
    IN_AIR,

    MOVEING_BLOCK,
    USEING_LADDER,
    SQUISHED,

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

public class PlayerStateMachine : MonoBehaviour
{
    BasicState[] m_states = new BasicState[2];
    PlayerData m_data;
    Rigidbody m_rigb;

    E_PLAYER_STATES m_currentState = E_PLAYER_STATES.IN_AIR;

    public bool grounded = true;
    bool DoubleJump = true;

    void Start()
    {
        m_states[0] = gameObject.AddComponent<OnGround>();
        m_states[1] = gameObject.AddComponent<InAIr>();
        m_data = GetComponent<PlayerData>();
        m_rigb = GetComponent<Rigidbody>();

        m_states[0].Initialize(m_rigb, m_data);
    }

    void Update()
    {
        m_states[(int)m_currentState].Cycle();
    }

    void FixedUpdate()
    {
        m_data.m_onLadder = false;

        for (int z = 0; z < 4; z++)
        {
            m_data.m_contacts[z] = false;
        }
    }

    void OnCollisionStay(Collision Other)
    {
        float angle = Vector2.Angle(Other.contacts[0].normal, Vector2.up);

        E_DIRECTIONS dir = E_DIRECTIONS.TOP;

        if (Mathf.Approximately(angle, 0.0f))
        {
            m_data.m_contacts[2] = true;
            dir = E_DIRECTIONS.BOTTOM;
        }
        else if (Mathf.Approximately(angle, 180.0f))
        {
            m_data.m_contacts[0] = true;
            dir = E_DIRECTIONS.TOP;
        }
        else if (Mathf.Approximately(angle, 90.0f))
        {
            if (Other.transform.position.x > m_rigb.position.x)
            {
                m_data.m_contacts[1] = true;
                dir = E_DIRECTIONS.RIGHT;
            }
            else
            {
                m_data.m_contacts[3] = true;
                dir = E_DIRECTIONS.LEFT;
            }
        }

        m_states[(int)m_currentState].Colide(dir, Other.gameObject.tag);
    }

    /*void OnCollisionExit(Collision Other)
    {
        grounded = false;

        // this is needed for disabling the double jump as OnCollisionStay will set doubleJump to true 
        // after the player has jumped as they are still making contact with the ground
        if (!m_data.m_playerDoubleJump)
        {
            DoubleJump = false;
        }

        if (Mathf.Approximately(angle, 90f))
        {
            if (slideRight & Other.transform.position.x > m_rigb.position.x)
            {
                m_useWallGravity = false;
                slideRight = false;
            }
            else if (slideLeft & Other.transform.position.x < m_rigb.position.x)
            {
                m_useWallGravity = false;
                slideLeft = false;
            }
        }
    }*/

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ladder")
        {
            m_data.m_onLadder = true;
        }
    }
}
