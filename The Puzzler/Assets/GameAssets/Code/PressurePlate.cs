﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    bool m_activated = false;
    Material m_mat;

    public GameObject[] m_linkedObjects;

    void Start()
    {
        m_mat = GetComponent<Renderer>().material;

        m_mat.color = Color.red;
    }

    void Update()
    {

    }

    private void OnCollisionStay(Collision Other)
    {
        float angle = Vector2.Angle(Other.contacts[0].normal, Vector2.up);


        if (Mathf.Approximately(angle, 180.0f))
        {
            if (Other.gameObject.tag == "Player" || Other.gameObject.tag == "Box")
            {
                if (!m_activated)
                {
                    for (int z = 0; z < m_linkedObjects.Length; z++)
                    {
                        ButtonInteraction script = m_linkedObjects[z].GetComponent<ButtonInteraction>();

                        if (script)
                        {
                            script.OnInteract();
                        }
                    }
                }

                m_activated = true;

                m_mat.color = Color.green;
            }
        }
    }

    private void OnCollisionExit(Collision Other)
    {
        if (Other.gameObject.tag == "Player" || Other.gameObject.tag == "Box")
        {
            if (m_activated)
            {
                for (int z = 0; z < m_linkedObjects.Length; z++)
                {
                    ButtonInteraction script = m_linkedObjects[z].GetComponent<ButtonInteraction>();

                    if (script)
                    {
                        script.OnInteract();
                    }
                }
            }

            m_activated = false;

            m_mat.color = Color.red;
        }
    }
}