﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLazerGenerator : MonoBehaviour
{
    private LineRenderer m_line;
    private Timer m_timer;

    private Timer m_activeTimer;
    private Timer m_inactiveTimer;

    public float m_activeTime = 1.5f;
    public float m_inactiveTime = 1.5f;

    public void Start()
    {
        m_activeTimer = new Timer();
        m_activeTimer.m_time = m_activeTime;
        m_activeTimer.Play();

        m_inactiveTimer = new Timer();
        m_inactiveTimer.m_time = m_inactiveTime;

        m_timer = new Timer();
        m_timer.m_time = m_activeTime * 0.5f;
        m_timer.Play();
    }

    public void Update()
    {
        m_activeTimer.Cycle();
        m_inactiveTimer.Cycle();

        if (!m_activeTimer.m_completed)
        {
            if (!m_activeTimer.m_playing)
            {
                m_activeTimer.Play();
            }

            RaycastHit hit;

            Physics.Raycast(transform.position, -Vector3.up, out hit);

            m_line = GetComponent<LineRenderer>();
            m_line.material = new Material(Shader.Find("Sprites/Default"));

            if (hit.collider.gameObject.tag == "Player")
            {
                PlayerData data = hit.collider.gameObject.GetComponent<PlayerData>();
                data.m_squished = true;
            }
            else if (hit.collider.gameObject.tag == "Enemy")
            {
                Enemy data = hit.collider.gameObject.GetComponent<Enemy>();
                data.m_KOd = true;
            }

            Vector3[] positions = new Vector3[2];
            positions[0] = gameObject.transform.position;
            positions[1] = gameObject.transform.position;
            positions[1].y -= hit.distance;

            m_line.positionCount = positions.Length;
            m_line.SetPositions(positions);
            m_line.startWidth = 0.1f;
            m_line.endWidth = 0.1f;

            float alpha = 1.0f;
            Gradient gradient = new Gradient();

            Color col = new Color(1.0f, m_timer.GetLerp(), m_timer.GetLerp(), 0.5f);

            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(col, 0.0f), new GradientColorKey(col, 1.0f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f)
                }
            );

            m_line.colorGradient = gradient;

            m_timer.Cycle();

            if (m_timer.m_completed)
            {
                m_timer.Play(!m_timer.m_reversed);
            }
        }
        else
        {
            if (!m_inactiveTimer.m_playing)
            {
                m_inactiveTimer.Play();
            }

            Gradient gradient = new Gradient();

            Color col = new Color(1.0f, m_timer.GetLerp(), m_timer.GetLerp(), 0.0f);

            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(col, 0.0f), new GradientColorKey(col, 1.0f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f)
                }
            );

            m_line.colorGradient = gradient;
        }

        if (m_activeTimer.m_completed && m_inactiveTimer.m_completed)
        {
            m_activeTimer.Play();
            m_inactiveTimer.m_playing = false;
        }
    }
}
