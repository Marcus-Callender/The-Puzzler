using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum E_ActionState
{
    NEXT_TO_PLAYER,
    FOLOWING_PLAYER,
    LOOKING_FOR_PLAYER,
    PATROLLING,
    ATTACKING,

    SIZE,
    NULL
}

public class Enemy : MonoBehaviour
{
    private PlayerData[] m_players;
    private Rigidbody m_rigb;
    private bool m_faceingLeft;
    private bool m_grounded = false;
    
    // y = -11 is eqivelent to null as y = -10 is the kill floor
    private Vector3 m_PlayerLastPosition;
    private Renderer m_renderer;

    public bool m_KOd = false;
    
    private E_ActionState m_actionState;

    private bool m_patrollingLeft = false;
    private Vector3 m_patrollingCenter;

    private float m_attackDelay = 0.6f;
    private float m_attackActive = 0.6f;
    private float m_attackRecovery = 0.6f;
    private Timer m_attackDelayTimer;
    private Timer m_attackActiveTimer;
    private Timer m_attackRecoveryTimer;

    private PlayerData m_targate = null;

    private GameObject m_attack;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();
        m_players = FindObjectsOfType<PlayerData>();
        m_renderer = GetComponent<Renderer>();

        Transform[] refrences = gameObject.GetComponentsInChildren<Transform>();

        for (int z = 0; z < refrences.Length; z++)
        {
            if (refrences[z].tag == "Attack")
            {
                m_attack = refrences[z].gameObject;
            }
        }

        if (m_attack == null)
        {
            Debug.Log("-----Error attack refrence not found!------");
        }

        m_attack.SetActive(false);
        
        m_attackDelayTimer = new Timer();
        m_attackActiveTimer = new Timer();
        m_attackRecoveryTimer = new Timer();

        m_attackDelayTimer.m_time = m_attackDelay;
        m_attackActiveTimer.m_time = m_attackActive;
        m_attackRecoveryTimer.m_time = m_attackRecovery;
        
        m_actionState = E_ActionState.PATROLLING;
        m_PlayerLastPosition = new Vector3(0.0f, -11.0f, 0.0f);
        m_patrollingCenter = gameObject.transform.position;
    }

    void Update()
    {
        if (m_grounded)
        {
            m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);
        }

        m_attackDelayTimer.Cycle();
        m_attackActiveTimer.Cycle();
        m_attackRecoveryTimer.Cycle();
        
        if (m_actionState != E_ActionState.ATTACKING)
        {
            CheckForPlayer();
        }

        Movement();
    }

    void CheckForPlayer()
    {
        E_ActionState m_previousState = m_actionState;

        m_actionState = E_ActionState.NULL;
        m_targate = null;
        m_renderer.material.color = Color.green;

        for (int z = 0; z < m_players.Length; z++)
        {
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

        if (m_actionState == E_ActionState.NULL && m_PlayerLastPosition.y != -11.0f)
        {
            m_actionState = E_ActionState.LOOKING_FOR_PLAYER;
            m_renderer.material.color = Color.yellow;
        }

        if (m_actionState == E_ActionState.NULL)
        {
            if (m_previousState != E_ActionState.PATROLLING)
            {
                m_patrollingCenter = gameObject.transform.position;
            }

            m_actionState = E_ActionState.PATROLLING;
        }

        if (m_previousState == E_ActionState.NEXT_TO_PLAYER && !(m_actionState == E_ActionState.NEXT_TO_PLAYER /*|| m_actionState == E_ActionState.ATTACKING*/))
        {
            m_attackDelayTimer.m_completed = false;
            m_attackDelayTimer.m_playing = false;
        }
    }

    void Movement()
    {
        if (m_actionState == E_ActionState.FOLOWING_PLAYER)
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
        }
        else if (m_actionState == E_ActionState.LOOKING_FOR_PLAYER)
        {
            if (Mathf.Abs(gameObject.transform.position.x - m_PlayerLastPosition.x) < 0.1f)
            {
                m_PlayerLastPosition.y = -11.0f;
            }

            if (gameObject.transform.position.x > m_PlayerLastPosition.x)
            {
                m_rigb.velocity = new Vector3(-3.0f, m_rigb.velocity.y);
                m_faceingLeft = true;
            }
            else
            {
                m_rigb.velocity = new Vector3(3.0f, m_rigb.velocity.y);
                m_faceingLeft = false;
            }
        }
        else if (m_actionState == E_ActionState.PATROLLING)
        {
            if (gameObject.transform.position.x > m_patrollingCenter.x + 1.0f)
            {
                m_patrollingLeft = true;
            }
            else if (gameObject.transform.position.x < m_patrollingCenter.x - 1.0f)
            {
                m_patrollingLeft = false;
            }

            m_rigb.velocity = new Vector3(m_patrollingLeft ? -1.0f : 1.0f, m_rigb.velocity.y);
        }
        else if (m_actionState == E_ActionState.NEXT_TO_PLAYER)
        {
            if (!m_attackDelayTimer.m_playing)
            {
                m_attackDelayTimer.Play();
            }

            if (m_attackDelayTimer.m_completed)
            {
                m_actionState = E_ActionState.ATTACKING;
                m_attackDelayTimer.Stop();
                m_attackActiveTimer.Play();

                m_attack.SetActive(true);
            }
        }
        else if(m_actionState == E_ActionState.ATTACKING)
        {
            if (m_attackActiveTimer.m_completed)
            {
                m_attackActiveTimer.Stop();

                m_attack.SetActive(false);

                m_attackRecoveryTimer.Play();
            }

            if (m_attackRecoveryTimer.m_completed)
            {
                m_attackRecoveryTimer.Stop();

                m_actionState = E_ActionState.PATROLLING;
            }
        }
    }

    void FixedUpdate()
    {
        if (gameObject.transform.position.y < -40.0f)
        {
            Destroy(gameObject);
        }
        else if (m_KOd)
        {
            m_rigb.velocity = new Vector3(0.0f, -7.0f, -7.0f);
        }
        else if ((m_actionState == E_ActionState.FOLOWING_PLAYER || m_actionState == E_ActionState.LOOKING_FOR_PLAYER || m_actionState == E_ActionState.PATROLLING) && m_grounded)
        {
            Debug.DrawRay(transform.position, new Vector3(m_faceingLeft ? -1.0f : 1.0f, 0.0f), Color.red);

            if (Physics.Raycast(transform.position, new Vector3(m_faceingLeft ? -1.0f : 1.0f, 0.0f), 0.7f))
            {
                Debug.Log("Jump up platform");
                m_rigb.velocity = new Vector3(m_rigb.velocity.x, 6.5f);
            }

            Debug.DrawRay(transform.position, new Vector3(m_faceingLeft ? -1.0f : 1.0f, -1.0f), Color.yellow);

            if (!Physics.Raycast(transform.position, new Vector3(m_faceingLeft ? -1.0f : 1.0f, -1.0f)))
            {
                Debug.DrawRay(transform.position + new Vector3(m_faceingLeft ? -1.0f : 1.0f, -1.0f), new Vector3(m_faceingLeft ? -2.0f : 2.0f, 0.0f), Color.yellow);

                if (Physics.Raycast(transform.position + new Vector3(m_faceingLeft ? -1.0f : 1.0f, -1.0f), new Vector3(m_faceingLeft ? -2.0f : 2.0f, 0.0f)))
                {
                    Debug.Log("Jump over gap");
                    m_rigb.velocity = new Vector3(m_rigb.velocity.x, 6.5f);
                }
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
