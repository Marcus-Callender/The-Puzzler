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
    private Vector3 m_velocity;

    public Vector3 m_gravity;

    // used to tell the camera to transition to a new state
    public bool m_moveingBox = false;

    // used by buttons to tell  if the player wants them to activate
    public bool m_pressingButton = false;

    public bool m_playerDoubleJump = false;
    public bool m_playerWallSlide = false;

    // moves the player to the KOd state
    public bool m_squished = false;

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

    // TODO see if this can be removed
    private Quaternion m_rotation;

    // used to prevent the player flipping while in 2D and moving crates 
    public bool m_stopRotation = false;

    // used to prevent delta time indapendent actions such as ghost playbacks from running while the game is paused
    public bool m_pause = false;

    public Quaternion m_cameraRotation;

    public List<S_inputStruct> m_preloadedInputs;

    public void Initialize()
    {
        m_rigb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();

        PersistantData data = PersistantData.m_instance;

        m_preloadedInputs = new List<S_inputStruct>();

        SetRotation(gameObject.transform.rotation);

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
            gameObject.transform.rotation = m_rotation;
        }
    }

    void FixedUpdate()
    {
        if (!m_use3D)
        {
            m_rigb.velocity = ((gameObject.transform.forward * (m_left_right ? 1.0f : -1.0f)) * m_velocity.x) + (gameObject.transform.up * m_velocity.y);
        }
        else
        {
            m_rigb.velocity = (gameObject.transform.forward * m_velocity.x) + (gameObject.transform.up * m_velocity.y) + (gameObject.transform.right * m_velocity.z);
        }

        m_velocity.x = 0.0f;
        m_velocity.z = 0.0f;
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
        m_rotation = rot;
    }

    // returns the vecolity the player will attempt to move this frame
    public Vector3 GetExpectedVelocity()
    {
        Vector3 vel = Vector3.zero;

        if (!m_use3D && !m_left_right)
        {
            // used in excaptions where the box movment would be reversed
            vel += (-m_velocity.x * transform.forward) + (m_velocity.y * transform.up) + (gameObject.transform.right * m_velocity.z);
        }
        else
        {
            vel += (m_velocity.x * transform.forward) + (m_velocity.y * transform.up) + (gameObject.transform.right * m_velocity.z);
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
        SetRotation(transform.rotation);
    }

    public Quaternion getCameraRot()
    {
        return m_cameraRotation;
    }

    public Vector3 GetVelocity()
    {
        return m_velocity;
    }

    public void AddVelocity(Vector3 AddVel)
    {
        m_velocity += AddVel;
    }

    public void AddVelocity(float x, float y, float z)
    {
        if (Mathf.Abs(z) > 6.5 || Mathf.Abs(z) > 6.5)
        {
            Debug.LogError("Movment error!");
        }

        m_velocity.x += x;
        m_velocity.y += y;
        m_velocity.z += z;
    }

    public void SetVelocity(float x, float y, float z)
    {
        m_velocity.x = x;
        m_velocity.y = y;
        m_velocity.z = z;
    }

    public void SetYVelocity(float y)
    {
        m_velocity.y = y;
    }

    public void SetVelocity(char c, float f)
    {
        if (c == 'x')
        {
            m_velocity.x = f;
        }
        if (c == 'y')
        {
            m_velocity.y = f;
        }
        if (c == 'z')
        {
            m_velocity.z = f;
        }
    }
}
