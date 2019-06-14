using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingBox : BasicState
{
    private float m_dragSpeed = 3.5f;

    public GameObject m_box;
    public Rigidbody m_boxRigb;

    public override void Initialize(Rigidbody rigb, PlayerData data)
    {
        base.Initialize(rigb, data);
    }

    public override void Enter()
    {
        m_data.m_moveingBox = true;
        m_data.m_stopRotation = true;

        RaycastHit hit;
        Physics.Raycast(m_data.GetCenterTransform(), transform.forward, out hit);
        Debug.DrawRay(m_data.GetCenterTransform(), transform.forward, Color.red, 3.0f);

        if (hit.transform && hit.transform.tag == "Box")
        {
            m_box = hit.transform.gameObject;
            m_boxRigb = m_box.GetComponent<Rigidbody>();

            transform.forward = -hit.normal;
        }

        if (!m_box)
        {
            Debug.Log("Box was not found");
        }
    }

    public override IGhostInteractable GhostSpecialEnter()
    {
        if (m_box != null)
        {
            IGhostInteractable interaction = m_box.GetComponent<IGhostInteractable>();

            if (interaction == null)
            {
                Debug.Log("intection could not be found");
            }
            else
            {
                interaction.StartIntecation();
                return interaction;
            }
        }

        return null;
    }

    public override void Exit()
    {
        if (m_box)
        {
            m_box = null;
        }

        if (m_boxRigb)
        {
            m_boxRigb.velocity = new Vector3(0.0f, -9.81f, 0.0f);
            m_boxRigb = null;
        }

        m_data.m_moveingBox = false;
        m_data.m_stopRotation = false;
    }

    public override IGhostInteractable GhostSpecialExit()
    {
        if (m_box != null)
        {
            IGhostInteractable interaction = m_box.GetComponent<IGhostInteractable>();

            if (interaction == null)
            {
                Debug.Log("intection could not be found");
            }
            else
            {
                interaction.StopIntecation();
                return interaction;
            }
        }

        return null;
    }

    public override E_PLAYER_STATES Cycle(S_inputStruct inputs)
    {
        if (!m_box)
        {
            Debug.Log("No box refrence");

            return E_PLAYER_STATES.ON_GROUND;
        }

        bool getInput = false;

        if (m_data.m_use3D)
        {
            getInput = inputs.m_movementVector.x != 0.0f || inputs.m_movementVector.y != 0.0f;
        }
        else
        {
            getInput = inputs.m_movementVector.x != 0.0f;
        }

        if (m_data.m_use3D)
        {
            m_data.m_velocity.x += m_dragSpeed * inputs.m_movementVector.x;
            m_data.m_velocity.z += m_dragSpeed * inputs.m_movementVector.y;
        }
        else
        {
            MoveHorzontal(m_dragSpeed, inputs);
        }

        m_boxRigb.velocity = m_data.GetExpectedVelocity();

        if (!GetInput(E_INPUTS.MOVE_BOX_HOLD, inputs))
        {
            Debug.Log("Box movment button relesed");
            return E_PLAYER_STATES.ON_GROUND;
        }

        return E_PLAYER_STATES.MOVEING_BLOCK;
    }


    public override E_PLAYER_STATES Colide(E_DIRECTIONS _dir, string _tag)
    {
        base.Colide(_dir, _tag);

        if (_tag != "Box" && (_dir == E_DIRECTIONS.LEFT || _dir == E_DIRECTIONS.RIGHT))
        {
            return E_PLAYER_STATES.ON_GROUND;
        }

        return E_PLAYER_STATES.MOVEING_BLOCK;
    }

    public override E_PLAYER_STATES LeaveColision(string _tag)
    {
        return base.LeaveColision(_tag);
    }
}
