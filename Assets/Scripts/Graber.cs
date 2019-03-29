using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graber : MonoBehaviour
{
    public PlayerController player;
    public GameObject obj;

    void Update(){
        if (Input.GetButtonUp("Fire3")) {
            obj.transform.SetParent(null);
            player.holding = false;
            obj = null;
        }

    }

    void OnTriggerStay(Collider other){
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Moveble") && Input.GetButtonDown("Fire3") && !player.crouching){
            obj = other.gameObject;
            obj.transform.SetParent(transform);
            Vector3 aux = obj.transform.localPosition;
            aux.x *= 1.2f;
            obj.transform.localPosition = aux;
            player.holding = true;
        }

    }
}
