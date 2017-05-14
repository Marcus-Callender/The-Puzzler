﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public enum E_PLAYER_STATES
{
    ON_GROUND,
    IN_AIR,

    MOVEING_BLOCK,
    USEING_LADDER,
    SQUISHED,

    DOUBLE_JUMPING,
    WALL_SLIDEING,

    SIZE,
    NULL
}

public enum E_DIRECTIONS
{
    TOP,
    BOTTOM,
    LEFT,
    RIGHT
}

public class PlayerStateMachine : MonoBehaviour
{
    BasicState[] m_states = new BasicState[4];
    PlayerData m_data;
    Rigidbody m_rigb;

    E_PLAYER_STATES m_currentState = E_PLAYER_STATES.IN_AIR;
    E_PLAYER_STATES m_newState = E_PLAYER_STATES.IN_AIR;

    public BoxMovenemt m_linkedBox = null;

    //public bool grounded = true;
    //bool DoubleJump = true;

    void Start()
    {
        m_states[0] = gameObject.AddComponent<OnGround>();
        m_states[1] = gameObject.AddComponent<InAIr>();
        m_states[2] = gameObject.AddComponent<MoveingBox>();
        m_states[3] = gameObject.AddComponent<ClimbingLadder>();

        m_data = GetComponent<PlayerData>();
        m_rigb = GetComponent<Rigidbody>();

        m_states[0].Initialize(m_rigb, m_data);
        m_states[1].Initialize(m_rigb, m_data);
        m_states[2].Initialize(m_rigb, m_data);
        m_states[3].Initialize(m_rigb, m_data);
    }

    void Update()
    {
        if (transform.position.y < -10.0f)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        m_data.m_pressingButton = Input.GetButtonDown("PressButton");

        m_newState = m_states[(int)m_currentState].Cycle();
        CheckState();

        m_data.m_closeToBox = false;
    }

    void FixedUpdate()
    {
        m_newState = m_states[(int)m_currentState].PhysCycle();
        CheckState();

        m_data.m_onLadder = false;

        for (int z = 0; z < 4; z++)
        {
            m_data.m_contacts[z] = false;
            m_data.m_InteractableContacts[z] = false;
        }
    }

    private void CheckGroundColl()
    {
        var up = transform.TransformDirection(Vector3.up);
        // note: the use of var as the type. This is because in c# you 
        // can have lamda functions which open up the use of untyped variables
        // these variables can only live INSIDE a function. 
        RaycastHit hit;
        Debug.DrawRay(transform.position, -up * 2, Color.green);

        if (Physics.Raycast(transform.position, -up, out hit, 2))
        {

            Debug.Log("HIT");

            if (hit.collider.gameObject.name == "floor")
            {
                Destroy(GetComponent("Rigidbody"));
            }
        }
    }

