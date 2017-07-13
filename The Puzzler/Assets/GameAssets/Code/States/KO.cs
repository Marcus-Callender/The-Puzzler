using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class KO : BasicState
{
    private Timer m_KOTime;

    public override void Initialize(Rigidbody rigb, PlayerData data, PlayerInputs inputs)
    {
        m_rigb = rigb;
        m_data = data;
        m_inputs = inputs;

        m_KOTime = new Timer();
        m_KOTime.m_time = 1.0f;
    }

    public override void Enter()
    {
        m_KOTime.Play();
    }

    public override E_PLAYER_STATES Cycle()
    {
        m_data.m_velocityX = 0.0f;
        m_data.m_velocityY = 0.0f;

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
