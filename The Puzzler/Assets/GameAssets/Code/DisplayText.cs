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
        // gets a refrence to the text componant and clears it
        m_displayText = GetComponent<Text>();
        m_displayText.text = "";

        // initializes the display timer
        m_displayTimer = new Timer();
        m_displayTimer.m_time = 2.5f;
    }
    
    void Update()
    {
        if (m_displayTimer.m_playing)
        {

            m_displayTimer.Cycle();

            if (m_displayTimer.m_completed)
            {
                // clears the text when the timer has completed
                m_displayText.text = "";
            }
        }
    }

    public void ReceveText(string text)
    {
        m_displayText.text = text;
        // starts the display timer from the begining
        m_displayTimer.Play();
    }
}
