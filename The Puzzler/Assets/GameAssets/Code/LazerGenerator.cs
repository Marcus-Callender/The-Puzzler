using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGenerator : MonoBehaviour
{
    //float length = 5.0f;
    //LineRenderer line;

    //void Start()
    //{

    //    line = GetComponent<LineRenderer>();
    //    line.positionCount = 2;
    //    line.startWidth = 0.2f;
    //    line.endWidth = 0.2f;

    //    line.startColor = Color.red;
    //    line.endColor = Color.red;
    //}

    //void Update()
    //{
    //    Vector3 pos = gameObject.transform.position;
    //    pos.y -= length;

    //    line.enabled = true;
    //    line.SetPosition(0, gameObject.transform.position);
    //    line.SetPosition(1, pos);
    //}

    /*private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));

        // Set some positions
        Vector3[] positions = new Vector3[3];
        positions[0] = new Vector3(-2.0f, -2.0f, 0.0f);
        positions[1] = new Vector3(0.0f, 2.0f, 0.0f);
        positions[2] = new Vector3(2.0f, -2.0f, 0.0f);
        lr.positionCount = positions.Length;
        lr.SetPositions(positions);

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lr.colorGradient = gradient;
    }*/

    private LineRenderer lr;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            print("Found an object - distance: " + hit.distance);

        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));

        // Set some positions
        Vector3[] positions = new Vector3[2];
        positions[0] = gameObject.transform.position;
        positions[1] = gameObject.transform.position;
        positions[1].y -= hit.distance;

        lr.positionCount = positions.Length;
        lr.SetPositions(positions);
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lr.colorGradient = gradient;
    }
}
