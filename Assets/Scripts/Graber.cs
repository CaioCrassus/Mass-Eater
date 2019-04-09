using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graber : MonoBehaviour
{
    public PlayerController player;
    public GameObject obj;

    private Vector3 objPos;

    public GameObject Boxes;
    void Update()
    {
        if (Input.GetButtonUp("Fire3"))
        {
            obj.transform.SetParent(Boxes.transform);
            player.holding = false;
            obj = null;
            objPos = Vector3.zero;
        }

    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.name);

        if (player.canHold && other.CompareTag("Moveble") && Input.GetButtonDown("Fire3") && !player.crouching && player.controller.isGrounded)
        {
            obj = other.gameObject;
            obj.transform.SetParent(transform);
            Vector3 aux = obj.transform.localPosition;
            aux.x *= 1.1f;
            obj.transform.localPosition = aux;
            objPos = obj.transform.localPosition;
            player.holding = true;
            other.gameObject.GetComponent<LimitX>().isHeld = true;
        }
        if (objPos != Vector3.zero)
            obj.transform.localPosition = objPos;
    }

    void OnTriggerExit(Collider other)
    {
        obj.transform.SetParent(Boxes.transform);
        player.holding = false;
        obj = null;
        objPos = Vector3.zero;
        other.gameObject.GetComponent<LimitX>().isHeld = false;
    }
}
