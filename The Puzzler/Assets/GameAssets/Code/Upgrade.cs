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

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerStateMachine data = collision.gameObject.GetComponent<PlayerStateMachine>();

            if (data)
            {
                data.Upgrade(m_upgrade);
            }
        }
    }
}
