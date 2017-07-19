using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UICountdown : MonoBehaviour
{
    Image m_sprite;
    public float m_progress = 1.0f;

    void Start()
    {
        m_sprite = GetComponent<Image>();

        m_sprite.type = Image.Type.Filled;
        m_sprite.fillMethod = Image.FillMethod.Radial360;
        m_sprite.fillAmount = 1.0f;
        m_sprite.fillClockwise = false;
    }
    
    void Update()
    {
        m_sprite.fillAmount = m_progress;
    }
}
