using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMovement : ButtonInteraction
{
    private Rigidbody m_rigb;

    private Vector3 m_point1;
    public Vector3 m_point2;

    public float m_speed = 1.0f;
    public float m_speedReverse = 1.0f;
    private float m_distance = 0.0f;
    private float m_traveledDistance = 0.0f;

    // when true this is moving from point 1 to point 2 and vice versa when false
    public bool m_goToPoint2 = false;
    public bool m_moveing = false;

    private Vector3 m_speedSegments;

    private GameObject m_keepScale;

    public override void Start()
    {
        base.Start();

        m_rigb = GetComponent<Rigidbody>();

        m_point1 = gameObject.transform.position;

        m_speedSegments = m_point2 - m_point1;
        m_distance = Mathf.Abs(m_speedSegments.x) + Mathf.Abs(m_speedSegments.y) + Mathf.Abs(m_speedSegments.z);

        m_speedSegments.x = m_speedSegments.x / m_distance;
        m_speedSegments.y = m_speedSegments.y / m_distance;
        m_speedSegments.z = m_speedSegments.z / m_distance;

        m_keepScale = new GameObject();
        m_keepScale.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        m_keepScale.transform.SetParent(gameObject.transform);

        Debug.Log(m_speedSegments);
    }

    public override void Update()
    {
        base.Update();

        if (m_moveing)
        {
            if (m_goToPoint2)
            {
                m_rigb.velocity = m_speedSegments * m_speed;

                m_traveledDistance += m_speed * Time.deltaTime;

                if (m_traveledDistance >= m_distance)
                {
                    m_goToPoint2 = false;
                }
            }
            else
            {
                m_rigb.velocity = m_speedSegments * -m_speedReverse;

                m_traveledDistance -= m_speedReverse * Time.deltaTime;

                if (m_traveledDistance <= 0.0f)
                {
                    m_goToPoint2 = true;
                }
            }
        }
        else
        {
            m_rigb.velocity = Vector3.zero;
        }
    }

    public override void OnInteract()
    {
        base.OnInteract();

        m_moveing = !m_moveing;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(m_keepScale.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        for (int z = 0; z < children.Length; z++)
        {
            if (collision.gameObject.transform == children[z])
            {
                children[z].SetParent(null);
            }
        }
    }
}
