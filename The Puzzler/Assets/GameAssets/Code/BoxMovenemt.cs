using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovenemt : MonoBehaviour
{
    Rigidbody m_rigb;
    PlayerData m_player;
    Rigidbody m_playerRigb;

    float m_playerInteractDistance = 0.5f;
    float m_playerBoxMinDistance = 0.0f;

    public bool m_requestStop = false;

    void Start()
    {
        m_rigb = gameObject.GetComponent<Rigidbody>();
        //m_player = FindObjectOfType<PlayerData>();
        m_player = GameObject.Find("Player").GetComponent<PlayerData>();

        m_playerRigb = m_player.gameObject.GetComponent<Rigidbody>();

        m_playerBoxMinDistance = (gameObject.transform.localScale.x + m_player.gameObject.transform.localScale.x) * 0.5f;
    }

    void Update()
    {
        //m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);

        float distance = Mathf.Abs(gameObject.transform.position.x - m_player.gameObject.transform.position.x);
        

        if (distance < m_playerBoxMinDistance + m_playerInteractDistance)
        {
            float top = m_rigb.transform.position.y + (m_rigb.transform.localScale.y * 0.5f);
            float bottom = m_rigb.transform.position.y - (m_rigb.transform.localScale.y * 0.5f);

            if (m_playerRigb.transform.position.y > bottom && m_playerRigb.transform.position.y < top)
            {
                m_player.m_closeToBox = true;

                Debug.Log("Player Close");

                if (m_player.m_moveingBox && m_player.m_linkedBox == null)
                {
                    m_player.m_linkedBox = this;
                }
            }
        }
    }

    void LateUpdate()
    {
        //m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);

        float distance = Mathf.Abs(gameObject.transform.position.x - m_player.gameObject.transform.position.x);

        if (m_requestStop)
        {
            Debug.Log("STOP");
        }

        if (distance < m_playerBoxMinDistance + m_playerInteractDistance && !m_requestStop)
        {
            float top = m_rigb.transform.position.y + (m_rigb.transform.localScale.y * 0.5f);
            float bottom = m_rigb.transform.position.y - (m_rigb.transform.localScale.y * 0.5f);

            if (m_playerRigb.transform.position.y > bottom && m_playerRigb.transform.position.y < top)
            {
                m_player.m_closeToBox = true;

                Debug.Log("Player Close");

                if (m_player.m_moveingBox)
                {
                    //m_rigb.velocity = new Vector3(m_player.m_velocityX, m_rigb.velocity.y);
                }

            }
        }

        if (m_player.m_moveingBox && Mathf.Abs(gameObject.transform.position.x - m_player.transform.position.x) < ((gameObject.transform.localScale.x + m_player.transform.localScale.x) * 0.5f))
        {
            Debug.Log("Overlapping");

            if (gameObject.transform.position.x > m_rigb.transform.position.x /*&& m_player.m_velocityX > 0.0f*/)
            {
                Vector3 newPos = gameObject.transform.position;

                newPos.x = m_player.transform.position.x - ((gameObject.transform.localScale.x + m_player.transform.localScale.x) * 0.5f);

                gameObject.transform.position = newPos;
            }
            else if (gameObject.transform.position.x < m_rigb.transform.position.x /*&& m_player.m_velocityX < 0.0f*/)
            {
                Vector3 newPos = gameObject.transform.position;

                newPos.x = m_player.transform.position.x + ((gameObject.transform.localScale.x + m_player.transform.localScale.x) * 0.5f);

                gameObject.transform.position = newPos;
            }
        }


        m_requestStop = false;
    }

    void FixedUpdate()
    {
        //m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);
    }

    public void Move(float xVelocity)
    {
        m_rigb.velocity = new Vector3(xVelocity, m_rigb.velocity.y);
    }
}
