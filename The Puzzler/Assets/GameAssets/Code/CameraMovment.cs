using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_CamType
{
    CAM_2D,
    CAM_3D_GROUND,
    CAM_3D_AIR
}

public struct S_CAM_DATA
{
    public Vector3 playerPos;
    public Quaternion playerRot;
    public bool playerLeftRight;
    public bool use3D;
}

public class CameraMovment : MonoBehaviour
{
    //private Rigidbody m_rigb;

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
        //Vector3 playerPos = m_player.getFollowPos();
        //Quaternion playerRot = m_player.getFollowRot();
        //bool playerLeftRight = m_player.getFollowLeftRight();
        //bool use3D = m_player.getFollow3D();

        S_CAM_DATA camData = new S_CAM_DATA();
        camData.playerPos = m_player.getFollowPos();
        camData.playerRot = m_player.getFollowRot();
        camData.playerLeftRight = m_player.getFollowLeftRight();
        camData.use3D = m_player.getFollow3D();

        if (!camData.use3D)
        {
            /*float yPosLerp = Mathf.Lerp(gameObject.transform.position.y, playerPos.y + 3.0f, Time.deltaTime * 3.0f);

            Vector3 pos = new Vector3(transform.position.x, yPosLerp, -8.0f);

            if (Mathf.Abs(playerPos.x - gameObject.transform.position.x) > 1.5f)
            {
                if (playerPos.x > gameObject.transform.position.x)
                {
                    pos.x += 14.0f * Time.deltaTime;
                }
                else
                {
                    pos.x -= 14.0f * Time.deltaTime;
                }
            }
            else if (playerPos.x > gameObject.transform.position.x + 1.0f)
            {
                pos.x = playerPos.x - 1.0f;
            }
            else if (playerPos.x < gameObject.transform.position.x - 1.0f)
            {
                pos.x = playerPos.x + 1.0f;
            }

            gameObject.transform.position = pos;*/


            /*gameObject.transform.position = camData.playerPos;
            gameObject.transform.rotation = camData.playerRot;

            gameObject.transform.Rotate(Vector3.up, (camData.playerLeftRight ? -90.0f : 90.0f));
            gameObject.transform.Translate(new Vector3(0.0f, 2.0f, -8.0f));*/

            //gameObject.transform.rotation = Camera2DRot(camData);
            //gameObject.transform.position = Camera2DPos(camData);

            m_nextCam = E_CamType.CAM_2D;
        }
        else
        {
            if (m_player.m_data.m_velocityY != -9.81f)
            {
                /*//playerPos.y += 1.0f;
                //playerRot *= Quaternion.Euler(45.0f, 0.0f, 0.0f);
                camData.playerRot *= Quaternion.Euler(30.0f, 0.0f, 0.0f);

                gameObject.transform.position = camData.playerPos;
                gameObject.transform.rotation = camData.playerRot;

                //gameObject.transform.Translate(new Vector3(-3.0f, 1.2f, 0.5f));
                gameObject.transform.Translate(new Vector3(0.5f, 1.2f, -3.0f));*/

                //gameObject.transform.rotation = Camera3DAirRot(camData);
                //gameObject.transform.position = Camera3DAirPos(camData);

                m_nextCam = E_CamType.CAM_3D_AIR;


            }
            else
            {
                /*gameObject.transform.position = camData.playerPos;
                gameObject.transform.rotation = camData.playerRot;

                //gameObject.transform.Translate(new Vector3(-3.0f, 1.2f, 0.5f));
                gameObject.transform.Translate(new Vector3(0.5f, 1.2f, -3.0f));*/

                //gameObject.transform.rotation = Camera3DGroundRot(camData);
                //gameObject.transform.position = Camera3DGroundPos(camData);

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

            gameObject.transform.rotation = Quaternion.Slerp(getNewRot(m_currentCam, camData), getNewRot(m_nextCam, camData), m_transitionTimer.GetLerp());
            gameObject.transform.position = Vector3.Lerp(getNewPos(m_currentCam, camData), getNewPos(m_nextCam, camData), m_transitionTimer.GetLerp());

            if (m_transitionTimer.m_completed)
            {
                m_currentCam = m_nextCam;
                m_transitionTimer.Stop();
            }

        }
        else
        {
            m_transitionTimer.Stop();

            gameObject.transform.rotation = getNewRot(m_currentCam, camData);
            gameObject.transform.position = getNewPos(m_currentCam, camData);
        }
    }

    void Transition()
    {
        Vector3 playerPos = m_player.getFollowPos();
        Quaternion playerRot = m_player.getFollowRot();
        bool playerLeftRight = m_player.getFollowLeftRight();

        float dt = Time.deltaTime * 5.0f;

        Vector3 newPos = playerPos + new Vector3(0.0f, 2.0f, -8.0f);
        Quaternion newRot = playerRot;
        newRot *= Quaternion.Euler(Vector3.up * (playerLeftRight ? -90.0f : 90.0f));

        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, dt);
        transform.position = Vector3.Lerp(transform.position, newPos, dt);
    }

    private Vector3 Camera2DPos( S_CAM_DATA data)
    {
        Vector3 pos = new Vector3();

        pos = data.playerPos;

        //pos += new Vector3(0.0f, 2.0f, -8.0f);
        pos += transform.up * 2.0f;
        pos += transform.forward * -8.0f;

        return pos;
    }

    private Quaternion Camera2DRot(S_CAM_DATA data)
    {
        Quaternion rot = new Quaternion();

        rot = data.playerRot;

        rot *= Quaternion.Euler(Vector3.up * (data.playerLeftRight ? -90.0f : 90.0f));
        //rot *= Quaternion.Euler(0.0f, (data.playerLeftRight ? -45.0f : 45.0f), 0.0f);

        return rot;
    }

    private Vector3 Camera3DGroundPos(S_CAM_DATA data)
    {
        Vector3 pos = new Vector3();

        pos = data.playerPos;

        pos += transform.right * 0.5f;
        pos += transform.up * 1.2f;
        pos += transform.forward * -3.0f;

        return pos;
    }

    private Quaternion Camera3DGroundRot(S_CAM_DATA data)
    {
        Quaternion rot = new Quaternion();

        rot = data.playerRot;
        
        return rot;
    }

    private Vector3 Camera3DAirPos(S_CAM_DATA data)
    {
        Vector3 pos = new Vector3();

        pos = data.playerPos;
        
        gameObject.transform.position = data.playerPos;

        pos += transform.right * 0.5f;
        pos += transform.up * 1.2f;
        pos += transform.forward * -3.0f;

        return pos;
    }

    private Quaternion Camera3DAirRot(S_CAM_DATA data)
    {
        Quaternion rot = new Quaternion();

        rot = data.playerRot;

        rot *= Quaternion.Euler(30.0f, 0.0f, 0.0f);

        return rot;
    }

    private Vector3 getNewPos(E_CamType type, S_CAM_DATA data)
    {
        if (type == E_CamType.CAM_3D_AIR)
        {
            return Camera3DAirPos(data);
        }
        else if (type == E_CamType.CAM_3D_GROUND)
        {
            return Camera3DGroundPos(data);
        }

        return Camera2DPos(data);
    }

    private Quaternion getNewRot(E_CamType type, S_CAM_DATA data)
    {
        if (type == E_CamType.CAM_3D_AIR)
        {
            return Camera3DAirRot(data);
        }
        else if (type == E_CamType.CAM_3D_GROUND)
        {
            return Camera3DGroundRot(data);
        }

        return Camera2DRot(data);
    }
}
