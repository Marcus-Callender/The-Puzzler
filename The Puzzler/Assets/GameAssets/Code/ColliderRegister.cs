using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRegister : MonoBehaviour
{
    static ColliderRegister m_instance;
    const int m_cs_dataArrayLength = 10;
    CollisionBox[] m_dataArray = new CollisionBox[m_cs_dataArrayLength];

    void Awake()
    {
        m_instance = this;
    }
    
    void AddColider(CollisionBox data)
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

    void RemoveCollider(CollisionBox data)
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
    CollisionBox GetElement(int z)
    {
        return m_dataArray[z];
    }
}
