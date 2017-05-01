using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMovement : ButtonInteraction
{
    private Rigidbody m_rigb;

    private Vector3 m_point1;
    public Vector3 m_point2;

    public float m_speed = 1.0f;
    private float m_distance = 0.0f;
    private float m_traveledDistance = 0.0f;

    // when true this is moving from point 1 to point 2 and vice versa when false
    private bool m_goToPoint2 = false;
    private bool m_moveing = false;

    private Vector3 m_speedSegments;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();

        m_point1 = gameObject.transform.position;

        m_speedSegments = m_point2 - m_point1;
        m_distance = Mathf.Abs(m_speedSegments.x) + Mathf.Abs(m_speedSegments.y) + Mathf.Abs(m_speedSegments.z);

        m_speedSegments.x = m_speedSegments.x / m_distance;
        m_speedSegments.y = m_speedSegments.y / m_distance;
        m_speedSegments.z = m_speedSegments.z / m_distance;

        Debug.Log(m_speedSegments);
    }

    void Update()
    {
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
                m_rigb.velocity = m_speedSegments * -m_speed;

                m_traveledDistance -= m_speed * Time.deltaTime;

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
}
