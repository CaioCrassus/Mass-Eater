using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressPlate : MonoBehaviour
{
    public bool pressed { private set; get; }

    public bool playerCanActivate = true;
    public bool triggerOnce = false;
    private bool triggerOnceCheck = true;
    public bool triggerOnly = false;

    private Vector3 origin;
    private Vector3 pressedPos;

    public Activate toActivate;

    public string interactibleTag;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer != 9 && col.CompareTag(interactibleTag))
        {
            if (col.CompareTag("Player") && !playerCanActivate) return;
            origin = transform.parent.position;
            pressedPos = origin - (Vector3.up * 0.09f);
            Debug.Log("Pressed");
            if (triggerOnce && triggerOnceCheck)
            {
                pressed = true;
                toActivate.run();
                triggerOnceCheck = false;
            }
            else if (!triggerOnce)
            {
                pressed = true;
                toActivate.run();
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.layer != 9 && col.CompareTag(interactibleTag))
        {
            if (col.CompareTag("Player") && !playerCanActivate) return;
            transform.parent.position = Vector3.Lerp(transform.parent.position, pressedPos, .1f);
            if (Vector3.Distance(transform.parent.position, pressedPos) < 0.01f)
            {
                transform.parent.position = pressedPos;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer != 9 && col.CompareTag(interactibleTag))
        {
            if (col.CompareTag("Player") && !playerCanActivate) return;
            StartCoroutine("ReturnToOrigin");
        }
    }


    IEnumerator ReturnToOrigin()
    {
        if (!triggerOnce && !triggerOnly)
        {
            pressed = false;
            toActivate.run();
        }
        while (origin != transform.parent.position)
        {
            transform.parent.position = Vector3.Lerp(transform.parent.position, origin, .1f);
            if (Vector3.Distance(transform.parent.position, origin) < 0.01f)
                transform.parent.position = origin;
            yield return 0;
        }
    }
}
