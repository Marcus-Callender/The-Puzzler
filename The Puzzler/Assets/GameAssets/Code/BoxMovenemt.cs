using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovenemt : MonoBehaviour
{
    Rigidbody m_rigb;
    PlayerData m_player;
    Rigidbody m_playerRigb;

    float m_playerInteractDistance = 0.5f;
    float m_playerBoxMinDistance = 0.0f;

    void Start()
    {
        m_rigb = gameObject.GetComponent<Rigidbody>();
        m_player = FindObjectOfType<PlayerData>();

        m_playerRigb = m_player.gameObject.GetComponent<Rigidbody>();

        m_playerBoxMinDistance = (gameObject.transform.localScale.x + m_player.gameObject.transform.localScale.x) * 0.5f;
    }

    void LateUpdate()
    {
        m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);

        float distance = Mathf.Abs(gameObject.transform.position.x - m_player.gameObject.transform.position.x);

        if (distance > m_playerBoxMinDistance && distance < m_playerBoxMinDistance + m_playerInteractDistance)
        {
            m_player.m_closeToBox = true;

            Debug.Log("Player Close");

            if (m_player.m_moveingBox)
            {
                m_rigb.velocity = new Vector3(m_playerRigb.velocity.x, m_rigb.velocity.y);
            }
        }
    }
}
