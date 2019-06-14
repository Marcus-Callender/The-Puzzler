using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTransition : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_newGravity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            PlayerData data = other.gameObject.GetComponent<PlayerData>();

            if (data)
            {
                data.m_gravity = m_newGravity;
            }
        }
    }
}
