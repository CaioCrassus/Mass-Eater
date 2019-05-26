using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Activate : MonoBehaviour
{
    private static Camera mainCam;
    public GameObject obj;

    public AudioClip open;
    public AudioClip close;

    private AudioSource audioSource;

    void Start()
    {
        mainCam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    public void run()
    {
        mainCam.gameObject.GetComponent<CameraControl>().target = obj;
        PlayerController.instance.cameraOnPlayer = false;
        Invoke("runMecanism", .8f);
    }

    void runMecanism()
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
            audioSource.PlayOneShot(close);
        }
        else
        {
            obj.SetActive(true);
            audioSource.PlayOneShot(open);
        }

        Invoke("BackToMainCamera", .5f);
    }

    void BackToMainCamera()
    {
        mainCam.gameObject.GetComponent<CameraControl>().resetTarget();
        PlayerController.instance.cameraOnPlayer = true;
    }
}
