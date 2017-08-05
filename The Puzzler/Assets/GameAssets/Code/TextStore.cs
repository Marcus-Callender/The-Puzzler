using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextStore : MonoBehaviour
{
    private DisplayText m_screenText;
    public string m_textToDisplay;

    void Start()
    {
        m_screenText = GameObject.Find("UI").GetComponentInChildren<DisplayText>();

        if (!m_screenText)
        {
            Debug.Log("--- Unable to find text to write to ---");
        }
    }
    
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ghost")
        {
            m_screenText.ReceveText(m_textToDisplay);
        }
    }
}
