using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitX : MonoBehaviour
{

    public float minX;
    public float maxX;

    private float y;

    public bool isHeld = false;

    void Start()
    {
        y = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 aux = transform.position;// transform.TransformPoint(transform.position);
        if (aux.x < minX) aux.x = minX;
        else if (aux.x > maxX) aux.x = maxX;
        aux.y = y;
        //if (!isHeld)
        transform.position = aux;//transform.InverseTransformPoint(aux);
    }
}
