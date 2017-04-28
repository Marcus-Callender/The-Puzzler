using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float jumpSpeed = 10;
    public float speed = 5;
    float direction;
    float angle;
    bool grounded = true;
    bool DoubleJump = true;
    bool slideLeft = false;
    bool slideRight = false;
    
    float m_verticalVelocity = 0.0f;
    float m_gravity = 9.81f;
    float m_wallGravity = 3.5f;
    bool m_useWallGravity = false;

    Rigidbody m_rigb;
    
    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        if (m_useWallGravity)
        {
            m_rigb.velocity = new Vector3(direction * speed, m_rigb.velocity.y - (m_wallGravity * Time.deltaTime));
        }
        else
        {
            m_rigb.velocity = new Vector3(direction * speed, m_rigb.velocity.y - (m_gravity * Time.deltaTime));
        }

        if (Input.GetButtonDown("Jump") && DoubleJump)
        {
            m_rigb.velocity = new Vector3(m_rigb.velocity.x, jumpSpeed);

            if (!grounded)
            {
                DoubleJump = false;
            }
        }
        else if (Input.GetButtonUp("Jump") & m_rigb.velocity.y > 0f)
        {
            m_rigb.velocity = new Vector3(m_rigb.velocity.x, 0f);
        }

        if (slideRight & direction <= 0)
        {
            m_useWallGravity = false;
            slideRight = false;
        }
        else if (slideLeft & direction >= 0)
        {
            m_useWallGravity = false;
            slideLeft = false;
        }

    }

    void OnCollisionEnter(Collision Other)
    {
        grounded = true;
    }

    void OnCollisionStay(Collision Other)
    {
        grounded = true;
        DoubleJump = true;

        angle = Vector2.Angle(Other.contacts[0].normal, Vector2.up);

        if (Mathf.Approximately(angle, 90f))
        {
            if (Other.transform.position.x > m_rigb.position.x)
            {
                if (direction > 0)
                {
                    m_useWallGravity = true;
                    slideRight = true;
                }
            }
            else
            {
                if (direction < 0)
                {
                    m_useWallGravity = true;
                    slideLeft = true;
                }
            }
        }
    }

    void OnCollisionExit(Collision Other)
    {
        grounded = false;

        if (Mathf.Approximately(angle, 90f))
        {
            if (slideRight & Other.transform.position.x > m_rigb.position.x)
            {
                m_useWallGravity = false;
                slideRight = false;
            }
            else if (slideLeft & Other.transform.position.x < m_rigb.position.x)
            {
                m_useWallGravity = false;
                slideLeft = false;
            }
        }
    }
}

