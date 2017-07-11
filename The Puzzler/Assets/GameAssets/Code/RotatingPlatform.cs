using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public bool m_active = false;
    public Vector3 m_rotation;
    public float m_speed;

    private GameObject m_keepScale;

    void Start()
    {
        m_keepScale = new GameObject();
        m_keepScale.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        m_keepScale.transform.SetParent(gameObject.transform);
    }

    void Update()
    {
        if (m_active)
        {
            transform.Rotate(m_rotation.normalized * m_speed * Time.deltaTime);
        }
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
