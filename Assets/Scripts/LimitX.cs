using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitX : MonoBehaviour
{

    public float minX;
    public float maxX;

    private Vector2 stayPos;

    public bool isHeld = false;

    void Start()
    {
        stayPos.y = transform.position.y;
        stayPos.x = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 aux = transform.position;// transform.TransformPoint(transform.position);
        if (isHeld) stayPos.x = transform.position.x;
        else aux.x = stayPos.x;
        aux.x = Mathf.Clamp(aux.x, minX, maxX);
        aux.y = stayPos.y;
        transform.position = aux;//transform.InverseTransformPoint(aux);
    }
}
