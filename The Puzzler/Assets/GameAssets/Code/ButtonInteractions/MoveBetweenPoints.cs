using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : ButtonInteraction
{
    private Rigidbody m_rigb;

    private Vector3 m_point1;
    //private Vector3 m_point2;
    public Vector3 m_distanceToMove;

    public float m_speed = 1.0f;
    private float m_distance = 0.0f;
    private float m_traveledDistance = 0.0f;

    // when true this is moving from point 1 to point 2 and vice versa when false
    private bool m_goToPoint2 = false;
    public bool m_moveing = false;

    private Vector3 m_speedSegments;

    private GameObject m_keepScale;

    public override void Start()
    {
        base.Start();

        m_rigb = GetComponent<Rigidbody>();

        m_point1 = gameObject.transform.position;
        
        m_distance = Mathf.Abs(m_distanceToMove.x) + Mathf.Abs(m_distanceToMove.y) + Mathf.Abs(m_distanceToMove.z);

        m_speedSegments.x = m_distanceToMove.x / m_distance;
        m_speedSegments.y = m_distanceToMove.y / m_distance;
        m_speedSegments.z = m_distanceToMove.z / m_distance;

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
            //m_rigb.isKinematic = false;

            if (m_goToPoint2)
            {
                m_rigb.velocity = m_speedSegments * m_speed;

                m_traveledDistance += m_speed * Time.deltaTime;
            }
            else
            {
                m_rigb.velocity = m_speedSegments * -m_speed;

                m_traveledDistance -= m_speed * Time.deltaTime;
            }

            if (HasRechedDestination())
            {
                m_moveing = false;
            }
        }
        else
        {
            m_rigb.velocity = Vector3.zero;
            //m_rigb.isKinematic = true;
        }
    }

    public override void Activate()
    {
        base.Activate();

        m_goToPoint2 = !m_goToPoint2;
        m_moveing = true;
    }

    private bool HasRechedDestination()
    {
        if (m_goToPoint2)
        {
            if (GetDistanceBetweenPoints(gameObject.transform.position, m_point1) >= GetDistanceBetweenPoints(m_point1, m_point1 + m_distanceToMove))
            {
                return true;
            }
        }
        else
        {
            if (GetDistanceBetweenPoints(gameObject.transform.position, m_point1 + m_distanceToMove) >= GetDistanceBetweenPoints(m_point1, m_point1 + m_distanceToMove))
            {
                return true;
            }
        }

        return false;
    }

    private float GetDistanceBetweenPoints(Vector3 point1, Vector3 point2)
    {
        return Mathf.Abs(point1.x - point2.x) + Mathf.Abs(point1.y - point2.y) + Mathf.Abs(point1.z - point2.z);
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
