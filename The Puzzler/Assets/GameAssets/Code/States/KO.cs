using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class KO : BasicState
{
    private Timer m_KOTime;

    public override void Initialize(Rigidbody rigb, PlayerData data)
    {
        m_rigb = rigb;
        m_data = data;

        m_KOTime = new Timer();
        m_KOTime.m_time = 1.0f;
    }

    public override void Enter()
    {
        m_KOTime.Play();
    }

    public override E_PLAYER_STATES Cycle(S_inputStruct inputs)
    {
        m_data.SetVelocity(0.0f, 0.0f, 0.0f);

        m_KOTime.Cycle();

        if (m_KOTime.m_completed)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);

            return E_PLAYER_STATES.IN_AIR;
        }

        return E_PLAYER_STATES.NULL;
    }
}
