﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressPlate : MonoBehaviour
{
    public bool pressed { private set; get; }

    public bool playerCanActivate = true;

    private Vector3 origin;
    private Vector3 pressedPos;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer != 9)
        {
            origin = transform.parent.position;
            pressedPos = origin - (Vector3.up * 0.09f);
            pressed = true;
            Debug.Log("Pressed");
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.layer != 9 && !col.CompareTag("Player"))
        {
            transform.parent.position = Vector3.Lerp(transform.parent.position, pressedPos, .1f);
            if (Vector3.Distance(transform.parent.position, pressedPos) < 0.01f)
                transform.parent.position = pressedPos;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer != 9 && !col.CompareTag("Player"))
            StartCoroutine("ReturnToOrigin");
    }


    IEnumerator ReturnToOrigin()
    {
        while (origin != transform.parent.position)
        {
            transform.parent.position = Vector3.Lerp(transform.parent.position, origin, .1f);
            if (Vector3.Distance(transform.parent.position, origin) < 0.01f)
                transform.parent.position = origin;
            yield return 0;
        }
    }
}
