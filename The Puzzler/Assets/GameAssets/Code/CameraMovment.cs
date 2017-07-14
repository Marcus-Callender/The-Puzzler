﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_CamType
{
    CAM_2D,
    CAM_3D_GROUND,
    CAM_3D_AIR,
    CAM_3D_MOVING_BOX

}

public class CameraMovment : MonoBehaviour
{
    // I am using the player data as this will be useful for when I implement zooming the camera out as a button
    //private PlayerData m_player;

    //float m_cameraSpeed = 6.5f;

    private PlayerStateMachine m_player;
    private E_CamType m_currentCam;
    private E_CamType m_nextCam;
    private Timer m_transitionTimer;

    void Start()
    {
        m_player = FindObjectOfType<PlayerStateMachine>();

        Vector3 playerPos = m_player.gameObject.transform.position;

        gameObject.transform.position = new Vector3(playerPos.x, playerPos.y + 3.0f, -8.0f);

        m_transitionTimer = new Timer();
        m_transitionTimer.m_time = 0.2f;

        if (m_player.m_data.m_use3D)
        {
            m_currentCam = E_CamType.CAM_3D_AIR;
            m_nextCam = E_CamType.CAM_3D_AIR;
        }
        else
        {
            m_currentCam = E_CamType.CAM_2D;
            m_nextCam = E_CamType.CAM_2D;
        }
    }

    void LateUpdate()
    {
        PlayerData followData = m_player.getFollowData();

        if (!followData.m_use3D)
        {
            m_nextCam = E_CamType.CAM_2D;
        }
        else
        {
            if (followData.m_velocityY != -9.81f)
            {
                m_nextCam = E_CamType.CAM_3D_AIR;
            }
            else if (followData.m_moveingBox)
            {
                m_nextCam = E_CamType.CAM_3D_MOVING_BOX;
            }
            else
            {
                m_nextCam = E_CamType.CAM_3D_GROUND;
            }
        }

        m_transitionTimer.Cycle();

        if (m_currentCam != m_nextCam)
        {
            if (!m_transitionTimer.m_playing)
            {
                m_transitionTimer.Play();
            }

            gameObject.transform.rotation = Quaternion.Slerp(getNewRot(m_currentCam, followData), getNewRot(m_nextCam, followData), m_transitionTimer.GetLerp());
            gameObject.transform.position = Vector3.Lerp(getNewPos(m_currentCam, followData), getNewPos(m_nextCam, followData), m_transitionTimer.GetLerp());

            if (m_transitionTimer.m_completed)
            {
                m_currentCam = m_nextCam;
                m_transitionTimer.Stop();
            }

        }
        else
        {
            m_transitionTimer.Stop();

            gameObject.transform.rotation = getNewRot(m_currentCam, followData);
            gameObject.transform.position = getNewPos(m_currentCam, followData);
        }
    }

    void Transition(PlayerData data)
    {
        Vector3 playerPos = data.transform.position;
        Quaternion playerRot = data.transform.rotation;
        bool playerLeftRight = data.m_left_right;

        float dt = Time.deltaTime * 5.0f;

        Vector3 newPos = playerPos + new Vector3(0.0f, 2.0f, -8.0f);
        Quaternion newRot = playerRot;
        newRot *= Quaternion.Euler(Vector3.up * (playerLeftRight ? -90.0f : 90.0f));

        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, dt);
        transform.position = Vector3.Lerp(transform.position, newPos, dt);
    }

    private Vector3 Camera2DPos(PlayerData data)
    {
        Vector3 pos = new Vector3();

        pos = data.transform.position;

        pos += transform.up * 2.0f;
        pos += transform.forward * -8.0f;

        //float yPosLerp = Mathf.Lerp(gameObject.transform.position.y, data.playerPos.y + 3.0f, Time.deltaTime * 3.0f);
        //
        //Vector3 pos = data.playerPos;
        //pos.y = yPosLerp;
        //
        //if (Mathf.Abs(data.playerPos.x - gameObject.transform.position.x) > 1.5f)
        //{
        //    if (data.playerPos.x > gameObject.transform.position.x)
        //    {
        //        pos += 14.0f * Time.deltaTime * gameObject.transform.right;
        //    }
        //    else
        //    {
        //        pos -= 14.0f * Time.deltaTime * gameObject.transform.right;
        //    }
        //}
        //else if (data.playerPos.x > gameObject.transform.position.x + 1.0f)
        //{
        //    pos = (data.playerPos.x - 1.0f) * gameObject.transform.right;
        //}
        //else if (data.playerPos.x < gameObject.transform.position.x - 1.0f)
        //{
        //    pos = (data.playerPos.x + 1.0f) * gameObject.transform.right;
        //}
        //
        //gameObject.transform.position = pos;

        return pos;
    }

    private Quaternion Camera2DRot(PlayerData data)
    {
        Quaternion rot = new Quaternion();

        rot = data.transform.rotation;

        rot *= Quaternion.Euler(Vector3.up * (data.m_left_right ? -90.0f : 90.0f));

        return rot;
    }

    private Vector3 Camera3DGroundPos(PlayerData data)
    {
        Vector3 pos = new Vector3();

        pos = data.transform.position;

        pos += transform.right * 0.5f;
        pos += transform.up * 1.2f;
        pos += transform.forward * -3.0f;

        return pos;
    }

    private Quaternion Camera3DGroundRot(PlayerData data)
    {
        Quaternion rot = new Quaternion();

        rot = data.transform.rotation;

        return rot;
    }

    private Vector3 Camera3DAirPos(PlayerData data)
    {
        Vector3 pos = new Vector3();

        pos = data.transform.position;

        gameObject.transform.position = data.transform.position;

        pos += transform.right * 0.5f;
        pos += transform.up * 1.2f;
        pos += transform.forward * -3.0f;

        return pos;
    }

    private Quaternion Camera3DAirRot(PlayerData data)
    {
        Quaternion rot = new Quaternion();

        rot = data.transform.rotation;

        rot *= Quaternion.Euler(30.0f, 0.0f, 0.0f);

        return rot;
    }

    private Vector3 Camera3DMoveingBoxPos(PlayerData data)
    {
        Vector3 pos = new Vector3();

        pos = data.transform.position;

        gameObject.transform.position = data.transform.position;

        pos += transform.right * 0.5f;
        pos += transform.up * 2.2f;
        pos += transform.forward * -4.5f;

        return pos;
    }

    private Quaternion Camera3DMovingBoxRot(PlayerData data)
    {
        Quaternion rot = new Quaternion();

        rot = data.transform.rotation;

        rot *= Quaternion.Euler(50.0f, 0.0f, 0.0f);

        return rot;
    }

    private Vector3 getNewPos(E_CamType type, PlayerData data)
    {
        if (type == E_CamType.CAM_3D_AIR)
        {
            return Camera3DAirPos(data);
        }
        else if (type == E_CamType.CAM_3D_GROUND)
        {
            return Camera3DGroundPos(data);
        }
        else if (type == E_CamType.CAM_3D_MOVING_BOX)
        {
            return Camera3DMoveingBoxPos(data);
        }

        return Camera2DPos(data);
    }

    private Quaternion getNewRot(E_CamType type, PlayerData data)
    {
        if (type == E_CamType.CAM_3D_AIR)
        {
            return Camera3DAirRot(data);
        }
        else if (type == E_CamType.CAM_3D_GROUND)
        {
            return Camera3DGroundRot(data);
        }
        else if (type == E_CamType.CAM_3D_MOVING_BOX)
        {
            return Camera3DMovingBoxRot(data);
        }

        return Camera2DRot(data);
    }
}
