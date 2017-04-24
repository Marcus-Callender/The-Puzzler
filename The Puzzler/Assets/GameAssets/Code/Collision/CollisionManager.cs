using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    static CollisionManager m_instance;
    const int m_cs_dataArrayLength = 10;
    CollisionData[] m_dataArray = new CollisionData[m_cs_dataArrayLength];
    
    void Awake()
    {
        m_instance = this;
    }
    
    void Update()
    {
        for (int z = 0; z < m_cs_dataArrayLength; z++)
        {
            for (int x = z + 1; x < m_cs_dataArrayLength; x++)
            {
                if (m_dataArray[z] != null && m_dataArray[x] != null)
                {
                    if (m_dataArray[z].m_newPosY - (m_dataArray[z].m_heght * 0.5f) < m_dataArray[x].m_newPosY + (m_dataArray[x].m_heght * 0.5f))
                    {
                        m_dataArray[z].m_colidedVertical = true;
                        m_dataArray[x].m_colidedVertical = true;

                        float colisionPoint = ((m_dataArray[z].m_newPosY - (m_dataArray[z].m_heght * 0.5f)) + (m_dataArray[x].m_newPosY + (m_dataArray[x].m_heght * 0.5f))) * 0.5f;

                        m_dataArray[z].m_colisionPosY = colisionPoint;
                        m_dataArray[x].m_colisionPosY = colisionPoint;
                    }
                }
            }
        }
    }

    public static CollisionManager GetInstance()
    {
        return m_instance;
    }

    public void RegisterData(CollisionData data)
    {
        int lowestFreeSpace = -1;

        for (int z = 0; z < m_cs_dataArrayLength; z++)
        {
            if (m_dataArray[z] == null && lowestFreeSpace == -1)
            {
                lowestFreeSpace = z;
            }
            else if (m_dataArray[z] != null)
            {
                if (m_dataArray[z].m_id == data.m_id)
                {
                    return;
                }
            }
        }

        m_dataArray[lowestFreeSpace] = data;
    }

    public void UnRegisterData(CollisionData data)
    {
        for (int z = 0; z < m_cs_dataArrayLength; z++)
        {
            if (m_dataArray[z] == data)
            {
                m_dataArray[z] = null;
                break;
            }
        }
    }
}

