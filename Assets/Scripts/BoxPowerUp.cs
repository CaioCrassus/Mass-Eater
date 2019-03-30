using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().canHold = true;
            Debug.Log("Can push boxes!!");
        }
    }
}
