using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBox : MonoBehaviour, IGhostInteractable
{
    Vector3 m_basePosition;
    public Material m_mat;
    private Material m_baseMat;

    void Start()
    {

    }

    void Update()
    {

    }

    public void StartIntecation()
    {
        Debug.Log("### Start interaction: " + transform.position);
        m_basePosition = transform.position;
        m_baseMat = gameObject.GetComponent<Renderer>().material;
        gameObject.GetComponent<Renderer>().material = m_mat;
    }

    public void StopIntecation()
    {
        Debug.Log("### Stop interaction: " + transform.position + " to: " + m_basePosition);
        transform.position = m_basePosition;
        gameObject.GetComponent<Renderer>().material = m_baseMat;
    }
}
