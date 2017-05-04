using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class BasicMovement : MonoBehaviour
{
    PlayerData m_data;

    float jumpSpeed = 9.5f;
    float speed = 6.5f;
    float direction;
    float angle;
    public bool grounded = true;
    bool DoubleJump = true;
    bool slideLeft = false;
    bool slideRight = false;

    float m_gravity = 23.0f;
    float m_wallGravity = 3.5f;
    public bool m_useWallGravity = false;
    float m_boxMovingSpeed = 1.5f;

    Rigidbody m_rigb;

    // 0 = top, 1 = right, 2 = bottom, 3 = left
    private bool[] m_contacts = new bool[4];

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();
        m_data = GetComponent<PlayerData>();
    }

    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        if (m_data.m_moveingBox)
        {
            m_data.m_velocityX = direction * m_boxMovingSpeed;
        }
        else
        {
            m_data.m_velocityX = direction * speed;
        }

        if (m_useWallGravity)
        {
            m_data.m_velocityY -= (m_wallGravity * Time.deltaTime);
        }
        else
        {
            m_data.m_velocityY -= (m_gravity * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && DoubleJump && !m_data.m_moveingBox)
        {
            m_data.m_velocityY = jumpSpeed;

            if (!m_data.m_playerDoubleJump)
            {
                DoubleJump = false;
            }

            if (!grounded)
            {
                DoubleJump = false;
            }
        }
        else if (Input.GetButtonUp("Jump") & m_rigb.velocity.y > 0f)
        {
            m_data.m_velocityY = 0.0f;
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

        m_data.m_moveingBox = false;

        if (grounded && Input.GetButton("MoveBox") && m_data.m_closeToBox)
        {
            m_data.m_moveingBox = true;
        }

        m_data.m_pressingButton = false;

        if (Input.GetButtonDown("PressButton"))
        {
            m_data.m_pressingButton = true;
        }

        if (transform.position.y < -10.0f)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        m_data.m_closeToBox = false;
    }

    void FixedUpdate()
    {
        for (int z = 0; z < 4; z++)
        {
            m_contacts[z] = false;
        }
    }

    void OnCollisionStay(Collision Other)
    {
        angle = Vector2.Angle(Other.contacts[0].normal, Vector2.up);

        if (Mathf.Approximately(angle, 0.0f))
        {
            grounded = true;
            DoubleJump = true;

            m_contacts[2] = true;

            if (m_contacts[0])
            {
                Debug.Log("Squished!");
            }
        }
        else if (Mathf.Approximately(angle, 180.0f))
        {
            m_contacts[0] = true;

            if (m_contacts[2])
            {
                Debug.Log("Squished!");
            }
        }
        else if (Mathf.Approximately(angle, 90.0f))
        {
            if (Other.transform.position.x > m_rigb.position.x)
            {
                m_contacts[1] = true;

                if (m_contacts[3])
                {
                    Debug.Log("Squished!");
                }
            }
            else
            {
                m_contacts[3] = true;

                if (m_contacts[1])
                {
                    Debug.Log("Squished!");
                }
            }
        }

        if (m_data.m_playerWallSlide)
        {
            if (Mathf.Approximately(angle, 90f))
            {
                grounded = true;
                DoubleJump = true;

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
    }

    void OnCollisionExit(Collision Other)
    {
        grounded = false;

        // this is needed for disabling the double jump as OnCollisionStay will set doubleJump to true 
        // after the player has jumped as they are still making contact with the ground
        if (!m_data.m_playerDoubleJump)
        {
            DoubleJump = false;
        }

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

