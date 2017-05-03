using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this object is used to keep any data between levels/games
public class PersistantData : MonoBehaviour
{
    public bool m_playerDoubleJump = false;
    public bool m_playerWallSlide = false;

    public static PersistantData m_instance;


    void Awake()
    {
        if (m_instance == null)
        {
            DontDestroyOnLoad(this);
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {

    }
}
