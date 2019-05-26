using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LimitX : MonoBehaviour
{

    public float minX;
    public float maxX;

    private Vector2 stayPos;

    public bool isHeld = false;

    private Rigidbody rd;

    private AudioSource audioSource;
    public AudioClip dragedAudio;

    void Start()
    {
        rd = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        stayPos.y = transform.position.y;
        stayPos.x = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 aux = transform.position;// transform.TransformPoint(transform.position);
        if (isHeld)
        {
            if (stayPos.x != transform.position.x && !audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.PlayOneShot(dragedAudio);
            }
            else if (stayPos.x == transform.position.x && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            stayPos.x = transform.position.x;
        }
        else aux.x = stayPos.x;
        aux.x = Mathf.Clamp(aux.x, minX, maxX);
        aux.y = stayPos.y;
        transform.position = aux;//transform.InverseTransformPoint(aux);
    }
}
