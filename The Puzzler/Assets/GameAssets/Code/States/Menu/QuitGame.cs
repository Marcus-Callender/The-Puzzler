using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    public AudioSource m_audio;

    void Start()
    {
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(StartQuit);
        m_audio = GetComponent<AudioSource>();
    }

    void StartQuit()
    {
        StartCoroutine(Quit());
    }

    IEnumerator Quit()
    {
        m_audio.Play();

        yield return new WaitForSeconds(0.15f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
