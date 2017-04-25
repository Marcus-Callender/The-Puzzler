using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBox : MonoBehaviour
{
    CollisionManager m_manager;
    CollisionData m_data;

    public ColiderType m_type;

    static int m_s_instances = 0;
    int m_id = 0;

    // tracks if the object collided on the previous frame
    public bool m_colidedHorizontal;
    public bool m_colidedVertical;

    void Start()
    {
        m_data = new CollisionData();

        m_id = m_s_instances;
        m_s_instances++;

        Transform transf = gameObject.transform;

        m_data.m_id = m_id;

        m_data.m_posX = transf.position.x;
        m_data.m_posY = transf.position.y;
        m_data.m_width = transf.localScale.x;
        m_data.m_heght = transf.localScale.y;
        
        m_data.m_newPosX = m_data.m_posX;
        m_data.m_newPosY = m_data.m_posY;

        m_manager = CollisionManager.GetInstance();
        m_manager.RegisterData(m_data);
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        if (!m_data.m_colidedVertical)
        {
            m_data.m_posY = m_data.m_newPosY;

            Vector3 pos = gameObject.transform.position;
            pos.y = m_data.m_posY;
            gameObject.transform.position = pos;
        }

        if (!m_data.m_colidedHorizontal)
        {
            m_data.m_posX = m_data.m_newPosX;

            Vector3 pos = gameObject.transform.position;
            pos.x = m_data.m_posX;
            gameObject.transform.position = pos;
        }

        //m_data.m_posX = m_data.m_newPosX;
        //m_data.m_posY = m_data.m_newPosY;

        m_data.m_collisionTop = false;

        m_data.m_newPosX = m_data.m_posX;
        m_data.m_newPosY = m_data.m_posY;
        
        m_colidedHorizontal = m_data.m_colidedHorizontal;
        m_colidedVertical = m_data.m_colidedVertical;

        m_data.m_colidedHorizontal = false;
        m_data.m_colidedVertical = false;
    }

    // moves the object this many units
    public void Move(float x, float y)
    {
        if (x != 0.0f)
           m_data.m_newPosX = m_data.m_posX + x;
        
        if (y != 0.0f)
            m_data.m_newPosY = m_data.m_posY + y;
    }

    // moves the object to this position
    public void MoveTo(float x, float y)
    {
        m_data.m_newPosX = x;
        m_data.m_newPosY = y;
    }

    public CollisionData GetCollData()
    {
        return m_data;
    }
}

