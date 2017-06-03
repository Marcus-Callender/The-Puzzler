using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum E_ActionState
{
    NEXT_TO_PLAYER,
    FOLOWING_PLAYER,
    LOOKING_FOR_PLAYER,
    PATROLLING,

    SIZE,
    NULL
}

public class Enemy : MonoBehaviour
{
    private PlayerData[] m_players;
    private Rigidbody m_rigb;
    private bool m_faceingLeft;
    private bool m_grounded = false;
    private bool m_folowingPlayer = false;

    private bool m_playerClose = false;

    // y = -11 is eqivelent to null as y = -10 is the kill floor
    private Vector3 m_PlayerLastPosition;
    private Renderer m_renderer;

    public bool m_KOd = false;

    private float m_xDir = 0.0f;
    private float m_yDir = 0.0f;
    private E_ActionState m_actionState;

    private PlayerData m_targate = null;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();
        m_players = FindObjectsOfType<PlayerData>();
        m_renderer = GetComponent<Renderer>();

        m_actionState = E_ActionState.PATROLLING;
        m_PlayerLastPosition = new Vector3(0.0f, -11.0f, 0.0f);
    }

    void Update()
    {
        m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);
        m_folowingPlayer = false;
        m_playerClose = false;

        CheckForPlayer();
        Movement();
    }

    void CheckForPlayer()
    {
        m_actionState = E_ActionState.PATROLLING;
        m_targate = null;
        m_renderer.material.color = Color.green;

        for (int z = 0; z < m_players.Length; z++)
        {
            //float distance = Mathf.Abs(gameObject.transform.position.x - m_players[z].GetCenterTransform().x) + Mathf.Abs(gameObject.transform.position.y - m_players[z].transform.position.y);
            float distance = Mathf.Abs(gameObject.transform.position.x - m_players[z].GetCenterTransform().x);

            if (m_players[z].tag == "Player" && distance <= 1.5f)
            {
                m_targate = m_players[z];
                m_actionState = E_ActionState.NEXT_TO_PLAYER;
                m_renderer.material.color = Color.magenta;
                break;
            }
            else if (m_players[z].tag == "Player" && distance < 5.0f && distance > 1.5f)
            {

                RaycastHit hitData;

                Physics.Raycast(transform.position, m_players[z].GetCenterTransform() - transform.position, out hitData);
                
                if (hitData.transform == m_players[z].transform)
                {
                    Debug.DrawRay(transform.position, m_players[z].GetCenterTransform() - transform.position, Color.green);

                    m_targate = m_players[z];
                    m_PlayerLastPosition = m_players[z].GetCenterTransform();
                    m_actionState = E_ActionState.FOLOWING_PLAYER;
                    m_renderer.material.color = Color.red;
                }
                else
                {
                    Debug.DrawRay(transform.position, m_players[z].GetCenterTransform() - transform.position, Color.red);
                }
            }
        }

        if (m_actionState == E_ActionState.PATROLLING && m_PlayerLastPosition.y != 11.0f)
        {
            m_actionState = E_ActionState.LOOKING_FOR_PLAYER;
            m_renderer.material.color = Color.yellow;
        }
    }

    void Movement()
    {
        if (m_actionState == E_ActionState.FOLOWING_PLAYER || m_actionState == E_ActionState.LOOKING_FOR_PLAYER)
        {
            if (gameObject.transform.position.x > m_targate.GetCenterTransform().x)
            {
                m_rigb.velocity = new Vector3(-3.0f, m_rigb.velocity.y);
                m_faceingLeft = true;
            }
            else
            {
                m_rigb.velocity = new Vector3(3.0f, m_rigb.velocity.y);
                m_faceingLeft = false;
            }

            if ((gameObject.transform.position.y + 1.0f) < m_targate.GetCenterTransform().y && m_grounded)
            {
                m_rigb.velocity = new Vector3(m_rigb.velocity.x, 4.5f);
            }
        }
    }

    void FixedUpdate()
    {
        if (gameObject.transform.position.y < -10.0f)
        {
            Destroy(gameObject);
        }
        else if (m_KOd)
        {
            m_rigb.velocity = new Vector3(0.0f, -7.0f, -7.0f);
        }
        else if ((m_actionState == E_ActionState.FOLOWING_PLAYER || m_actionState == E_ActionState.LOOKING_FOR_PLAYER) && m_grounded)
        {
            Debug.DrawRay(transform.position, new Vector3(m_faceingLeft ? -1.0f : 1.0f, 0.0f), Color.red);

            if (Physics.Raycast(transform.position, new Vector3(m_faceingLeft ? -1.0f : 1.0f, 0.0f), 0.7f))
            {
                m_rigb.velocity = new Vector3(m_rigb.velocity.x, 4.5f);
            }
        }

        m_grounded = false;
    }

    private void OnCollisionStay(Collision Other)
    {
        float angle = Vector2.Angle(Other.contacts[0].normal, Vector2.up);

        if (Mathf.Approximately(angle, 0.0f))
        {
            m_grounded = true;
        }
    }
}
