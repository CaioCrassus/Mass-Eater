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

    public float waitTime = 1f;

    private AudioSource audioSource;

    public bool activateOnce;
    private bool activatedOnce = false;
    void Start()
    {
        mainCam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    public void run()
    {
        if (!activateOnce || !activatedOnce)
        {
            mainCam.gameObject.GetComponent<CameraControl>().target = obj;
            PlayerController.instance.cameraOnPlayer = false;
            Invoke("runMecanism", .8f);
            activatedOnce = true;
        }
    }

    void runMecanism()
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
            if(close != null)
            audioSource.PlayOneShot(close);
        }
        else
        {
            obj.SetActive(true);
            if(open != null)
            audioSource.PlayOneShot(open);
        }

        Invoke("BackToMainCamera", waitTime);
    }

    void BackToMainCamera()
    {
        mainCam.gameObject.GetComponent<CameraControl>().resetTarget();
        PlayerController.instance.cameraOnPlayer = true;
        //if (deactivateControlAfterUse) Destroy(this);
    }
}
