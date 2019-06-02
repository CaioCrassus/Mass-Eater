using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionAttackBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && transform.parent.GetComponent<Scorpion>().attack)
        {
            col.GetComponent<PlayerController>().LoseMass(transform.position.x);
            transform.parent.GetComponent<Scorpion>().attack = false;
            transform.parent.GetComponent<Scorpion>().animator.SetBool("attack", false);
            gameObject.SetActive(false);
        }
    }
}
