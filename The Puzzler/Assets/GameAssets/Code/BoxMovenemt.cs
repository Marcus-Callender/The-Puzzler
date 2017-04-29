using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovenemt : MonoBehaviour
{
    Rigidbody m_rigb;
    PlayerData m_player;
    Rigidbody m_playerRigb;

    float m_playerInteractDistance = 1.2f;//0.8f;

    void Start()
    {
        m_rigb = gameObject.GetComponent<Rigidbody>();
        m_player = FindObjectOfType<PlayerData>();

        m_playerRigb = m_player.gameObject.GetComponent<Rigidbody>();

        m_playerInteractDistance += gameObject.transform.localScale.x * 0.5f;
    }

    void LateUpdate()
    {
        m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);
    }

    private void FixedUpdate()
    {
        m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);

        if (Mathf.Abs(gameObject.transform.position.x - m_player.gameObject.transform.position.x) <= m_playerInteractDistance)
        {
            m_player.m_closeToBox = true;

            if (m_player.m_moveingBox)
            {
                m_rigb.velocity = new Vector3(m_playerRigb.velocity.x, m_rigb.velocity.y);

                Debug.Log("Player Close");
            }
        }

    }
}
