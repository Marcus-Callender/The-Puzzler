using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovenemt : MonoBehaviour
{
    Rigidbody m_rigb;
    BasicMovement m_player;

    void Start()
    {
        m_rigb = gameObject.GetComponent<Rigidbody>();
        m_player = GetComponent<BasicMovement>();
    }

    void LateUpdate()
    {
        m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);
    }

    private void FixedUpdate()
    {
        m_rigb.velocity = new Vector3(0.0f, m_rigb.velocity.y);

    }
}
