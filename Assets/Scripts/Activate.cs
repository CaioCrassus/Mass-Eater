using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : MonoBehaviour
{

    public GameObject obj;

    public void run()
    {
        if (obj.activeSelf) obj.SetActive(false);
        else obj.SetActive(true);
    }
}
