﻿using System.Collections;
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
    public Animator m_anim;

    public GameObject m_ghost;
    public BoxMovenemt m_linkedBox;

    public GameObject m_overideFollow;

    public bool m_use3D = false;
    public Quaternion m_rotation;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();

        PersistantData data = PersistantData.m_instance;

        m_anim = GetComponent<Animator>();

        m_rotation = gameObject.transform.rotation;

        if (data)
        {
            m_playerDoubleJump = data.m_playerDoubleJump;
            m_playerWallSlide = data.m_playerWallSlide;
        }
    }

    void Update()
    {
        if (!m_use3D)
        {
            if (m_left_right && m_velocityX < 0.0f)
            {
                gameObject.transform.Rotate(new Vector3(0.0f, 180.0f));
                //m_rotation.x = 90.0f;
                m_rotation = gameObject.transform.rotation;

                m_left_right = false;
            }
            else if (!m_left_right && m_velocityX > 0.0f)
            {
                gameObject.transform.Rotate(new Vector3(0.0f, 180.0f));
                //m_rotation.x = -90.0f;
                m_rotation = gameObject.transform.rotation;

                m_left_right = true;
            }
        }

        gameObject.transform.rotation = m_rotation;

        Debug.Log("Y: " + (gameObject.transform.rotation.y - m_rotation.y));
        Debug.Log("W: " + (gameObject.transform.rotation.w - m_rotation.w));
        Debug.Log("Rotation: " + m_rotation);
    }

    void FixedUpdate()
    {
        if (!m_use3D)
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
        else
        {
            m_rigb.velocity = (gameObject.transform.forward * m_velocityX) + (gameObject.transform.up * m_velocityY);
        }
    }

    public Vector3 GetCenterTransform()
    {
        Vector3 newTreansform = gameObject.transform.position;

        newTreansform.y += gameObject.transform.localScale.y/* * 0.5f*/;

        return newTreansform;
    }
}
