using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
    Renderer m_material;
	// Use this for initialization
	void Start () {
        m_material = gameObject.GetComponent<Renderer>();
        m_material.material.color = Color.blue;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
