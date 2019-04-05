using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Bird : MonoBehaviour
{
    public bool vertical = true;
    public float speed = 2;

    private float direction = 1;
    private Vector3 origin;
    private CharacterController controller;
    public LayerMask platform;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        origin = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
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
        }
        else
        {
            bool ray = Physics.Raycast(transform.position, transform.right * direction, .45f, platform);
            Debug.DrawRay(transform.position, transform.right * .45f * direction, Color.white);

            if (ray)
            {
                if (direction == 1) direction = -1;
                else direction = 1;
            }
            move.x = direction * speed;
        }
        controller.Move(move * Time.deltaTime);
        Vector3 aux = transform.position;
        if (vertical) aux.x = origin.x;
        else aux.y = origin.y;
        aux.z = 0;
        transform.position = aux;

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Trap"))
        {
            Destroy(gameObject);
        }
    }
}
