using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateNextRoom : MonoBehaviour
{
    public GameObject nextRoom;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nextRoom.SetActive(true);
        }
    }
}
