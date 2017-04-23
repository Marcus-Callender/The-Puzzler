﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // how fast the charater moves left to right
    float m_speed = 10.0f;
    float m_jump = 5.0f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0.5f)
        {
            // gets a refrence to the position
            Vector3 pos = gameObject.transform.position;
            // adds to the position
            pos.x += m_speed * Time.deltaTime;
            // applies the new position
            gameObject.transform.position = pos;
        }
        else if (Input.GetAxis("Horizontal") < -0.5f)
        {
            Vector3 pos = gameObject.transform.position;
            pos.x -= m_speed * Time.deltaTime;
            gameObject.transform.position = pos;
        }
        if (Input.GetAxis("Vertical") > 0.5f)
        {
            // gets a refrence to the position
            Vector3 pos = gameObject.transform.position;
            // adds to the position
            pos.y += m_jump * Time.deltaTime;
            // applies the new position
            gameObject.transform.position = pos;
        }
        else if (Input.GetAxis("Vertical") < -0.5f)
        {
            Vector3 pos = gameObject.transform.position;
            pos.y -= m_jump * Time.deltaTime;
            gameObject.transform.position = pos;
        }
    }
}


