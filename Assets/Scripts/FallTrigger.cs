using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{


    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            transform.parent.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
