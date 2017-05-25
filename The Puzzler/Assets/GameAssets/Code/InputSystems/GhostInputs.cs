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

    public override void Cycle()
    {
        if (m_recording)
        {
            if (m_arrayPosition < m_recordingSize)
            {
                // resets the input
                m_Inputs = (char)0;

                if (m_arrayPosition == 0)
                {
                    // if the recording has just started mark the current position
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
            // stops inputs from carrying over from when the ghost was moving
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
        // chacks if the inputs varible inclues the specified bit
        return (m_Inputs & (char)InputToBit(input)) > 0;
    }

    protected override int InputToBit(E_INPUTS input)
    {
        // function return examples
        // 0 => 1
        // 3 => 8
        // 7 => 128

        int bit = 1;

        for (int z = 0; z < (int)input; z++)
        {
            bit *= 2;
        }

        return bit;
    }

    public void Reset()
    {
        for (int z = 0; z < m_recordingSize; z++)
        {
            m_recordedInputs[z] = (char)0;
        }

        m_arrayPosition = 0;
        m_recorded = false;
        m_recording = false;
        m_playing = false;
    }
}
