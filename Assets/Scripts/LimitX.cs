using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitX : MonoBehaviour
{

    public float minX;
    public float maxX;

    private Vector2 stayPos;

    public bool isHeld = false;

    private Rigidbody rd;
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        stayPos.y = rd.position.y;
        stayPos.x = rd.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 aux = rd.position;// transform.TransformPoint(transform.position);
        if (isHeld) stayPos.x = rd.position.x;
        else aux.x = stayPos.x;
        aux.x = Mathf.Clamp(aux.x, minX, maxX);
        aux.y = stayPos.y;
        rd.position = aux;//transform.InverseTransformPoint(aux);
    }
}
