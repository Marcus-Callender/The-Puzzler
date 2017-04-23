using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // how fast the charater moves left to right
    float m_speed = 10.0f;
    //float m_jump = 5.0f;
    bool m_jumping = false;
    float m_verticalSpeed = 0.0f;
    float m_jumpForce = .50f;
    float m_gravity = 2.5f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            // gets a refrence to the position
            Vector3 pos = gameObject.transform.position;
            // adds to the position
            pos.x += m_speed * Time.deltaTime;
            // applies the new position
            gameObject.transform.position = pos;
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            Vector3 pos = gameObject.transform.position;
            pos.x -= m_speed * Time.deltaTime;
            gameObject.transform.position = pos;
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            if (!m_jumping)
            {
                m_jumping = true;
                m_verticalSpeed = m_jumpForce;
            }
        }

        if (m_jumping)
        {
            Vector3 pos = gameObject.transform.position;
            m_verticalSpeed -= m_gravity * Time.deltaTime;
            pos.y += m_verticalSpeed;
            gameObject.transform.position = pos;
        }
    }
}