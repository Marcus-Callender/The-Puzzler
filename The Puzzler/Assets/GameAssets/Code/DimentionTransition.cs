using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimentionTransition : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ghost")
        {
            PlayerData data = other.gameObject.GetComponent<PlayerData>();

            Physics.Raycast(new Ray(transform.position, other.transform.position - transform.position));

            Debug.DrawRay(transform.position, other.transform.position - transform.position, Color.blue);
            Debug.DrawRay(transform.position, transform.right, Color.green);

            if (Vector3.Angle(transform.right, other.transform.position - transform.position) < 90.0f)
            {
                data.m_use3D = false;
                //data.m_left_right = true;
                data.m_left_right = (data.m_velocityX < 0.0f);
                data.SetRotation(gameObject.transform.rotation);
            }
            else
            {
                data.m_use3D = true;
            }
        }
    }
}
