using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool m_paused = false;
    private Transform m_menu;
    public GameObject m_resumeButton;

    private PlayerStateMachine m_player;

    void Start()
    {
        m_menu = transform.Find("Canvas");
        m_menu.gameObject.SetActive(false);

        if (m_resumeButton)
        {
            UnityEngine.UI.Button button = m_resumeButton.GetComponent<UnityEngine.UI.Button>();

            if (button)
            {
                button.onClick.AddListener(TogglePause);
            }
        }

        m_player = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        m_paused = !m_paused;
        m_player.Pause(m_paused);

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