    void OnCollisionStay(Collision Other)
    {
        /*Debug.Log("Up: " + Vector3.Angle(Other.contacts[0].normal, Vector3.up));
        Debug.Log("Left: " + Vector3.Angle(Other.contacts[0].normal, Vector3.left));
        Debug.Log("Forward: " + Vector3.Angle(Other.contacts[0].normal, Vector3.forward));

        Debug.Log("Contacts: " + Other.contacts.Length);

        for (int z = 0; z < Other.contacts.Length; z++)
        {
            Debug.DrawRay(Other.contacts[z].point, Other.contacts[z].normal, Color.white);

            if (z < Other.contacts.Length - 1)
            {
                if (Other.contacts[z].normal != Other.contacts[z + 1].normal)
                {
                    Debug.Log("DIFF IN NORMALS");
                }
            }
        }*/

        float angle = Vector2.Angle(Other.contacts[0].normal, Vector2.up);

        E_DIRECTIONS dir = E_DIRECTIONS.TOP;

        if (Mathf.Approximately(angle, 0.0f))
        {
            dir = E_DIRECTIONS.BOTTOM;

            if (Other.gameObject.GetComponent<Rigidbody>())
            {
                m_data.m_velocityX += Other.gameObject.GetComponent<Rigidbody>().velocity.x;
            }

            if (Other.gameObject.tag != "Box")
            {
                m_data.m_contacts[2] = true;

                if (m_data.m_contacts[0])
                {
                    m_data.m_squished = true;
                    Debug.Log("Squished!");
                }
            }
        }
        else if (Mathf.Approximately(angle, 180.0f))
        {
            dir = E_DIRECTIONS.TOP;

            if (Other.gameObject.tag != "Box")
            {
                m_data.m_contacts[0] = true;

                if (m_data.m_contacts[2])
                {
                    m_data.m_squished = true;
                    Debug.Log("Squished!");
                }
            }
        }
        else if (Mathf.Approximately(angle, 90.0f))
        {
            angle = Vector2.Angle(Other.contacts[0].normal, Vector2.left);

            if (Mathf.Approximately(angle, 0.0f))
            {
                dir = E_DIRECTIONS.RIGHT;

                if (Other.gameObject.tag != "Box")
                {
                    m_data.m_contacts[1] = true;

                    if (m_data.m_contacts[3])
                    {
                        m_data.m_squished = true;
                        Debug.Log("Squished!");
                    }

                    if (m_data.m_InteractableContacts[3])
                    {
                        m_linkedBox.m_requestStop = true;
                        Debug.Log("Stop Requested");
                    }
                }
                else
                {
                    m_linkedBox = Other.gameObject.GetComponent<BoxMovenemt>();

                    m_data.m_InteractableContacts[1] = true;

                    if (m_data.m_contacts[3])
                    {
                        m_linkedBox.m_requestStop = true;

                        Debug.Log("Stop Requested");
                    }
                }
            }
            else if (Mathf.Approximately(angle, 180.0f))
            {
                dir = E_DIRECTIONS.LEFT;

                if (Other.gameObject.tag != "Box")
                {
                    m_data.m_contacts[3] = true;

                    if (m_data.m_contacts[1])
                    {
                        m_data.m_squished = true;
                        Debug.Log("Squished!");
                    }

                    if (m_data.m_InteractableContacts[1])
                    {
                        m_linkedBox.m_requestStop = true;
                        Debug.Log("Stop Requested");
                    }
                }
                else
                {
                    m_linkedBox = Other.gameObject.GetComponent<BoxMovenemt>();

                    m_data.m_InteractableContacts[3] = true;

                    if (m_data.m_contacts[1])
                    {
                        m_linkedBox.m_requestStop = true;

                        Debug.Log("Stop Requested");
                    }
                }
            }
        }

        if (Mathf.Approximately(Vector2.Angle(Other.contacts[0].normal, Vector2.left), 90.0f) && Mathf.Approximately(Vector2.Angle(Other.contacts[0].normal, Vector2.up), 90.0f))
        {
            m_data.m_squished = true;
        }

        m_newState = m_states[(int)m_currentState].Colide(dir, Other.gameObject.tag);
        CheckState();
    }

    void OnCollisionExit(Collision Other)
    {
        m_newState = m_states[(int)m_currentState].LeaveColision(Other.gameObject.tag);
        CheckState();
    }

    /*void OnCollisionExit(Collision Other)
    {
        grounded = false;

        // this is needed for disabling the double jump as OnCollisionStay will set doubleJump to true 
        // after the player has jumped as they are still making contact with the ground
        if (!m_data.m_playerDoubleJump)
        {
            DoubleJump = false;
        }

        if (Mathf.Approximately(angle, 90f))
        {
            if (slideRight & Other.transform.position.x > m_rigb.position.x)
            {
                m_useWallGravity = false;
                slideRight = false;
            }
            else if (slideLeft & Other.transform.position.x < m_rigb.position.x)
            {
                m_useWallGravity = false;
                slideLeft = false;
            }
        }
    }*/

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ladder")
        {
            m_data.m_onLadder = true;
        }

        m_newState = m_states[(int)m_currentState].InTrigger(other.gameObject.tag);
        CheckState();
    }

    void OnTriggerExit(Collider other)
    {
        m_newState = m_states[(int)m_currentState].LeaveTrigger(other.gameObject.tag);
        CheckState();
    }

    void CheckState()
    {
        if (m_newState != E_PLAYER_STATES.NULL && m_newState != m_currentState)
        {
            m_states[(int)m_currentState].Exit();
            m_states[(int)m_newState].Enter();

            Debug.Log(m_currentState + " -> " + m_newState);

            m_currentState = m_newState;
        }
    }
}
