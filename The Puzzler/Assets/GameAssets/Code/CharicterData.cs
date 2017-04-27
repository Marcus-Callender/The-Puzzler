using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CHARICTER_STATES
{
    STAND,
    WALK,
    JUMP,

    NUMBER_OF_STATES,
    NULL
}

public class CharicterData : MonoBehaviour
{
    public Rigidbody m_rigb;
    public float m_xVelocity = 0.0f;
    public float m_yVelocity = 0.0f;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        m_rigb.velocity = new Vector3(m_xVelocity, m_yVelocity);
    }
}
