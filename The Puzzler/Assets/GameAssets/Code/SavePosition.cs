using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePosition : MonoBehaviour
{


    void Start()
    {

    }
    
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        SaveData data = other.gameObject.GetComponent<SaveData>();

        if (data)
        {
            data.SavePosition(gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
