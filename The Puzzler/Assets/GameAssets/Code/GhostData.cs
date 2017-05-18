using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_INPUTS
{
    LEFT,
    RIGHT,
    JUMP,
    //MOVE_BOX,
    //PRESS_BUTTON,

    NULL
}

public class GhostData : MonoBehaviour
{
    private char[] m_recordedInputs = new char[60 * 3];

    void Start()
    {

    }
    
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.0f)
        {

        }

        if (Input.GetAxisRaw("Horizontal") < 0.0f)
        {

        }

        if (Input.GetButton("Jump"))
        {

        }
    }

    int InputToBit(E_INPUTS input)
    {
        int bit = 1;

        for (int z = 0; z < (int)input; z++)
        {
            bit *= 2;
        }

        return bit;
    }
}
