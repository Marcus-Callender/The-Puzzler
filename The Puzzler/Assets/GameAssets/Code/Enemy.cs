using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerData[] m_players;
    private Rigidbody m_rigb;
    private bool m_faceingLeft;
    private bool m_grounded = false;
    private bool m_folowingPlayer = false;

    // y = -11 is eqivelent to null as y = -10 is the kill floor
    private Vector3 m_PlayerLastPosition;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();
        m_players = FindObjectsOfType<PlayerData>();

        m_PlayerLastPosition = new Vector3(0.0f, -11.0f, 0.0f);
    }

    void Update()
    {
        m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);
        m_folowingPlayer = false;

        Debug.Log("Searching for player");

        for (int z = 0; z < m_players.Length; z++)
        {
            //float distance = Mathf.Abs(gameObject.transform.position.x - m_players[z].GetCenterTransform().x) + Mathf.Abs(gameObject.transform.position.y - m_players[z].transform.position.y);
            float distance = Mathf.Abs(gameObject.transform.position.x - m_players[z].GetCenterTransform().x);

            if (m_players[z].tag == "Player" && distance < 5.0f && distance > 1.5f)
            {
                Vector3 fwd = transform.TransformDirection(Vector3.forward);

                RaycastHit hitData;

                if (!Physics.Raycast(transform.position, m_players[z].GetCenterTransform() - transform.position, out hitData))
                {
                    Debug.DrawRay(transform.position, m_players[z].GetCenterTransform() - transform.position, Color.red);
                    Debug.Log("Player hidden");
                }
                else
                {
                    if (hitData.transform == m_players[z].transform)
                    {
                        Debug.DrawRay(transform.position, m_players[z].GetCenterTransform() - transform.position, Color.green);

                        Debug.Log("Player found");

                        m_folowingPlayer = true;

                        if (gameObject.transform.position.x > m_players[z].GetCenterTransform().x)
                        {
                            m_rigb.velocity = new Vector3(-3.0f, m_rigb.velocity.y);
                            m_faceingLeft = true;
                        }
                        else
                        {
                            m_rigb.velocity = new Vector3(3.0f, m_rigb.velocity.y);
                            m_faceingLeft = false;
                        }

                        m_PlayerLastPosition = m_players[z].GetCenterTransform();
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, m_players[z].GetCenterTransform() - transform.position, Color.red);
                        Debug.Log("Error");
                    }
                }
            }
        }

        if (!m_folowingPlayer)
        {
            if (m_PlayerLastPosition.y != -11.0f)
            {
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

                if (Mathf.Abs(m_PlayerLastPosition.x - gameObject.transform.position.x) < 0.2f)
                {
                    m_PlayerLastPosition.y = -11.0f;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (m_folowingPlayer && m_grounded)
        {
            Debug.DrawRay(transform.position, new Vector3(m_faceingLeft ? -1.0f : 1.0f, 0.0f), Color.red);

            if (Physics.Raycast(transform.position, new Vector3(m_faceingLeft ? -1.0f : 1.0f, 0.0f), 0.7f))
            {
                m_rigb.velocity = new Vector3(m_rigb.velocity.x, 4.0f);
            }
        }
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
