using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    private Timer m_displayTimer;
    private Text m_displayText;

    void Start()
    {
        m_displayText = GetComponent<Text>();
        m_displayText.text = "";

        m_displayTimer = new Timer();
        m_displayTimer.m_time = 4.0f;
    }
    
    void Update()
    {
        if (m_displayTimer.m_playing)
        {
            m_displayTimer.Cycle();

            if (m_displayTimer.m_completed)
            {
                m_displayText.text = "";
            }
        }
    }

    public void ReceveText(string text)
    {
        m_displayText.text = text;
        m_displayTimer.Play();
    }
}
