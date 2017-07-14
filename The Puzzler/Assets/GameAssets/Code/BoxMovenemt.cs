using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovenemt : MonoBehaviour
{
    Rigidbody m_rigb;
    PlayerData[] m_playerRefs;

    float m_playerInteractDistance = 0.5f;
    float m_playerBoxMinDistance = 0.0f;

    public bool m_requestStop = false;
    private GameObject m_linkedBox;

    void Start()
    {
        m_rigb = gameObject.GetComponent<Rigidbody>();
        m_playerRefs = FindObjectsOfType<PlayerData>();

        m_playerBoxMinDistance = (gameObject.transform.localScale.x + m_playerRefs[0].gameObject.transform.localScale.x) * 0.5f;
    }

    private void Update()
    {
        m_rigb.velocity = new Vector3(m_rigb.velocity.x, m_rigb.velocity.y - 9.81f, m_rigb.velocity.z);
    }

    public void Move(float xVelocity)
    {
        m_rigb.velocity = new Vector3(xVelocity, m_rigb.velocity.y);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Box")
        {
            m_linkedBox = other.gameObject;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log("Box Colision Exit");

        m_rigb.velocity.Set(0.0f, 0.0f, 0.0f);
    }
}
