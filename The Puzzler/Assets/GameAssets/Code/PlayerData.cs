using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public float m_velocityX = 0.0f;
    public float m_velocityY = 0.0f;

    public bool m_moveingBox = false;
    public bool m_closeToBox = false;

    private Rigidbody m_rigb;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {

    }

    void FixedUpdate()
    {
        m_rigb.velocity = new Vector3(m_velocityX, m_velocityY);
    }
}
