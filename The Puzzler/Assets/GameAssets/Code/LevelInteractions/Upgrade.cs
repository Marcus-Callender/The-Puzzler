using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_UPGRADES
{
    MOVE_CRATE,
    GHOST_1,
    GHOST_2,

    SIZE,
    NILL
}

public class Upgrade : MonoBehaviour
{
    public E_UPGRADES m_upgrade;

    public float m_bobHeight = 0.5f;
    public float m_bobCycleTime = 1.0f;
    private Timer m_bobTimer;
    private float m_bobSpeed;

    void Start()
    {
        m_bobTimer = new Timer();
        m_bobTimer.m_time = m_bobCycleTime;
        m_bobTimer.Play();

        m_bobSpeed = m_bobHeight / m_bobCycleTime;
    }

    void Update()
    {
        m_bobTimer.Cycle();

        if (m_bobTimer.m_reversed)
        {
            transform.position += new Vector3(0.0f, m_bobSpeed * Time.deltaTime, 0.0f);

            if (m_bobTimer.m_completed)
            {
                m_bobTimer.Play();
            }
        }
        else
        {
            transform.position -= new Vector3(0.0f, m_bobSpeed * Time.deltaTime, 0.0f);

            if (m_bobTimer.m_completed)
            {
                m_bobTimer.Play(true);
            }
        }

        transform.Rotate(new Vector3(56.0f, 32.0f, 90.0f) * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            PlayerStateMachine data = other.gameObject.GetComponent<PlayerStateMachine>();

            if (data)
            {
                data.Upgrade(m_upgrade);
                gameObject.SetActive(false);
            }
        }
    }
}
