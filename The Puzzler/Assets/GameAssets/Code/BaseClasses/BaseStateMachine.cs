using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class BaseStateMachine : MonoBehaviour
{
    protected BasicState[] m_states2D = new BasicState[8];
    protected BasicState[] m_states3D = new BasicState[8];
    public PlayerData m_data;
    protected Rigidbody m_rigb;
    protected bool m_lockState = false;

    [SerializeField]
    protected E_PLAYER_STATES m_currentState = E_PLAYER_STATES.IN_AIR;
    protected E_PLAYER_STATES m_newState = E_PLAYER_STATES.IN_AIR;

    protected S_inputStruct m_inputs;

    public virtual void Initialize()
    {
        m_data = GetComponent<PlayerData>();
        m_rigb = GetComponent<Rigidbody>();

        m_data.Initialize();
    }

    public virtual void Cycle()
    {
        m_newState = GetCurrentState().Cycle(m_inputs);
        CheckState();
    }

    public void FixedCycle()
    {
        m_newState = GetCurrentState().PhysCycle(m_inputs);
        CheckState();

        for (int z = 0; z < 4; z++)
        {
            m_data.m_contacts[z] = false;
            m_data.m_InteractableContacts[z] = false;
        }
    }

    public void GetInputs(S_inputStruct inputs)
    {
        if (m_data.m_preloadedInputs.Count > 0)
        {
            m_inputs = m_data.m_preloadedInputs[0];
            m_data.m_preloadedInputs.RemoveAt(0);
        }
        else
        {
            m_inputs = inputs;
        }

        /*if (m_data.m_preloadedJoystickMovements.Count > 0)
        {
            m_JoystickMovement = m_data.m_preloadedJoystickMovements[0];
            m_data.m_preloadedJoystickMovements.RemoveAt(0);
        }
        else
        {
            m_JoystickMovement = stickMovements;
        }*/
    }

    public virtual void OnCollisionStay(Collision Other)
    {
        float angle = Vector2.Angle(Other.contacts[0].normal, Vector2.up);

        E_DIRECTIONS dir = E_DIRECTIONS.TOP;

        if (Mathf.Approximately(angle, 0.0f))
        {
            dir = E_DIRECTIONS.BOTTOM;

            Rigidbody rigb = Other.gameObject.GetComponent<Rigidbody>();

            if (!rigb && gameObject.transform.parent)
            {
                rigb = gameObject.transform.parent.GetComponent<Rigidbody>();
            }

            if (rigb)
            {
                m_data.m_velocityY += rigb.velocity.y;

                if (m_data.m_use3D)
                {
                    Vector3 platformVelToLocalVel = transform.InverseTransformDirection(rigb.velocity);
                    m_data.m_velocityX += platformVelToLocalVel.z;
                    m_data.m_velocityZ += platformVelToLocalVel.x;
                }
                else
                {
                    m_data.m_velocityX += rigb.velocity.x;
                    m_data.m_velocityZ += rigb.velocity.z;
                }
            }
        }
        else if (Mathf.Approximately(angle, 180.0f))
        {
            dir = E_DIRECTIONS.TOP;
        }

        m_newState = GetCurrentState().Colide(dir, Other.gameObject.tag);
        CheckState();
    }

    void OnCollisionExit(Collision Other)
    {
        m_newState = GetCurrentState().LeaveColision(Other.gameObject.tag);
        CheckState();
    }

    public virtual void OnTriggerStay(Collider other)
    {
        m_newState = GetCurrentState().InTrigger(other.gameObject.tag, m_inputs);
        CheckState();
    }

    void OnTriggerExit(Collider other)
    {
        m_newState = GetCurrentState().LeaveTrigger(other.gameObject.tag);
        CheckState();
    }

    protected virtual void CheckState()
    {
        // prevents the state from changing
        if (!m_lockState)
        {
            m_data.m_anim.SetBool("KOd", m_data.m_squished);

            if (m_data.m_squished)
            {
                m_newState = E_PLAYER_STATES.KO;
            }

            if (m_newState != E_PLAYER_STATES.NULL && m_newState != m_currentState && m_states2D[(int)m_newState])
            {
                // tells the old state is is being left and the new state is being entered
                GetCurrentState().Exit();
                m_states2D[(int)m_newState].Enter();

                // shows the state transition that took place
                Debug.Log(m_currentState + " -> " + m_newState);

                // sets the new state to be used
                m_currentState = m_newState;
            }
        }
    }

    protected BasicState GetCurrentState()
    {
        if (!m_data)
        {
            Debug.LogError("--- Data refrence not found");
        }

        if (!m_states3D[(int)m_currentState] || !m_states2D[(int)m_currentState])
        {
            Debug.LogError("--- State refrence not found");
        }

        if (m_data.m_use3D)
        {
            return m_states3D[(int)m_currentState];
        }

        return m_states2D[(int)m_currentState];
    }

    protected BasicState GetNewState()
    {
        if (m_data.m_use3D)
        {
            return m_states3D[(int)m_newState];
        }

        return m_states2D[(int)m_newState];
    }

    public virtual void Pause(bool paused)
    {
        m_lockState = paused;
        m_data.m_anim.SetBool("Paused", paused);
    }

    public virtual bool GetInput(E_INPUTS input)
    {
        return (m_inputs.m_buttons & (char)InputToBit(input)) > 0;
    }

    protected virtual int InputToBit(E_INPUTS input)
    {
        int bit = 1;

        for (int z = 0; z < (int)input; z++)
        {
            bit *= 2;
        }

        return bit;
    }
}
