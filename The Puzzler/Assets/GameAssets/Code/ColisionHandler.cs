using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionHandler : MonoBehaviour
{
    float m_verticalSpeed = 0.0f;
    float m_gravity = 2.5f;
    CollisionData m_data;
    
    void Start()
    {

    }
    
    void Update()
    {

    }

    void VerticalCollision()
    {
        if (!m_data.m_colidedVertical)
        {
            m_verticalSpeed -= m_gravity * Time.deltaTime;
        }
        else if (m_verticalSpeed < 0.0f)
        {
            m_verticalSpeed = -0.005f;
            //m_coll.MoveTo(m_coll.GetCollData().m_newPosX, m_coll.GetCollData().m_colisionPosY);
        }
    }

    void HorizontalCollision()
    {

    }
}

