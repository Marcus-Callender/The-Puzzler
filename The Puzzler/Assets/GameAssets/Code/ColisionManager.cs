using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionManager : MonoBehaviour
{
    static ColisionManager m_instance;
    ColisionData[] m_dataArray;// = new ColisionData[10];
    
    void Awake()
    {
        m_instance = this;
        m_dataArray.Initialize();
    }
    
    void LateUpdate()
    {
        for (int z = 0; z < m_dataArray.Length; z++)
        {
            Debug.Log(m_dataArray[z]);

        }
    }

    public static ColisionManager GetInstance()
    {
        return m_instance;
    }

    public void Registered(ColisionData data)
    {
        Debug.Log(data.m_posX + ", " + data.m_posY + ", " + data.m_heght + ", " + data.m_width + ".");
    }

    public void RegisterData(ColisionData data)
    {
        m_dataArray[m_dataArray.Length] = data;
    }
}
