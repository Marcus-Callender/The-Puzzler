using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    protected bool m_activated;

    public virtual void OnInteract()
    {
        m_activated = !m_activated;
    }
}
