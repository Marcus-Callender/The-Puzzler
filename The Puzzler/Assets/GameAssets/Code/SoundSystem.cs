using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    private AudioSource m_audioSource;

    public AudioClip[] m_music;
    public int m_musicIndex = 0;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();

        m_audioSource.clip = m_music[m_musicIndex];
    }
    
    void Update()
    {

    }

    public void NextTrack()
    {
        if (m_music.Length - 1 > m_musicIndex)
        {
            m_musicIndex++;

            m_audioSource.clip = m_music[m_musicIndex];
            m_audioSource.Play();
        }
    }
}
