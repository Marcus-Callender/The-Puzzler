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
        Vector3 pos = gameObject.transform.position;
        pos.x += 0.1f;
        Debug.DrawRay(pos, new Vector3(0.1f, 0.0f, 0.0f), Color.green);
    }

    /*void OnTriggerExit(Collider other)
    {
        Debug.DrawRay(gameObject.transform.position, Vector3.right, Color.red);

        Physics.Raycast(gameObject.transform.position, Vector3.right);

        //RaycastHit ray;

        Vector3 pos = gameObject.transform.position;
        pos.x += 0.5f;

        bool use3D = !Physics.Raycast(pos, Vector3.right, 0.1f);

        Debug.Log("3D Trigger: " + use3D);

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ghost")
        {
            PlayerData data = other.gameObject.GetComponent<PlayerData>();

            if (data)
            {
                //data.m_use3D = !data.m_use3D;
                data.m_use3D = use3D;
                data.m_left_right = true;

                other.gameObject.transform.rotation = gameObject.transform.rotation;
                other.gameObject.transform.Rotate(Vector3.up, 90.0f);
            }
        }
    }*/

    /*void OnTriggerStay(Collider other)
    {
        Quaternion otherRoataion = other.transform.rotation;
        otherRoataion.x += 90.0f;

        //if (Mathf.Abs(otherRoataion.x - gameObject.transform.rotation.x) > 180.0f)
        if ((otherRoataion.x - gameObject.transform.rotation.x) > 0.0f)
        {
            Debug.Log("2D");
        }
        else
        {
            Debug.Log("3D");
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        float x;
        Vector3 up = Vector3.up;
        other.transform.rotation.ToAngleAxis(out x, out up);
        Debug.Log("X: " + x + " || Up: " + up);

        bool use3D = Mathf.Abs(x - other.gameObject.transform.rotation.y) > Mathf.Abs(x - other.gameObject.transform.rotation.y - 90.0f);

        Debug.Log("Use3D: " + use3D);
        Debug.Log("Diffrence: " + Mathf.Abs(x - other.gameObject.transform.rotation.y));

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ghost")
        {
            PlayerData data = other.gameObject.GetComponent<PlayerData>();
            
            if (data)
            {
                data.m_use3D = !data.m_use3D;
                //data.m_use3D = use3D;
                data.m_left_right = true;

                //other.gameObject.transform.rotation = gameObject.transform.rotation;
                //other.gameObject.transform.Rotate(Vector3.up, 90.0f);

                data.SetRotation(gameObject.transform.rotation);
            }
        }






        /*Debug.Log("charicter forward: " + other.transform.rotation.y);

        Quaternion otherRoataion = other.transform.rotation;
        otherRoataion.x += 90.0f;

        bool use3D = ((otherRoataion.x - gameObject.transform.rotation.x) < 0.0f);

        //Debug.Log("3D Trigger: " + use3D);

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ghost")
        {
            PlayerData data = other.gameObject.GetComponent<PlayerData>();

            if (data)
            {
                data.m_use3D = !data.m_use3D;
                //data.m_use3D = use3D;
                data.m_left_right = true;

                //other.gameObject.transform.rotation = gameObject.transform.rotation;
                //other.gameObject.transform.Rotate(Vector3.up, 90.0f);
            }
        }*/
    }

    /*private void OnTriggerExit(Collider other)
    {
        Quaternion otherRoataion = other.transform.rotation;

        bool use3D = ((otherRoataion.z - gameObject.transform.rotation.y) < 0.0f);

        Debug.Log("Trigger Angle X: " + (otherRoataion.z - gameObject.transform.rotation.x));
        Debug.Log("Trigger Angle Y: " + (otherRoataion.z - gameObject.transform.rotation.y));
        Debug.Log("Trigger Angle Z: " + (otherRoataion.z - gameObject.transform.rotation.z));



        Vector3 direction = other.transform.position - gameObject.transform.position;
        float angle = Vector3.Angle(direction, transform.up);

        //other.transform.localRotation.y;
        //gameObject.transform.localRotation.y;



        Debug.Log("3D Trigger: " + angle);

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ghost")
        {
            PlayerData data = other.gameObject.GetComponent<PlayerData>();

            if (data)
            {
                Debug.Log("Angle Diffrence: " + (gameObject.transform.localRotation.y - data.GetRealRotation().y));

                /*if (Mathf.Abs(gameObject.transform.localRotation.y - data.GetRealRotation().y) > 90.0f)
                {

                    data.m_use3D = use3D;
                    data.m_left_right = true;
    
                    other.gameObject.transform.rotation = gameObject.transform.rotation;
                    other.gameObject.transform.Rotate(Vector3.up, 90.0f);
                }*/

                /*use3D = (Mathf.Abs(gameObject.transform.localRotation.y - data.GetRealRotation().y) > 90.0f);

                //Debug.Log("#####" + use3D);
                Debug.Log("#####" + data.GetRealRotation().y);

                data.m_use3D = use3D;
                data.m_left_right = true;

                other.gameObject.transform.rotation = gameObject.transform.rotation;
                other.gameObject.transform.Rotate(Vector3.up, 90.0f);


                //data.m_use3D = !data.m_use3D;
            }
        }
    }*/
}
