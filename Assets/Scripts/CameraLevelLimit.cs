using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLevelLimit : MonoBehaviour
{
    public Vector3 minPos;
    public Vector3 maxPos;


    public Transform respawnPos;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraControl.limit = this;
            if (respawnPos != null)
                PlayerController.respawnPos = respawnPos.position;
        }
    }

    /*void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }*/
}
