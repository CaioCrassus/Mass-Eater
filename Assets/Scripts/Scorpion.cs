using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Scorpion : MonoBehaviour
{

    public float speed = 2;
    public float gravity = 20;

    private CharacterController controller;

    public LayerMask platformLayer;

    public bool persue = false;
    public float persueSpeed = 2.5f;
    public LayerMask playerLayer;
    public float viewAngle = 15;
    public float viewRadius = 3;
    private Vector3 move;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, playerLayer);

        GameObject player = null;

        if (targetsInViewRadius.Length > 0)
        {
            player = targetsInViewRadius[0].gameObject;
            Vector3 dirToTarget = (player.transform.position - transform.position).normalized;
            if (Vector2.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, player.transform.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, platformLayer))
                {
                    Debug.DrawRay(transform.position, dirToTarget * 100000, Color.white);
                    persue = true;
                }
                else
                {
                    persue = false;
                }
            }
        }


        bool ray = Physics.Raycast(transform.position + Vector3.up * 0.45f, transform.right, .8f, platformLayer);
        bool down = Physics.Raycast(transform.position + transform.right * 0.45f, Vector3.down, .8f, platformLayer);
        //Debug.DrawRay(transform.position + Vector3.up * 0.45f, transform.right * .8f, Color.white);
        //Debug.DrawRay(transform.position + transform.right * 0.45f, Vector3.down * .8f, Color.white);

        Vector3 aux = transform.localEulerAngles;
        if ((ray || !down) && !persue)
        {
            if (transform.localRotation.y == 0) aux.y = 180;
            else /*if (transform.localRotation.y == 180)*/ aux.y = 0;

        }
        if (persue && player != null)
        {
            if (player.transform.position.x < transform.position.x) aux.y = 180;
            else aux.y = 0;
        }
        transform.localEulerAngles = aux;

        move.x = transform.right.x * (persue ? persueSpeed : speed);
        if (controller.isGrounded) move.y = 0;
        else move.y -= gravity * Time.deltaTime;
        controller.Move(move * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Trap"))
        {
            Destroy(gameObject);
        }
    }
}
