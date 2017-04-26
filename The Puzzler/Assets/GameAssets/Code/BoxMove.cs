using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : MonoBehaviour
{
    Renderer m_material;

    float m_verticalSpeed = 0.0f;
    float m_gravity = 2.5f;

    CollisionBox m_coll;

    void Start()
    {
        m_coll = GetComponent<CollisionBox>();

        m_material = gameObject.GetComponent<Renderer>();
        m_material.material.color = Color.gray;
    }

    void Update()
    {
        if (m_coll.m_PrevColidedVertical || m_coll.m_colidedHorizontal)
        {
            Debug.Log("Box Collision");
        }

        //Debug.Log("Box speed: " + m_verticalSpeed);
        //Debug.Log("Box Y: " + m_coll.m_posY);
        //Debug.Log("Box height: " + m_coll.m_heght);

        if (m_coll.m_PrevColidedVertical && m_verticalSpeed > 0.0f)
        {
            m_verticalSpeed = -0.001f;
        }

        if (!m_coll.m_PrevColidedVertical)
        {
            m_verticalSpeed -= m_gravity * Time.deltaTime;
        }
        else if (m_verticalSpeed < 0.0f)
        {
            m_verticalSpeed = -0.005f;
            m_coll.MoveTo(m_coll.m_newPosX, m_coll.m_colisionPosY);
        }

        m_coll.Move(0.0f, m_verticalSpeed);
    }
}
