using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Text;

public class RandomizeText : MonoBehaviour
{
    private Timer m_changeTimer;
    private Text m_text;

    private int m_place = 0;

    void Start()
    {
        m_changeTimer = new Timer();
        m_changeTimer.m_time = Random.Range(0.032f, 0.064f);
        m_changeTimer.Play();

        m_text = GetComponent<Text>();
    }

    void Update()
    {
        m_changeTimer.Cycle(Time.unscaledDeltaTime);

        if (m_changeTimer.m_completed)
        {
            m_changeTimer.m_time = Random.Range(0.032f, 0.064f);
            m_changeTimer.Play();


            StringBuilder sb = new StringBuilder(m_text.text);
            //int random = m_place;
            int random = Random.Range(0, m_text.text.Length);

            if (char.IsUpper(sb[random]))
            {
                sb[random] = char.ToLower(sb[random]);
            }
            else if (char.IsLower(sb[random]))
            {
                sb[random] = char.ToUpper(sb[random]);
            }

            m_place++;

            if (m_place == m_text.text.Length)
            {
                m_place = 0;
            }

            m_text.text = sb.ToString();
        }
    }
}
