using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : ButtonInteraction
{
    private Vector3 m_point1;
    public Vector3 m_point2;

    public float m_speed = 1.0f;

    // when true this is moving from point 1 to point 2 and vice versa when false
    private bool m_forwardJourney = true;
    private bool m_stoped = false;

    void Start()
    {
        m_point1 = gameObject.transform.position;
    }

    void Update()
    {

    }
}
