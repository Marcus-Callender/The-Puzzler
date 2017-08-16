using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyBaseState
{
    public GameObject m_attack;

    private float m_attackDelay = 0.2f;
    private float m_attackActive = 0.6f;
    private float m_attackRecovery = 0.8f;

    private Timer m_attackDelayTimer;
    private Timer m_attackActiveTimer;
    private Timer m_attackRecoveryTimer;

    void Start()
    {
        Transform[] refrences = gameObject.GetComponentsInChildren<Transform>();

        for (int z = 0; z < refrences.Length; z++)
        {
            if (refrences[z].tag == "Attack")
            {
                m_attack = refrences[z].gameObject;
            }
        }

        if (m_attack == null)
        {
            Debug.Log("-----Error attack refrence not found!------");
        }

        m_attack.SetActive(false);

        m_attackDelayTimer = new Timer();
        m_attackActiveTimer = new Timer();
        m_attackRecoveryTimer = new Timer();

        m_attackDelayTimer.m_time = m_attackDelay;
        m_attackActiveTimer.m_time = m_attackActive;
        m_attackRecoveryTimer.m_time = m_attackRecovery;
    }

    public override E_ENEMY_STATES Cycle()
    {
        m_attack.transform.Rotate(0.0f, 30.0f, 0.0f);

        m_attackDelayTimer.Cycle();

        if (m_attackDelayTimer.m_completed)
        {
            m_attackActiveTimer.Cycle();
        }
        else if (m_attackActiveTimer.m_completed)
        {
            m_attackRecoveryTimer.Cycle();
        }
        else if (m_attackRecoveryTimer.m_completed)
        {
            return E_ENEMY_STATES.AIR;
        }

        return E_ENEMY_STATES.NULL;
    }

    public override E_ENEMY_STATES EnterState()
    {
        m_attack.SetActive(true);

        m_attackDelayTimer.Play();
        m_attackActiveTimer.Play();
        m_attackRecoveryTimer.Play();

        return E_ENEMY_STATES.NULL;
    }

    public override E_ENEMY_STATES ExitState()
    {
        m_attack.SetActive(false);

        return E_ENEMY_STATES.NULL;
    }
}
