using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Scorpion : MonoBehaviour
{

    public float speed = 2;
    public float gravity = 20;
    public float persueSpeed = 4;

    private CharacterController controller;

    public LayerMask platformLayer;

    private Vector3 move;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        bool ray = Physics.Raycast(transform.position + Vector3.up * 0.45f, transform.right, .8f, platformLayer);
        bool down = Physics.Raycast(transform.position + transform.right * 0.45f, Vector3.down, .8f, platformLayer);
        Debug.DrawRay(transform.position + Vector3.up * 0.45f, transform.right * .8f, Color.white);
        Debug.DrawRay(transform.position + transform.right * 0.45f, Vector3.down * .8f, Color.white);

        if (ray || !down)
        {
            Vector3 aux = transform.localEulerAngles;
            if (transform.localRotation.y == 0) aux.y = 180;
            else /*if (transform.localRotation.y == 180)*/ aux.y = 0;

            transform.localEulerAngles = aux;
        }

        move.x = transform.right.x * speed;
        move.y -= gravity * Time.deltaTime;
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
