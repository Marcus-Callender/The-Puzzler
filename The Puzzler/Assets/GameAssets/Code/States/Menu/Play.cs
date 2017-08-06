using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public string m_sceneToLoad;
    public AudioSource m_audio;

    void Start()
    {
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(StartPlayGame);
        m_audio = GetComponent<AudioSource>();
    }

    void StartPlayGame()
    {
        StartCoroutine(PlayGame());
    }

    IEnumerator PlayGame()
    {
        m_audio.Play();
        //m_audio.PlayOneShot(m_audio.clip, 1.0f);

        yield return new WaitForSeconds(0.15f);

        SceneManager.LoadScene(m_sceneToLoad, LoadSceneMode.Single);
    }
}
