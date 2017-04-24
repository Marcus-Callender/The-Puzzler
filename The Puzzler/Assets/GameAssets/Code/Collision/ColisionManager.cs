using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionManager : MonoBehaviour
{
    static ColisionManager m_instance;
    const int m_cs_dataArrayLength = 10;
    ColisionData[] m_dataArray = new ColisionData[m_cs_dataArrayLength];
    
    void Awake()
    {
        m_instance = this;
    }
    
    void Update()
    {

    }

    public static ColisionManager GetInstance()
    {
        return m_instance;
    }

    public void RegisterData(ColisionData data)
    {
        for (int z = 0; z < m_cs_dataArrayLength; z++)
        {
            if (m_dataArray[z] == null)
            {
                m_dataArray[z] = data;
                break;
            }
        }
    }

    public void UnRegisterData(ColisionData data)
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

