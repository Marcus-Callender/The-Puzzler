using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerData[] m_players;
    private Rigidbody m_rigb;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();
        m_players = FindObjectsOfType<PlayerData>();
    }

    void Update()
    {
        m_rigb.velocity = new Vector3(0.0f, 0.0f);

        Debug.Log("Searching for player");

        for (int z = 0; z < m_players.Length; z++)
        {
            float distance = Mathf.Abs(gameObject.transform.position.x - m_players[z].transform.position.x) + Mathf.Abs(gameObject.transform.position.y - m_players[z].transform.position.y);

            if (m_players[z].tag == "Player" && distance < 5.0f && distance > 2.0f)
            {
                Vector3 fwd = transform.TransformDirection(Vector3.forward);

                Debug.DrawRay(transform.position, m_players[z].transform.position - transform.position, Color.green);

                RaycastHit hitData;

                if (!Physics.Raycast(transform.position, m_players[z].transform.position - transform.position, out hitData))
                {
                    Debug.Log("Player hidden");
                }
                else
                {
                    if (hitData.transform == m_players[z].transform)
                    {
                        Debug.Log("Player found");

                        if (gameObject.transform.position.x > m_players[z].transform.position.x)
                        {
                            m_rigb.velocity = new Vector3(-3.0f, 0.0f);
                        }
                        else
                        {
                            m_rigb.velocity = new Vector3(3.0f, 0.0f);
                        }
                    }
                    else
                    {
                        Debug.Log("Error");
                    }
                }
            }
        }
    }
}
