using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    private Rigidbody m_rigb;

    // I am using the player data as this will be useful for when I implement zooming the camera out as a button
    private PlayerData m_player;

    float m_cameraSpeed = 6.5f;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();

        m_player = FindObjectOfType<PlayerData>();

        Vector3 playerPos = m_player.gameObject.transform.position;

        gameObject.transform.position = new Vector3(playerPos.x, playerPos.y + 3.0f, -10.0f);
    }
    
    void LateUpdate()
    {
        Vector3 playerPos = m_player.gameObject.transform.position;

        float yPosLerp = Mathf.Lerp(gameObject.transform.position.y, playerPos.y + 3.0f, Time.deltaTime * 3.0f);

        Vector3 pos = new Vector3(transform.position.x, yPosLerp, -10.0f);

        if (playerPos.x > gameObject.transform.position.x + 1.0f)
        {
            pos.x = playerPos.x - 1.0f;
        }
        else if (playerPos.x < gameObject.transform.position.x - 1.0f)
        {
            pos.x = playerPos.x + 1.0f;
        }

        gameObject.transform.position = pos;
    }
}
