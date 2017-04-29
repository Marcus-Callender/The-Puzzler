using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    bool m_activated = true;
    Material m_mat;

    public GameObject[] m_linkedObjects;

    void Start()
    {
        m_mat = GetComponent<Renderer>().material;
    }

    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("coliding");

        if (other.tag == "Player")
        {
            Debug.Log("found player");

            PlayerData player = other.GetComponent<PlayerData>();

            if (player)
            {
                Debug.Log("got data");

                if (player.m_pressingButton)
                {
                    Debug.Log("pressed button");
                    m_activated = !m_activated;

                    if (m_activated)
                    {
                        m_mat.color = Color.green;
                    }
                    else
                    {
                        m_mat.color = Color.red;
                    }

                    for (int z = 0; z < m_linkedObjects.Length; z++)
                    {
                        ButtonInteraction script = m_linkedObjects[z].GetComponent<ButtonInteraction>();

                        if (script)
                        {
                            script.OnInteract();
                        }
                    }
                }
            }
        }
    }
}
