using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && transform.parent.GetComponent<Boss>().attack && transform.parent.GetComponent<Boss>().coolDownAttackTimer <= 0)
        {
            Debug.Log("Boss Hit");
            col.GetComponent<PlayerController>().LoseMass(transform.position.x);
            transform.parent.GetComponent<Boss>().attack = false;
            //transform.parent.GetComponent<Boss>().animator.SetBool("attack", false);
            gameObject.SetActive(false);
            transform.parent.GetComponent<Boss>().CoolDownAttackTime();
        }
    }
}
