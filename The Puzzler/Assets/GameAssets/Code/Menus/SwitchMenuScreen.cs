using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenuScreen : MonoBehaviour
{
    public GameObject m_nextMenu;
    public AudioSource m_audio;

    void Start()
    {
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(Switch);
        m_audio = GetComponent<AudioSource>();

    }
    
    void Switch()
    {
        m_audio.Play();

        if (m_nextMenu)
        {
            m_nextMenu.SetActive(true);

            GameObject myScreen = gameObject;

            while (myScreen.tag != "MenuScreen")
            {
                myScreen = myScreen.transform.parent.gameObject;
            }

            myScreen.SetActive(false);
        }
    }
}
