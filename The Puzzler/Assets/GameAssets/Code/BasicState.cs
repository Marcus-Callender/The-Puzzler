using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicState : MonoBehaviour
{
    protected PlayerData m_data;
    protected Rigidbody m_rigb;
    
    float direction;
    float angle;
    public bool m_useWallGravity = false;


    public void Initialize(Rigidbody rigb, PlayerData data)
    {
        m_rigb = rigb;
        m_data = data;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {

    }

    public virtual E_PLAYER_STATES Cycle()
    {
        return E_PLAYER_STATES.NULL;
    }

    public virtual E_PLAYER_STATES PhysCycle()
    {
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

    public virtual E_PLAYER_STATES LeaveColision(string _tag)
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

        m_data.m_pressingButton = false;

        if (Input.GetButtonDown("PressButton"))
        {
            m_data.m_pressingButton = true;
        }
        
    }

    void OnCollisionStay(Collision Other)
    {
        angle = Vector2.Angle(Other.contacts[0].normal, Vector2.up);

        if (Mathf.Approximately(angle, 0.0f))
        {
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
    }
}
