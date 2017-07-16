using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool m_paused = false;
    private Transform m_menu;

    void Start()
    {
        m_menu = transform.Find("Canvas");
        m_menu.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            m_paused = !m_paused;

            if (m_paused)
            {
                m_menu.gameObject.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else
            {
                m_menu.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }
}
