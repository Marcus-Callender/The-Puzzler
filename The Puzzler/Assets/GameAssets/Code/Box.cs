using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.gray;
    }
    
    void Update()
    {

    }
}
