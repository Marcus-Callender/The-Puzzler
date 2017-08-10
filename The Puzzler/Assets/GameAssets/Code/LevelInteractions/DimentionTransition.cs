﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimentionTransition : MonoBehaviour
{
    public bool m_camera_left_right = false;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ghost")
        {
            PlayerData data = other.gameObject.GetComponent<PlayerData>();

            Physics.Raycast(new Ray(transform.position, other.transform.position - transform.position));

            Debug.DrawRay(transform.position, other.transform.position - transform.position, Color.blue);
            Debug.DrawRay(transform.position, transform.right, Color.green);

            if (Vector3.Angle(transform.right, other.transform.position - transform.position) < 90.0f)
            {
                // if the player is transtioning from 3D to 2D this aligns the player to the position of the transition
                // so they are garenteed to be able to collect powerups/use ladders ect
                if (data.m_use3D)
                {
                    Vector3 newPos = Vector3.zero;

                    newPos = transform.position;
                    newPos.y = data.transform.position.y;
                    newPos += -transform.forward * (transform.localScale.x * 2.0f);

                    data.transform.position = newPos;
                }

                data.m_use3D = false;
                data.m_left_right = m_camera_left_right;
                // alignes the player with the dimention transitions rotation so the player can't unintentionaly walk off a platrorm
                data.SetRotation(gameObject.transform.rotation);
            }
            else
            {
                // position or rotation adjustemts aren't needed for 2D to 3D transitions
                data.m_use3D = true;
            }
        }
    }
}