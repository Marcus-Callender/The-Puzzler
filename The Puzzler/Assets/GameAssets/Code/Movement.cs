﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour

{
    Renderer m_material;
    // how fast the charater moves left to right
    float m_speed = 10.0f;
    
    float m_verticalSpeed = 0.0f;
    float m_jumpForce = .50f;
    float m_gravity = 2.5f;

    CollisionBox m_coll;

    void Start()
    {
        m_coll = GetComponent<CollisionBox>();

        m_material = gameObject.GetComponent<Renderer>();
        m_material.material.color = Color.green;
    }

    void Update()
    {
        if (!m_coll.m_colidedVertical /*&& !m_coll.GetCollData().m_collisionTop*/)
        {
            m_verticalSpeed -= m_gravity * Time.deltaTime;
        }
        else if (m_verticalSpeed < 0.0f)
        {
            m_verticalSpeed = -0.005f;
            m_coll.MoveTo(m_coll.GetCollData().m_newPosX, m_coll.GetCollData().m_colisionPosY);
        }

        if (m_coll.m_colidedVertical && m_jumpForce > 0.0f)
        {
            m_verticalSpeed = 0.0f;
        }

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            m_coll.Move(m_speed * Time.deltaTime, 0.0f);
        }

        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            m_coll.Move(-m_speed * Time.deltaTime, 0.0f);

        }

        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            if (m_coll.m_colidedVertical && m_verticalSpeed < 0.0f)
            {
                m_verticalSpeed = m_jumpForce;
            }
        }

        m_coll.Move(0.0f, m_verticalSpeed);
    }
}

