using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBox : MonoBehaviour
{
    CollisionManager m_manager;
    //CollisionData m_data;

    static int m_s_instances = 0;
    public int m_id = 0;

    public bool m_colidedHorizontal;
    public bool m_colidedVertical;

    // tracks if the object collided on the previous frame
    public bool m_PrevColidedHorizontal;
    public bool m_PrevColidedVertical;

    public float m_posX;
    public float m_posY;
    public float m_width;
    public float m_heght;

    public float m_newPosX;
    public float m_newPosY;

    public bool m_collisionTop;

    public float m_colisionPosX;
    public float m_colisionPosY;

    void Start()
    {
        //m_data = new CollisionData();

        m_id = m_s_instances;
        m_s_instances++;

        Transform transf = gameObject.transform;

        
        m_posX = transf.position.x;
        m_posY = transf.position.y;
        m_width = transf.localScale.x;
        m_heght = transf.localScale.y;
        
        m_newPosX = m_posX;
        m_newPosY = m_posY;

        m_manager = CollisionManager.GetInstance();
        m_manager.RegisterData(this);
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        if (!m_colidedVertical)
        {
            m_posY = m_newPosY;

            Vector3 pos = gameObject.transform.position;
            pos.y = m_posY;
            gameObject.transform.position = pos;
        }

        if (!m_colidedHorizontal)
        {
            m_posX = m_newPosX;

            Vector3 pos = gameObject.transform.position;
            pos.x = m_posX;
            gameObject.transform.position = pos;
        }

        //m_data.m_posX = m_data.m_newPosX;
        //m_data.m_posY = m_data.m_newPosY;

        m_collisionTop = false;

        m_newPosX = m_posX;
        m_newPosY = m_posY;
        
        m_PrevColidedHorizontal = m_colidedHorizontal;
        m_PrevColidedVertical = m_colidedVertical;

        m_colidedHorizontal = false;
        m_colidedVertical = false;
    }

    // moves the object this many units
    public void Move(float x, float y)
    {
        if (x != 0.0f)
           m_newPosX = m_posX + x;
        
        if (y != 0.0f)
            m_newPosY = m_posY + y;
    }

    // moves the object to this position
    public void MoveTo(float x, float y)
    {
        m_newPosX = x;
        m_newPosY = y;
    }

    /*public CollisionData GetCollData()
    {
        return m_data;
    }*/
}

