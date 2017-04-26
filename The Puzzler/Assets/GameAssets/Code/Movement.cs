using System.Collections;
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
        bool stuckUnderPlatform = false;

        if (m_coll.m_PrevColidedVertical && m_verticalSpeed > 0.0f)
        //if (m_coll.m_colidedVertical && m_verticalSpeed > 0.0f)
        {
            m_verticalSpeed = -0.001f;
            stuckUnderPlatform = true;
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
            //if (m_coll.m_PrevColidedVertical && !stuckUnderPlatform/*m_verticalSpeed == -0.005f!m_coll.GetCollData().m_collisionTop && */)
            //if (m_coll.m_colidedVertical && !m_coll.m_collisionTop/*m_verticalSpeed == -0.005f!m_coll.GetCollData().m_collisionTop && */)
            if (m_coll.m_PrevColidedVertical && !m_coll.m_collisionTop/*m_verticalSpeed == -0.005f!m_coll.GetCollData().m_collisionTop && */)
            {
                m_verticalSpeed = m_jumpForce;
            }
        }

        if (!m_coll.m_PrevColidedVertical)
        //if (!m_coll.m_colidedVertical)
        {
            m_verticalSpeed -= m_gravity * Time.deltaTime;
        }
        else if (m_verticalSpeed < 0.0f)
        {
            //m_verticalSpeed = -0.005f;
            //m_coll.MoveTo(m_coll.m_newPosX, m_coll.m_colisionPosY);
        }

        m_coll.Move(0.0f, m_verticalSpeed);
    }
}

