using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostData : MonoBehaviour
{
    private char[] m_recordedInputs = new char[60 * 3];
    private int m_inputCount = 0;
    private Vector3 m_startPoint;
    private bool m_recording = false;

    public void StartRecording()
    {
        if (!m_recording)
        {
            m_startPoint = gameObject.transform.position;
        }

        m_recording = true;
    }

    void PlayRecording()
    {

    }

    void Update()
    {
        if (m_recording)
        {
            m_recordedInputs[m_inputCount] = (char)0;

            if (Input.GetAxisRaw("Horizontal") > 0.0f)
            {
                m_recordedInputs[m_inputCount] = (char)((int)m_recordedInputs[m_inputCount] | InputToBit(E_INPUTS.LEFT));
            }

            if (Input.GetAxisRaw("Horizontal") < 0.0f)
            {
                m_recordedInputs[m_inputCount] = (char)((int)m_recordedInputs[m_inputCount] | InputToBit(E_INPUTS.RIGHT));
            }

            if (Input.GetButton("Jump"))
            {
                m_recordedInputs[m_inputCount] = (char)((int)m_recordedInputs[m_inputCount] | InputToBit(E_INPUTS.JUMP));
            }

            m_inputCount++;

            if (m_inputCount == (60 * 3) - 1)
            {
                m_recording = false;
            }
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
