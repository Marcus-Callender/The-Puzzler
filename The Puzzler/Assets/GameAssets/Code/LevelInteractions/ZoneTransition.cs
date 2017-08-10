using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ZoneTransition : MonoBehaviour
{
    // 0 = red, 1 = green, 2 = blue
    float[] m_colours = new float[3];
    float m_colourTransitionTime = 2.5f;
    
    // used in calculations as '*' is faster than '/'
    float m_colourTransitionRate;

    // 0 = r -> g, 1 = g -> b, 2 = b -> r
    int m_transitionStage = 0;

    Material m_mat;

    public string m_sceneToLoad;

    void Start()
    {
        m_colourTransitionRate = 1.0f / m_colourTransitionTime;

        m_colours[0] = 1.0f;
        m_colours[1] = 0.0f;
        m_colours[2] = 0.0f;

        m_mat = GetComponent<Renderer>().material;
        m_mat.color = new Color(m_colours[0], m_colours[1], m_colours[2], 1.0f);
    }
    
    void Update()
    {
        float dt = Time.deltaTime;

        if (m_transitionStage == 0)
        {
            m_colours[0] -= m_colourTransitionRate * dt;
            m_colours[1] += m_colourTransitionRate * dt;

            if (m_colours[0] <= 0.0f)
            {
                m_colours[0] = 0.0f;
                m_colours[1] = 1.0f;

                m_transitionStage++;
            }
        }
        else if (m_transitionStage == 1)
        {
            m_colours[1] -= m_colourTransitionRate * dt;
            m_colours[2] += m_colourTransitionRate * dt;

            if (m_colours[1] <= 0.0f)
            {
                m_colours[1] = 0.0f;
                m_colours[2] = 1.0f;

                m_transitionStage++;
            }
        }
        else if (m_transitionStage == 2)
        {
            m_colours[2] -= m_colourTransitionRate * dt;
            m_colours[0] += m_colourTransitionRate * dt;

            if (m_colours[2] <= 0.0f)
            {
                m_colours[2] = 0.0f;
                m_colours[0] = 1.0f;

                m_transitionStage = 0;
            }
        }

        m_mat.color = new Color(m_colours[0], m_colours[1], m_colours[2], 1.0f);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(m_sceneToLoad, LoadSceneMode.Single);
        }
    }
}
