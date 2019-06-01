using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed = 0.05f;
    public float jumpForce = 2;
    public float gravity = 20;

    public LayerMask platformLayer;

    public float persueSpeed = 0.1f;

    private Vector2 move;
    public bool persue;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        bool ray = Physics.Raycast(transform.position, transform.right, 1.8f, platformLayer);
        Debug.DrawRay(transform.position, transform.right * 1.8f, Color.white);

        if (ray)
        {
            if (transform.rotation.y == 0) transform.rotation = new Quaternion(0, 180, 0, 0);
            else transform.rotation = new Quaternion(0, 0, 0, 0);
        }


        move.x = transform.right.x * (persue ? persueSpeed : speed);
        if (controller.isGrounded) move.y = 0;
        else move.y -= gravity * Time.deltaTime;

        controller.Move(move);
        Vector3 aux = transform.position;
        aux.z = 0;
        transform.position = aux;
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Moveble"))
        {
            hit.gameObject.GetComponent<LimitX>().DestroyBox();
        }
    }
}
