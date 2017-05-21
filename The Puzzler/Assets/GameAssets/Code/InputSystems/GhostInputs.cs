using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInputs : PlayerInputs
{
    public const int m_recordingSize = 60 * 4;

    private char m_Inputs;
    private char[] m_recordedInputs = new char[m_recordingSize];
    public int m_arrayPosition = 0;
    public bool m_recorded = false;
    public bool m_recording = false;
    public bool m_playing = false;

    public Vector3 m_startingPosition;

    private void Update()
    {
        if (m_recording)
        {
            if (m_arrayPosition < m_recordingSize)
            {
                m_Inputs = (char)0;

                if (m_arrayPosition == 0)
                {
                    m_startingPosition = gameObject.transform.position;
                }

                if (Input.GetAxisRaw("Horizontal") > 0.0f)
                {
                    m_Inputs |= (char)InputToBit(E_INPUTS.LEFT);
                }

                if (Input.GetAxisRaw("Horizontal") < 0.0f)
                {
                    m_Inputs |= (char)InputToBit(E_INPUTS.RIGHT);
                }

                if (Input.GetAxisRaw("Vertical") > 0.0f)
                {
                    m_Inputs |= (char)InputToBit(E_INPUTS.UP);
                }

                if (Input.GetAxisRaw("Vertical") < 0.0f)
                {
                    m_Inputs |= (char)InputToBit(E_INPUTS.DOWN);
                }

                if (Input.GetButton("Jump"))
                {
                    m_Inputs |= (char)InputToBit(E_INPUTS.JUMP);
                }

                if (Input.GetButton("MoveBox"))
                {
                    m_Inputs |= (char)InputToBit(E_INPUTS.MOVE_BOX);
                }

                if (Input.GetButtonDown("PressButton"))
                {
                    m_Inputs |= (char)InputToBit(E_INPUTS.PRESS_BUTTON);
                }

                m_recordedInputs[m_arrayPosition] = m_Inputs;
                m_arrayPosition++;
            }
            else
            {
                m_recorded = true;
                m_recording = false;

                gameObject.transform.position = m_startingPosition;
            }
        }
        else if (m_playing)
        {
            if (m_arrayPosition < m_recordingSize)
            {
                m_Inputs = m_recordedInputs[m_arrayPosition];
                m_arrayPosition++;
            }
            else
            {
                m_playing = false;

                gameObject.transform.position = m_startingPosition;
            }
        }
        else
        {
            m_Inputs = (char)0;
        }
    }

    public IEnumerator Play()
    {
        if (m_arrayPosition > m_recordingSize)
        {
            m_Inputs = m_recordedInputs[m_arrayPosition];
            m_arrayPosition++;
            yield return new WaitForSeconds(0.016f);
        }
        
        yield return new WaitForSeconds(0.016f);
    }

    public override bool GetInput(E_INPUTS input)
    {
        return (m_Inputs & (char)InputToBit(input)) > 0;
    }

    protected override int InputToBit(E_INPUTS input)
    {
        int bit = 1;

        for (int z = 0; z < (int)input; z++)
        {
            bit *= 2;
        }

        return bit;
    }
}
