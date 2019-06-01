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
        stayPos.y = rd.position.y;
        stayPos.x = rd.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 aux = rd.position;// rd.rdPoint(rd.position);
        if (isHeld)
        {
            if (stayPos.x != rd.position.x && !audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.PlayOneShot(dragedAudio);
            }
            else if (stayPos.x == rd.position.x && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            stayPos.x = rd.position.x;
        }
        else aux.x = stayPos.x;
        aux.x = Mathf.Clamp(aux.x, minX, maxX);
        //aux.y = stayPos.y;
        rd.position = aux;//rd.InverserdPoint(aux);
    }

    public void DestroyBox()
    {
        //transform.GetComponentInChildren<ParticleSystem>().Play(true);
        Invoke("DB", .1f);
    }

    private void DB()
    {
        Destroy(gameObject);
    }
}
