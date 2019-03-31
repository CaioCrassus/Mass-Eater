using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Bird : MonoBehaviour
{
    public bool vertical = true;
    public float distance;
    public float speed = 2;

    private float direction = 1;
    private Vector3 origin;
    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3();

        if (vertical)
        {
            if (transform.position.y < origin.y) direction = 1;
            else if (transform.position.y > origin.y + distance) direction = -1;
            move.y = direction * speed;
        }
        else
        {
            if (transform.position.x < origin.x) direction = 1;
            else if (transform.position.x > origin.x + distance) direction = -1;
            move.x = direction * speed;
        }
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
