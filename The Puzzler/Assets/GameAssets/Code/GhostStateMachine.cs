using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GhostStateMachine : MonoBehaviour
{
    BasicState[] m_states = new BasicState[4];
    PlayerData m_data;
    Rigidbody m_rigb;

    E_PLAYER_STATES m_currentState = E_PLAYER_STATES.IN_AIR;
    E_PLAYER_STATES m_newState = E_PLAYER_STATES.IN_AIR;

    public BoxMovenemt m_linkedBox = null;

    public GhostInputs m_inputs = null;

    public void Activate(Vector3 pos)
    {
        m_data.m_anim.SetBool("Stopped", false);

        if (m_inputs.m_recorded)
        {
            m_inputs.m_arrayPosition = 0;
            //IEnumerator couroutine = m_inputs.Play();
            //StartCoroutine(couroutine);
            m_inputs.m_playing = true;
        }
        else
        {
            gameObject.transform.position = pos;
            m_inputs.m_arrayPosition = 0;
            m_inputs.m_recording = true;
        }
    }
    
    void Start()
    {
        m_inputs = gameObject.AddComponent<GhostInputs>();

        m_states[0] = gameObject.AddComponent<OnGround>();
        m_states[1] = gameObject.AddComponent<InAIr>();
        m_states[2] = gameObject.AddComponent<MoveingBox>();
        m_states[3] = gameObject.AddComponent<ClimbingLadder>();

        m_data = GetComponent<PlayerData>();
        m_rigb = GetComponent<Rigidbody>();

        m_states[0].Initialize(m_rigb, m_data, m_inputs);
        m_states[1].Initialize(m_rigb, m_data, m_inputs);
        m_states[2].Initialize(m_rigb, m_data, m_inputs);
        m_states[3].Initialize(m_rigb, m_data, m_inputs);

        m_data.m_anim.SetBool("Stopped", true);
    }

    void Update()
    {
        if (m_inputs.m_recorded && m_inputs.m_arrayPosition == GhostInputs.m_recordingSize)
        {
            m_data.m_anim.SetBool("Stopped", true);
            m_inputs.m_pauseInputs = true;
        }

        if (transform.position.y < -10.0f)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        m_data.m_pressingButton = m_inputs.GetInput(E_INPUTS.PRESS_BUTTON);

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

            Debug.Log("Ghost: " + m_currentState + " -> " + m_newState);

            m_currentState = m_newState;
        }
    }
}
