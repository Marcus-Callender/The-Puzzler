using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovenemt : MonoBehaviour
{
    Rigidbody m_rigb;
    PlayerData[] m_playerRefs;

    float m_playerInteractDistance = 0.5f;
    float m_playerBoxMinDistance = 0.0f;

    public bool m_requestStop = false;
    private GameObject m_linkedBox;

    void Start()
    {
        m_rigb = gameObject.GetComponent<Rigidbody>();
        m_playerRefs = FindObjectsOfType<PlayerData>();

        m_playerBoxMinDistance = (gameObject.transform.localScale.x + m_playerRefs[0].gameObject.transform.localScale.x) * 0.5f;
    }

    //void LateUpdate()
    //{
    //    for (int z = 0; z < m_playerRefs.Length; z++)
    //    {
    //        float distance = Mathf.Abs(gameObject.transform.position.x - m_playerRefs[z].gameObject.transform.position.x);
    //
    //        if (m_requestStop)
    //        {
    //            Debug.Log("STOP");
    //        }
    //
    //        if (distance < m_playerBoxMinDistance + m_playerInteractDistance && !m_requestStop)
    //        {
    //            float top = m_rigb.transform.position.y + (m_rigb.transform.localScale.y * 0.5f);
    //            float bottom = m_rigb.transform.position.y - (m_rigb.transform.localScale.y * 0.5f);
    //
    //            if (m_playerRefs[z].transform.position.y > bottom && m_playerRefs[z].transform.position.y < top)
    //            {
    //                m_playerRefs[z].m_closeToBox = true;
    //
    //                if (m_playerRefs[z].m_moveingBox && m_playerRefs[z].m_linkedBox == null)
    //                {
    //                    m_playerRefs[z].m_linkedBox = this;
    //                }
    //            }
    //        }
    //
    //        if (m_playerRefs[z].m_moveingBox && Mathf.Abs(gameObject.transform.position.x - m_playerRefs[z].transform.position.x) < ((gameObject.transform.localScale.x + m_playerRefs[z].transform.localScale.x) * 0.5f))
    //        {
    //            Debug.Log("Overlapping");
    //
    //            if (gameObject.transform.position.x > m_rigb.transform.position.x /*&& m_player.m_velocityX > 0.0f*/)
    //            {
    //                Vector3 newPos = gameObject.transform.position;
    //
    //                newPos.x = m_playerRefs[z].transform.position.x - ((gameObject.transform.localScale.x + m_playerRefs[z].transform.localScale.x) * 0.5f);
    //
    //                gameObject.transform.position = newPos;
    //            }
    //            else if (gameObject.transform.position.x < m_rigb.transform.position.x /*&& m_player.m_velocityX < 0.0f*/)
    //            {
    //                Vector3 newPos = gameObject.transform.position;
    //
    //                newPos.x = m_playerRefs[z].transform.position.x + ((gameObject.transform.localScale.x + m_playerRefs[z].transform.localScale.x) * 0.5f);
    //
    //                gameObject.transform.position = newPos;
    //            }
    //        }
    //    }
    //
    //    bool linked = false;
    //
    //    for (int z = 0; z < m_playerRefs.Length; z++)
    //    {
    //        if (m_playerRefs[z].m_linkedBox == this)
    //        {
    //            linked = true;
    //        }
    //    }
    //
    //    if (!linked)
    //    {
    //        Move(0.0f);
    //    }
    //
    //    m_requestStop = false;
    //}

    private void FixedUpdate()
    {
        //m_rigb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void Move(float xVelocity)
    {
        m_rigb.velocity = new Vector3(xVelocity, m_rigb.velocity.y);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Box")
        {
            m_linkedBox = other.gameObject;
        }

    }

    private void OnCollisionExit(Collision other)
    {
        //if (other.gameObject == m_linkedBox)
        //{
        //    //Rigidbody rigb = m_linkedBox.GetComponent<Rigidbody>();
        //    //
        //    //if (rigb)
        //    //{
        //    //    rigb.velocity.Set(0.0f, 0.0f, 0.0f);
        //    //}
        //
        //    m_rigb.velocity.Set(0.0f, 0.0f, 0.0f);
        //}

        m_rigb.velocity.Set(0.0f, 0.0f, 0.0f);
    }
}
