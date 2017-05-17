using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public float m_velocityX = 0.0f;
    public float m_velocityY = 0.0f;
    
    public bool m_moveingBox = false;
    public bool m_closeToBox = false;

    public bool m_pressingButton = false;

    public bool m_playerDoubleJump = false;
    public bool m_playerWallSlide = false;

    public bool m_squished = false;
    public bool m_onLadder = false;

    // 0 = top, 1 = right, 2 = bottom, 3 = left
    public bool[] m_contacts = new bool[4];
    public bool[] m_InteractableContacts = new bool[4];

    // left = false, right = true
    public bool m_left_right = true;

    private Rigidbody m_rigb;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();

        PersistantData data = PersistantData.m_instance;

        if (data)
        {
            m_playerDoubleJump = data.m_playerDoubleJump;
            m_playerWallSlide = data.m_playerWallSlide;
        }
    }
    
    void Update()
    {
        if (m_left_right && m_velocityX > 0.0f)
        {
            Vector3 newScale = gameObject.transform.localScale;

            newScale.z = 1.0f;

            gameObject.transform.localScale = newScale;

            m_left_right = false;
        }
        else if (!m_left_right && m_velocityX < 0.0f)
        {
            Vector3 newScale = gameObject.transform.localScale;

            newScale.z = -1.0f;

            gameObject.transform.localScale = newScale;

            m_left_right = true;
        }
    }

    void FixedUpdate()
    {
        if (!m_squished)
        {
            m_rigb.velocity = new Vector3(m_velocityX, m_velocityY);
        }
        else
        {
            m_rigb.velocity = new Vector3(0.0f, -7.0f, -7.0f);
        }
    }
}
