using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class BasicState : MonoBehaviour
{
    protected PlayerData m_data;
    protected Rigidbody m_rigb;

    float jumpSpeed = 9.5f;
    float speed = 6.5f;
    float direction;
    float angle;
    bool slideLeft = false;
    bool slideRight = false;

    float m_gravity = 23.0f;
    float m_wallGravity = 3.5f;
    public bool m_useWallGravity = false;
    float m_boxMovingSpeed = 1.5f;

    float m_ladderClimbSpeed = 4.0f;


    public void Initialize(Rigidbody rigb, PlayerData data)
    {
        m_rigb = rigb;
        m_data = data;
    }

    public virtual E_PLAYER_STATES Cycle()
    {
        ApplyGravity(m_gravity);

        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES NoGround()
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES Colide(E_DIRECTIONS _dir, string _tag)
    {
        return E_PLAYER_STATES.NULL;
    }

    protected void MoveHorzontal(float _speed)
    {
        direction = Input.GetAxisRaw("Horizontal");

        m_data.m_velocityX = direction * _speed;
    }

    protected void ApplyGravity(float _force)
    {
        m_data.m_velocityY -= (_force * Time.deltaTime);
    }

    void Update()
    {
        //direction = Input.GetAxisRaw("Horizontal");

        //if (m_data.m_moveingBox)
        //{
        //    MoveHorzontal(m_boxMovingSpeed);
        //}
        //else
        //{
        //    MoveHorzontal(speed);
        //}

        if (m_useWallGravity)
        {
            m_data.m_velocityY -= (m_wallGravity * Time.deltaTime);
        }
        if (m_data.m_onLadder)
        {
            m_data.m_velocityY = m_ladderClimbSpeed * Input.GetAxisRaw("Vertical");
        }
        else
        {
            m_data.m_velocityY -= (m_gravity * Time.deltaTime);
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

        //if (grounded && Input.GetButton("MoveBox") && m_data.m_closeToBox)
        //{
        //    m_data.m_moveingBox = true;
        //}

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

    void OnCollisionStay(Collision Other)
    {
        angle = Vector2.Angle(Other.contacts[0].normal, Vector2.up);

        if (Mathf.Approximately(angle, 0.0f))
        {
            //grounded = true;
            //DoubleJump = true;

            m_data.m_contacts[2] = true;

            if (m_data.m_contacts[0])
            {
                m_data.m_squished = true;
                Debug.Log("Squished!");
            }
        }
        else if (Mathf.Approximately(angle, 180.0f))
        {
            m_data.m_contacts[0] = true;

            if (m_data.m_contacts[2])
            {
                m_data.m_squished = true;
                Debug.Log("Squished!");
            }
        }
        else if (Mathf.Approximately(angle, 90.0f))
        {
            if (Other.transform.position.x > m_rigb.position.x)
            {
                m_data.m_contacts[1] = true;

                if (m_data.m_contacts[3])
                {
                    m_data.m_squished = true;
                    Debug.Log("Squished!");
                }
            }
            else
            {
                m_data.m_contacts[3] = true;

                if (m_data.m_contacts[1])
                {
                    m_data.m_squished = true;
                    Debug.Log("Squished!");
                }
            }
        }

        if (m_data.m_playerWallSlide)
        {
            if (Mathf.Approximately(angle, 90f))
            {
                //grounded = true;
                //DoubleJump = true;

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
}
