using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

using System;
using System.IO;

public class ResetData : MonoBehaviour
{
    public AudioSource m_audio;

    void Start()
    {
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(StartReset);
        m_audio = GetComponent<AudioSource>();
    }

    void StartReset()
    {
        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        m_audio.Play();

        yield return new WaitForSeconds(0.15f);

        string directory = Directory.GetCurrentDirectory();
        
        string saveDir = directory + "\\save.sav";
        string posDir = directory + "\\posSave.sav";

        if (File.Exists(saveDir))
        {
            File.Delete(saveDir);
        }

        if (File.Exists(posDir))
        {
            File.Delete(posDir);
        }
    }
}
