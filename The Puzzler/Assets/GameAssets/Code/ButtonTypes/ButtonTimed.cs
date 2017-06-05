using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTimed : MonoBehaviour
{
    bool m_activated = true;
    Material m_mat;

    private Timer m_Timer;
    public float m_pressedTime = 3.0f;

    public GameObject[] m_linkedObjects;

    void Start()
    {
        m_mat = GetComponent<Renderer>().material;

        m_Timer = new Timer();
        m_Timer.m_time = m_pressedTime;

        m_mat.color = Color.blue;
    }

    void Update()
    {
        m_Timer.Cycle();

        if (m_Timer.m_completed && !m_activated)
        {
            m_activated = true;
            m_mat.color = Color.blue;

            for (int z = 0; z < m_linkedObjects.Length; z++)
            {
                ButtonInteraction script = m_linkedObjects[z].GetComponent<ButtonInteraction>();

                if (script)
                {
                    script.OnInteract();
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player"&& m_activated)
        {
            PlayerData player = other.GetComponent<PlayerData>();

            if (player)
            {
                if (player.m_pressingButton)
                {
                    m_activated = false;

                    m_mat.color = Color.red;
                    
                    for (int z = 0; z < m_linkedObjects.Length; z++)
                    {
                        ButtonInteraction script = m_linkedObjects[z].GetComponent<ButtonInteraction>();
                    
                        if (script)
                        {
                            script.OnInteract();
                        }
                    }

                    m_Timer.Play();
                }
            }
        }
    }
}
