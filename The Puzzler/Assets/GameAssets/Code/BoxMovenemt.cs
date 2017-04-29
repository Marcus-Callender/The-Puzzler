using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovenemt : MonoBehaviour
{
    Rigidbody m_rigb;
    BasicMovement m_player;

    float m_playerInteractDistance = 0.8f;

    void Start()
    {
        m_rigb = gameObject.GetComponent<Rigidbody>();
        m_player = FindObjectOfType<BasicMovement>();

        m_playerInteractDistance += gameObject.transform.localScale.x * 0.5f;
    }

    void LateUpdate()
    {
        if (Mathf.Abs(gameObject.transform.position.x - m_player.gameObject.transform.position.x) <= m_playerInteractDistance)
        {
            if (m_player.grounded)
            {
                Debug.Log("Player Close");
            }
        }

        m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);
    }

    private void FixedUpdate()
    {
        m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);

    }
}
