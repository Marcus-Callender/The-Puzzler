using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBox : MonoBehaviour
{
    ColisionManager m_manager;
    ColisionData m_data;

    void Start()
    {
        m_data = new ColisionData(); //gameObject.GetComponent<ColisionData>();

        Transform transf = gameObject.transform;

        m_data.m_posX = transf.position.x;
        m_data.m_posY = transf.position.y;
        m_data.m_width = transf.localScale.x;
        m_data.m_heght = transf.localScale.y;

        m_manager = ColisionManager.GetInstance();
        m_manager.RegisterData(m_data);
    }

    void Update()
    {
    }
}

