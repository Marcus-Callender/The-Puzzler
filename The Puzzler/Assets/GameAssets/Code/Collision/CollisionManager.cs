﻿using System.Collections;
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

    private float FindRight(int z)
    {
        return m_dataArray[z].m_posX + (m_dataArray[z].m_width * 0.5f);
    }

    private float FindLeft(int z)
    {
        return m_dataArray[z].m_posX - (m_dataArray[z].m_width * 0.5f);
    }

    private float FindTop(int z)
    {
        return m_dataArray[z].m_posY + (m_dataArray[z].m_heght * 0.5f);
    }

    private float FindBottom(int z)
    {
        return m_dataArray[z].m_posY - (m_dataArray[z].m_heght * 0.5f);
    }

    private float FindNewRight(int z)
    {
        return m_dataArray[z].m_newPosX + (m_dataArray[z].m_width * 0.5f);
    }

    private float FindNewLeft(int z)
    {
        return m_dataArray[z].m_newPosX - (m_dataArray[z].m_width * 0.5f);
    }

    private float FindNewTop(int z)
    {
        return m_dataArray[z].m_newPosY + (m_dataArray[z].m_heght * 0.5f);
    }

    private float FindNewBottom(int z)
    {
        return m_dataArray[z].m_newPosY - (m_dataArray[z].m_heght * 0.5f);
    }

    void Update()
    {
        for (int z = 0; z < m_cs_dataArrayLength; z++)
        {
            for (int x = z + 1; x < m_cs_dataArrayLength; x++)
            {
                //if (m_dataArray[z] != null && m_dataArray[x] != null)
                //{
                //    if (FindLeft(z) < FindRight(x) || FindLeft(x) > FindRight(z))
                //    {
                //        if (FindNewBottom(z) < FindNewTop(x))
                //        {
                //            m_dataArray[z].m_colidedVertical = true;
                //            m_dataArray[x].m_colidedVertical = true;
                //
                //            float colisionPoint = (FindNewBottom(z) + FindNewTop(x)) * 0.5f;
                //
                //            m_dataArray[z].m_colisionPosY = colisionPoint;
                //            m_dataArray[x].m_colisionPosY = colisionPoint;
                //        }
                //    }
                //
                //    //else if (FindLeft(x) < FindRight(z))  //(FindLeft(x) < FindRight(z))
                //    //{
                //    //    if (FindNewBottom(z) < FindNewTop(x))
                //    //    {
                //    //        m_dataArray[z].m_colidedVertical = true;
                //    //        m_dataArray[x].m_colidedVertical = true;
                //    //
                //    //        float colisionPoint = (FindNewBottom(z) + FindNewTop(x)) * 0.5f;
                //    //
                //    //        m_dataArray[z].m_colisionPosY = colisionPoint;
                //    //        m_dataArray[x].m_colisionPosY = colisionPoint;
                //    //    }
                //    //}
                //}

                bool colLeft = false;
                bool colRight = false;
                bool colTop = false;
                bool colBottom = false;

                if (FindNewLeft(z) > FindNewRight(x))
                {
                    colLeft = true;
                    Debug.Log("Left");
                }

                if (FindNewLeft(x) > FindNewRight(z))
                {
                    colRight = true;
                    Debug.Log("Right");
                }

                if (FindNewTop(z) < FindNewBottom(x))
                {
                    colTop = true;
                    Debug.Log("Top");
                }

                if (FindNewTop(x) > FindNewBottom(z) && FindNewTop(z) > FindNewBottom(x))
                {
                    if (FindLeft(x) < FindRight(z) && FindLeft(z) < FindRight(x))
                    {
                        colBottom = true;
                        Debug.Log("Bottom Collision");

                        m_dataArray[z].m_colidedVertical = true;
                        m_dataArray[x].m_colidedVertical = true;

                        float colisionPoint = (FindNewBottom(z) + FindNewTop(x)) * 0.5f;

                        m_dataArray[z].m_colisionPosY = colisionPoint;
                        m_dataArray[x].m_colisionPosY = colisionPoint;

                    }
                }

                float LeftZ = FindNewLeft(z);
                float LeftX = FindNewLeft(x);
                float RightZ = FindNewRight(z);
                float RightX = FindNewRight(x);

                if (FindBottom(x) < FindTop(z) && FindTop(x) > FindBottom(z))
                {
                    if (FindNewLeft(x) < FindNewRight(z) && FindNewLeft(z) < FindNewRight(x))
                    {
                        if (m_dataArray[z].m_posY > m_dataArray[x].m_posY)
                        {
                            m_dataArray[x].m_collisionTop = true;
                        }
                        else
                        {
                            m_dataArray[z].m_collisionTop = true;
                        }

                        //colBottom = true;
                        Debug.Log("Horizontal collision");

                        m_dataArray[z].m_colidedHorizontal = true;
                        m_dataArray[x].m_colidedHorizontal = true;

                        float colisionPoint = (FindNewLeft(z) + FindNewRight(x)) * 0.5f;

                        m_dataArray[z].m_colisionPosX = colisionPoint;
                        m_dataArray[x].m_colisionPosX = colisionPoint;

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

