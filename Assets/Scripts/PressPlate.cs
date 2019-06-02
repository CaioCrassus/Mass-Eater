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

    public AudioClip box;

    public AudioClip player;

    private AudioSource audioSource;

    public bool justPressed;

    public GameObject objPressing;

    public Spawner spawner;

    public bool destroyPressingObject;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        justPressed = false;
        if (pressed && objPressing == null)
        {
            StartCoroutine("ReturnToOrigin");
        }
    }

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
                if (toActivate != null)
                    toActivate.run();
            }

            if (col.CompareTag("Player")) audioSource.clip = player;
            else audioSource.clip = box;

            audioSource.Play();
            justPressed = true;
            if (spawner != null)
                spawner.SpawnObject();
            objPressing = col.gameObject;

            //if (col.CompareTag("Moveble") && destroyPressingObject) objPressing.GetComponent<LimitX>().DestroyBox();
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
        pressed = false;
        if (!triggerOnce && !triggerOnly)
        {
            if (toActivate != null)
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
