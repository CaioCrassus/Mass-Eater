using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPowerUp : MonoBehaviour
{

    public GameObject messege;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().canHold = true;
            Debug.Log("Can push boxes!!");
            messege.SetActive(true);
            Invoke("DisableMessege", 3f);
        }
    }

    void DisableMessege()
    {
        messege.SetActive(false);
    }
}
