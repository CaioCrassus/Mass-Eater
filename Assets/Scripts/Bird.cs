using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Bird : Enemy
{
    public bool vertical = true;
    public float speed = 2;

    private float direction = 1;
    private Vector3 origin;
    public LayerMask platform;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        origin = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!weakened)
        {
            Vector3 move = new Vector3();

            if (vertical)
            {
                bool ray = Physics.Raycast(transform.position, transform.up * direction, .45f, platform);
                Debug.DrawRay(transform.position, transform.up * .45f * direction, Color.white);

                if (ray)
                {
                    if (direction == 1) direction = -1;
                    else direction = 1;
                }
                move.y = direction * speed;
                animator.SetFloat("vel", move.y);
            }
            else
            {
                bool ray = Physics.Raycast(transform.position, transform.right, .45f, platform);
                Debug.DrawRay(transform.position, transform.right * .45f, Color.white);

                if (ray)
                {
                    if (direction == 1) direction = -1;
                    else direction = 1;
                }
                if (direction > 0) transform.rotation = new Quaternion(0, 0, 0, 0);
                else if (direction < 0) transform.localRotation = new Quaternion(0, 180, 0, 0);
                move.x = direction * speed;
                animator.SetFloat("vel", move.x);
            }
            controller.Move(move * Time.deltaTime);
            Vector3 aux = transform.position;
            if (vertical) aux.x = origin.x;
            else aux.y = origin.y;
            aux.z = 0;
            transform.position = aux;
        }
        else
        {
            if (!controller.isGrounded)
                controller.Move(-transform.up * speed);
        }
    }
}
