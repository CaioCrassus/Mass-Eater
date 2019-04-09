using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : MonoBehaviour
{
    private static Camera mainCam;
    public GameObject obj;

    void Start()
    {
        mainCam = Camera.main;
    }

    public void run()
    {
        mainCam.gameObject.GetComponent<CameraControl>().target = obj;
        PlayerController.instance.cameraOnPlayer = false;
        Invoke("runMecanism", .8f);
    }

    void runMecanism()
    {
        if (obj.activeSelf) obj.SetActive(false);
        else obj.SetActive(true);
        Invoke("BackToMainCamera", .5f);
    }

    void BackToMainCamera()
    {
        mainCam.gameObject.GetComponent<CameraControl>().resetTarget();
        PlayerController.instance.cameraOnPlayer = true;
    }
}
