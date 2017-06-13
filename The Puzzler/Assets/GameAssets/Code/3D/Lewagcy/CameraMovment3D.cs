using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment3D : MonoBehaviour
{
    private PlayerStateMachine m_player;

    void Start()
    {
        m_player = GameObject.FindObjectOfType<PlayerStateMachine>();

        Vector3 playerPos = m_player.gameObject.transform.position;

        gameObject.transform.position = new Vector3(playerPos.x, playerPos.y + 3.0f, -8.0f);
    }

    void LateUpdate()
    {
        /*Vector3 playerPos = m_player.getFollowPos();

        float yPosLerp = Mathf.Lerp(gameObject.transform.position.y, playerPos.y + 3.0f, Time.deltaTime * 3.0f);

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

        /*gameObject.transform.position = m_player.gameObject.transform.position;
        gameObject.transform.rotation = m_player.gameObject.transform.rotation;

        Vector3 position = gameObject.transform.position;
        Quaternion rotation = gameObject.transform.rotation;

        position.x -= 3.0f;
        position.y += 1.2f;
        position.z += 0.5f;

        gameObject.transform.position = position;*/



        /*Transform newTransform = new Transform();
        newTransform.position = m_player.gameObject.transform.position;
        newTransform.position = m_player.gameObject.transform.position;

        newTransform.Translate(new Vector3(-3.0f, 1.2f, 0.5f));

        gameObject.transform.position = newTransform.position;
        gameObject.transform.rotation = newTransform.rotation;*/






        gameObject.transform.position = m_player.gameObject.transform.position;
        gameObject.transform.rotation = m_player.gameObject.transform.rotation;

        //gameObject.transform.Translate(new Vector3(-3.0f, 1.2f, 0.5f));
        gameObject.transform.Translate(new Vector3(0.5f, 1.2f, -3.0f));
    }
}
