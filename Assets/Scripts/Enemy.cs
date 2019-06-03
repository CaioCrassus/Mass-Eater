using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    protected CharacterController controller;

    public bool weakened = false;

    public float timeToDestroy = 5;

    public float mass = 10;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Trap"))
        {
            weakened = true;
            Invoke("Die", 10);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
