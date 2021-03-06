﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGenerator : ButtonInteraction
{
    private LineRenderer m_line;
    private Timer m_timer;

    public float m_cycleTime = 0.5f;

    public override void Start()
    {
        base.Start();

        // sets up the timer
        m_timer = new Timer();
        m_timer.m_time = m_cycleTime;
        m_timer.Play();

        m_line = GetComponent<LineRenderer>();
        m_line.material = new Material(Shader.Find("Sprites/Default"));
    }

    public override void Update()
    {
        base.Update();

        if (!m_activated)
        {
            RaycastHit hit;

            //if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            //    print("Found an object - distance: " + hit.distance);

            Physics.Raycast(transform.position, -transform.up, out hit);
            Debug.DrawRay(transform.position, -transform.up, Color.blue);

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
            //positions[1].y -= hit.distance;
            positions[1] += -transform.up * hit.distance;

            m_line.positionCount = positions.Length;
            m_line.SetPositions(positions);
            m_line.startWidth = 0.1f;
            m_line.endWidth = 0.1f;

            float alpha = 1.0f;
            Gradient gradient = new Gradient();

            Color col = new Color(1.0f, m_timer.GetLerp(), m_timer.GetLerp(), 0.5f);

            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(col, 0.0f), new GradientColorKey(col, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
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
            Gradient gradient = new Gradient();

            Color col = new Color(1.0f, m_timer.GetLerp(), m_timer.GetLerp(), 0.0f);

            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(col, 0.0f), new GradientColorKey(col, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
                );

            m_line.colorGradient = gradient;
        }
    }
}
