using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingBox : BasicState
{
    private Timer m_pauseTimer;
    private Timer m_dragingTimer;

    private float m_dragSpeed = 3.5f;

    private bool m_moveInput = false;

    private GameObject m_box;
    private Rigidbody m_boxRigb;

    public override void Initialize(Rigidbody rigb, PlayerData data, PlayerInputs inputs)
    {
        base.Initialize(rigb, data, inputs);

        m_pauseTimer = new Timer();
        m_dragingTimer = new Timer();

        m_pauseTimer.m_time = 0.2f;
        m_dragingTimer.m_time = 0.7f;

    }

    public override void Enter()
    {

        m_data.m_velocityX = 0.0f;

        m_moveInput = false;
        m_data.m_stopRotation = true;

        RaycastHit hit;
        Physics.Raycast(m_data.GetCenterTransform(), transform.forward, out hit);
        Debug.DrawRay(m_data.GetCenterTransform(), transform.forward, Color.red);

        if (hit.transform && hit.transform.tag == "Box")
        {
            m_box = hit.transform.gameObject;
            m_boxRigb = m_box.GetComponent<Rigidbody>();

            //m_box.transform.SetParent(gameObject.transform);
            //m_boxRigb.mass = 1;
        }

        if (!m_box)
        {
            Debug.Log("--- error box refrence found was not valid ---");
        }
    }

    public override void Exit()
    {
        // stops the box from continuasly moving then un-links it
        //m_data.m_linkedBox.Move(0.0f);
        //m_data.m_linkedBox = null;

        if (m_box)
        {
            m_box = null;
        }

        if (m_boxRigb)
        {

            m_boxRigb.velocity = new Vector3(0.0f, -9.81f, 0.0f);
            m_boxRigb = null;
        }

        m_pauseTimer.Play();
        m_pauseTimer.m_playing = false;

        m_dragingTimer.Play();
        m_dragingTimer.m_playing = false;

        m_data.m_moveingBox = false;
        m_data.m_stopRotation = false;
    }

    public override E_PLAYER_STATES Cycle()
    {
        if (!m_box)
        {
            Debug.Log("--- no box refrence ---");

            return E_PLAYER_STATES.ON_GROUND;
        }

        bool getInput = false;

        if (m_data.m_use3D)
        {
            getInput = m_inputs.GetInput(E_INPUTS.DOWN) || m_inputs.GetInput(E_INPUTS.UP);
        }
        else
        {
            getInput = m_inputs.GetInput(E_INPUTS.LEFT) || m_inputs.GetInput(E_INPUTS.RIGHT);
        }

        if (getInput && !m_moveInput)
        {
            m_pauseTimer.Play();

            m_dragingTimer.Play();
            m_dragingTimer.m_playing = false;
        }

        m_moveInput = getInput;

        if (getInput)
        {
            if (!m_pauseTimer.m_completed)
            {
                m_pauseTimer.Cycle();
            }
            else if (!m_dragingTimer.m_playing)
            {
                m_dragingTimer.Play();
            }
            else if (!m_dragingTimer.m_completed)
            {
                m_dragingTimer.Cycle();

                if (m_data.m_use3D)
                {
                    if (m_inputs.GetInput(E_INPUTS.UP))
                        m_data.m_velocityX = m_dragSpeed;

                    if (m_inputs.GetInput(E_INPUTS.DOWN))
                        m_data.m_velocityX = -m_dragSpeed;
                }
                else
                {
                    MoveHorzontal(m_dragSpeed);
                }
            }
            else
            {
                m_data.m_velocityX = 0.0f;
                m_pauseTimer.Play();

                m_dragingTimer.m_playing = false;
            }
        }
        else
        {
            m_pauseTimer.Play();
            m_dragingTimer.Play();
        }

        m_boxRigb.velocity = m_data.GetExpectedVelocity();

        if (!m_inputs.GetInput(E_INPUTS.MOVE_BOX_HOLD))
        {
            return E_PLAYER_STATES.ON_GROUND;
        }

        return E_PLAYER_STATES.MOVEING_BLOCK;
    }


    public override E_PLAYER_STATES Colide(E_DIRECTIONS _dir, string _tag)
    {
        base.Colide(_dir, _tag);

        if (_tag != "Box" && (_dir == E_DIRECTIONS.LEFT || _dir == E_DIRECTIONS.RIGHT))
        {
            //m_data.m_linkedBox.m_requestStop = true
            return E_PLAYER_STATES.ON_GROUND;
        }

        return E_PLAYER_STATES.MOVEING_BLOCK;
    }

    public override E_PLAYER_STATES LeaveColision(string _tag)
    {
        return base.LeaveColision(_tag);
    }
}
