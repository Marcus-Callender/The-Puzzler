using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTransition : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_newGravity;

    private Timer m_timer;

    // Start is called before the first frame update
    void Start()
    {
        m_timer = new Timer();
        m_timer.m_time = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timer.m_playing)
        {
            m_timer.Cycle();

            if (m_timer.m_completed)
            {
                //PauseMenu.m_instance.Pause(false, false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered");

        if (other.gameObject.name == "Player")
        {
            PlayerData data = other.gameObject.GetComponent<PlayerData>();

            if (data)
            {
                //data.m_gravity = m_newGravity;
                Quaternion rot = data.gameObject.transform.rotation;
                rot *= Quaternion.Euler(0, 90, 0);
                data.SetRotation(rot);
                //PauseMenu.m_instance.Pause(true, true);

                m_timer.Play();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other)
        {
            Debug.Log("staying");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other)
        {
            Debug.Log("exit");
        }
    }
}
