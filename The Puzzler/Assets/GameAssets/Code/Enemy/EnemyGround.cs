using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : EnemyBaseState
{
    public float m_partolSpeed = 1.5f;
    public float m_chaseSpeed = 3.0f;

    public float m_partolAcceleration = 1.25f;
    public float m_chaseAleration = 2.5f;

    public override E_ENEMY_STATES Cycle()
    {


        bool faceingLeft = m_data.m_xVelocity < 0.0f;

        Debug.DrawRay(transform.position, new Vector3(faceingLeft ? -1.0f : 1.0f, -0.5f), Color.red);

        if (Physics.Raycast(transform.position, new Vector3(faceingLeft ? -1.0f : 1.0f, -0.5f), 0.7f))
        {
            Debug.Log("Jump up platform");
            m_rigb.velocity = new Vector3(m_data.m_xVelocity, 6.5f);

            return E_ENEMY_STATES.AIR;
        }

        Debug.DrawRay(transform.position, new Vector3(faceingLeft ? -1.0f : 1.0f, -1.5f), Color.yellow);

        if (!Physics.Raycast(transform.position, new Vector3(faceingLeft ? -1.0f : 1.0f, -1.5f)))
        {
            Debug.DrawRay(transform.position + new Vector3(faceingLeft ? -1.0f : 1.0f, -1.5f), new Vector3(faceingLeft ? -2.0f : 2.0f, -0.5f), Color.yellow);

            if (Physics.Raycast(transform.position + new Vector3(faceingLeft ? -1.0f : 1.0f, -1.5f), new Vector3(faceingLeft ? -2.0f : 2.0f, -0.5f)))
            {
                Debug.Log("Jump over gap");
                m_rigb.velocity = new Vector3(m_data.m_xVelocity, 6.5f);

                return E_ENEMY_STATES.AIR;
            }
        }
        
        return E_ENEMY_STATES.NULL;
    }
}
