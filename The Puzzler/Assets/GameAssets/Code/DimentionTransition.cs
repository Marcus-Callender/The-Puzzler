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
        //Vector3 pos = gameObject.transform.position;
        //pos.x += 0.1f;
        //Debug.DrawRay(pos, new Vector3(0.1f, 0.0f, 0.0f), Color.green);
        Debug.DrawRay(transform.position + new Vector3(0.7f, 0.0f), transform.right, Color.blue);
    }
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ghost")
        {
            PlayerData data = other.gameObject.GetComponent<PlayerData>();
            
            if (data)
            {
                //data.m_use3D = Physics.Raycast(transform.position + new Vector3(0.7f ,0.0f), transform.right);

                //data.m_use3D = !data.m_use3D;

                Vector3 left = gameObject.transform.position - gameObject.transform.right;
                Vector3 right = gameObject.transform.position + gameObject.transform.right;

                data.m_use3D = (Vector3.Distance(data.GetCenterTransform(), left) > Vector3.Distance(data.GetCenterTransform(), right));
                
                data.m_left_right = true;
                
                data.SetRotation(gameObject.transform.rotation);
            }
        }
        
    }
}
