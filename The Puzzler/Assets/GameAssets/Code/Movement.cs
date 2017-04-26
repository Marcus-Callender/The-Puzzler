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
    
    Rigidbody m_rigb;
    BoxCollider m_colider;

    void Start()
    {
        //m_coll = GetComponent<CollisionBox>();
        m_rigb = GetComponent<Rigidbody>();
        m_colider = GetComponent<BoxCollider>();

        m_material = gameObject.GetComponent<Renderer>();
        m_material.material.color = Color.green;
    }

    void Update()
    {
        Vector3 vel = new Vector3(0.0f, 0.0f, 0.0f);
        bool onGround = Physics.Raycast(transform.position, -transform.up, 0.2f);
        Debug.Log(onGround);

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            vel.x += m_speed * Time.deltaTime;
            
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {

            vel.x -= m_speed * Time.deltaTime;

        }

        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            if (Physics.Raycast(transform.position, -transform.up, 0.2f))
            {
                m_verticalSpeed = m_jumpForce;
            }
            
        }

        if (!Physics.Raycast(transform.position, -transform.up, 0.2f))
        {
            m_verticalSpeed -= m_gravity * Time.deltaTime;
        }

        vel.y += m_verticalSpeed;
        m_rigb.velocity = vel;
    }
}

