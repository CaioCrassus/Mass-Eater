﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Scorpion : Enemy
{

    public float speed = 2;
    public float gravity = 20;
    private AudioSource audioSource;

    public LayerMask platformLayer;

    public bool persue = false;
    public float persueSpeed = 2.5f;
    public LayerMask playerLayer;
    public float viewAngle = 15;
    public float viewRadius = 3;
    private Vector3 move;

    public bool playerOnView = false;


    public AudioClip walkSound;
    public AudioClip agroSound;

    public bool attack = false;

    public Animator animator;

    public ScorpionAttackBox attackBox;
    // Start is called before the first frame update
    void Start()
    {
        base.controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!weakened)
        {
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, playerLayer);

            GameObject player = null;

            float dstToTarget = 10;

            if (targetsInViewRadius.Length > 0)
            {
                player = targetsInViewRadius[0].gameObject;
                /*Vector3 dirToTarget = (player.transform.position - transform.position).normalized;
                if (Vector2.Angle(transform.right + new Vector3(0, .45f), dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, player.transform.position);

                    if (playerOnView && !Physics.Raycast(transform.position + new Vector3(0, .45f), dirToTarget, dstToTarget, platformLayer))
                    {
                        Debug.DrawRay(transform.position, dirToTarget * 100000, Color.white);
                        persue = true;
                    }
                    else
                    {
                        persue = false;
                    }
                }*/

                Vector3 dirToTarget = (player.transform.position - transform.position).normalized;
                dstToTarget = Vector3.Distance(transform.position, player.transform.position);
                if (playerOnView && !Physics.Raycast(transform.position + new Vector3(0, .45f), dirToTarget, dstToTarget, platformLayer))
                {
                    Debug.DrawRay(transform.position, dirToTarget * 100000, Color.white);
                    persue = true;
                    if (!audioSource.isPlaying && audioSource.clip == agroSound) audioSource.PlayOneShot(agroSound);
                }
                else
                {
                    persue = false;
                    if (!audioSource.isPlaying && audioSource.clip == walkSound) audioSource.PlayOneShot(walkSound);
                }
            }



            bool ray = Physics.Raycast(transform.position + Vector3.up * 0.25f, transform.right, .8f, platformLayer);
            bool down = Physics.Raycast(transform.position + transform.right * 0.45f, Vector3.down, .8f, platformLayer);
            Debug.DrawRay(transform.position + Vector3.up * 0.25f, transform.right * .8f, Color.white);
            Debug.DrawRay(transform.position + transform.right * 0.45f, Vector3.down * .8f, Color.white);

            Vector3 aux = transform.localEulerAngles;
            if ((ray || !down) && !persue)
            {
                if (transform.localRotation.y == 0) aux.y = 180;
                else /*if (transform.localRotation.y == 180)*/ aux.y = 0;

            }
            if (persue && player != null)
            {
                float dist = Mathf.Abs(player.transform.position.x - transform.position.x);
                //if (player.transform.position.x < transform.position.x && dist > 1) aux.y = 180;
                //else if (dist > 1) aux.y = 0;
                if (dstToTarget < 1 && player.GetComponent<PlayerController>().invencibleTimer <= 0)
                {
                    attackBox.gameObject.SetActive(true);
                    attack = true;
                    animator.SetBool("attack", true);
                    move.x = 0;
                }
            }
            transform.localEulerAngles = aux;

            move.x = transform.right.x * (persue ? persueSpeed : speed);
            if (controller.isGrounded) move.y = 0;
            else move.y -= gravity * Time.deltaTime;

            if (attack) move.x = 0;

            animator.SetFloat("xVel", move.x);

            controller.Move(move * Time.deltaTime);
            aux = transform.position;
            aux.z = 0;
            transform.position = aux;
        }
        else
        {
            if (controller.isGrounded) move.y = 0;
            else move.y -= gravity * Time.deltaTime;
            move.x = 0;

            controller.Move(move * Time.deltaTime);
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Trap"))
        {
            weakened = true;
            animator.SetBool("damaged", true);
            animator.SetBool("die", true);
            Invoke("Die", timeToDestroy);
        }
    }
}
