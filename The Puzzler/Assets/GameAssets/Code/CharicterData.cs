using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CHARICTER_STATES
{
    STAND,
    WALK,
    JUMP,

    NUMBER_OF_STATES,
    NULL
}

public class CharicterData : MonoBehaviour
{
    public Rigidbody m_rigb;

    void Start()
    {
        m_rigb = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }
}
