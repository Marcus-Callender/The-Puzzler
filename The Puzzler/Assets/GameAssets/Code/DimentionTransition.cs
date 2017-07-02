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
    
    private void OnTriggerEnter(Collider other)
    {
        // checks if the thing that entered the trigger needs to change dimention

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ghost")
        {
            // gets a player data refrence from it
            PlayerData data = other.gameObject.GetComponent<PlayerData>();
            
            // checks the refrence is valid
            if (data)
            {
                // creates two refrences for each end of the trasition zone
                Vector3 left = gameObject.transform.position - gameObject.transform.right;
                Vector3 right = gameObject.transform.position + gameObject.transform.right;

                // sets the using 3d to 
                data.m_use3D = (Vector3.Distance(data.GetCenterTransform(), left) > Vector3.Distance(data.GetCenterTransform(), right));
                
                // sets this to prevent 2d contols being inverted
                data.m_left_right = true;
                
                // tells the player data what rotation this object has
                data.SetRotation(gameObject.transform.rotation);
            }
        }
        
    }
}
