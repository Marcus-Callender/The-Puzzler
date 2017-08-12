using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharicterPosition
{
    public Vector3 pos;
    public Quaternion rot;
    public bool left_right;
    public bool use3D;
}

public class PlayerData : MonoBehaviour
{
    public float m_velocityX = 0.0f;
    public float m_velocityY = 0.0f;
    public float m_velocityZ = 0.0f;

    public bool m_moveingBox = false;
    public bool m_closeToBox = false;

    public bool m_pressingButton = false;

    public bool m_playerDoubleJump = false;
    public bool m_playerWallSlide = false;

    public bool m_squished = false;
    public bool m_onLadder = false;

    // 0 = top, 1 = right, 2 = bottom, 3 = left
    public bool[] m_contacts = new bool[4];
    public bool[] m_InteractableContacts = new bool[4];

    // left = false, right = true
    public bool m_left_right = true;

    public Rigidbody m_rigb;
    public Animator m_anim;

    public GameObject m_ghost;
    public BoxMovenemt m_linkedBox;

    public PlayerData m_overideFollow;

    public bool m_use3D = false;
    public Quaternion m_rotation;
    public bool m_stopRotation = false;
    
    public bool m_pause = false;
    //public char m_inputs;
    //public char m_JoystickMovement;

    public Quaternion m_cameraRotation;

    public void Initialize()
    {
        m_rigb = GetComponent<Rigidbody>();

        PersistantData data = PersistantData.m_instance;

        m_anim = GetComponent<Animator>();

        m_rotation = gameObject.transform.rotation;

        if (data)
        {
            m_playerDoubleJump = data.m_playerDoubleJump;
            m_playerWallSlide = data.m_playerWallSlide;
        }

        m_cameraRotation = transform.rotation;
    }

    void Update()
    {
        if (!m_stopRotation)
        {
            if (!m_use3D)
            {
                if (m_left_right && m_velocityX < 0.0f)
                {
                    gameObject.transform.Rotate(new Vector3(0.0f, 180.0f));

                    m_rotation = gameObject.transform.rotation;

                    m_left_right = false;
                }
                else if (!m_left_right && m_velocityX > 0.0f)
                {
                    gameObject.transform.Rotate(new Vector3(0.0f, 180.0f));

                    m_rotation = gameObject.transform.rotation;

                    m_left_right = true;
                }
            }

            gameObject.transform.rotation = m_rotation;
        }
    }

    void FixedUpdate()
    {
        if (!m_use3D)
        {
            m_rigb.velocity = ((gameObject.transform.forward * (m_left_right ? 1.0f : -1.0f)) * m_velocityX) + (gameObject.transform.up * m_velocityY);
        }
        else
        {
            m_rigb.velocity = (gameObject.transform.forward * m_velocityX) + (gameObject.transform.up * m_velocityY) + (gameObject.transform.right * m_velocityZ);
        }

        m_velocityX = 0.0f;
        m_velocityZ = 0.0f;
    }

    public Vector3 GetCenterTransform()
    {
        Vector3 newTreansform = gameObject.transform.position;

        newTreansform.y += gameObject.transform.localScale.y;

        return newTreansform;
    }

    public Quaternion GetRealRotation()
    {
        // adjustes for left/right rotations in 2d mode to make calculations easier
        if (m_use3D)
        {
            return gameObject.transform.rotation;
        }

        Quaternion realRotation = gameObject.transform.rotation;

        if (!m_left_right)
        {
            realRotation.y += 180.0f;
        }

        return realRotation;
    }

    public void SetRotation(Quaternion rot)
    {
        // only run when the player is in 2d
        if (!m_use3D)
        {
            Quaternion newRot = rot;

            // rotates the player 90 degrees sideways
            newRot *= Quaternion.Euler(0.0f, 90.0f, 0.0f);

            m_rotation = newRot;
        }
    }

    // returns the vecolity the player will attempt to move this frame
    public Vector3 GetExpectedVelocity()
    {
        Vector3 vel = Vector3.zero;

        if (!m_use3D && !m_left_right)
        {
            // used in excaptions where the box movment would be reversed
            vel += (-m_velocityX * transform.forward) + (m_velocityY * transform.up) + (gameObject.transform.right * m_velocityZ);
        }
        else
        {
            vel += (m_velocityX * transform.forward) + (m_velocityY * transform.up) + (gameObject.transform.right * m_velocityZ);
        }

        return vel;
    }

    public CharicterPosition getPositionData()
    {
        CharicterPosition posData = new CharicterPosition();

        posData.pos = transform.position;
        posData.rot = transform.rotation;
        posData.use3D = m_use3D;
        posData.left_right = m_left_right;

        return posData;
    }

    public void resetCameraDirection()
    {
        m_cameraRotation = transform.rotation;
        m_rotation = transform.rotation;
    }
}
