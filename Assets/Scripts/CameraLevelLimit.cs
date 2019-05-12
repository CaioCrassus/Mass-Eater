using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLevelLimit : MonoBehaviour
{
    public Vector3 minPos;
    public Vector3 maxPos;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraControl.limit = this;
        }
    }
}
