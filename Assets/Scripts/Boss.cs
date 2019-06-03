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


    private float life = 3;
    public bool attack = false;


    public Animator animator;
    private AudioSource audioSource;

    private PlayerController player;

    public GameObject attackBox;

    public float coolDownAttackTime = 2;
    public float coolDownAttackTimer;

    private bool damaged = false;
    private int damagedDir;

    public float stunTime = 1;
    private float stunTimer;

    public bool weak;
    public bool died;

    public Activate lastButtonDoor;

    public static bool bossDefeated;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        player = PlayerController.instance;
        //weak = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!died)
        {
            bool ray = Physics.Raycast(transform.position, transform.right, 1.8f, platformLayer);
            Debug.DrawRay(transform.position, transform.right * 1.8f, Color.white);

            if (ray)
            {
                if (transform.rotation.y == 0) transform.rotation = new Quaternion(0, 180, 0, 0);
                else transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            if (!weak)
            {
                move.x = transform.right.x * (persue ? persueSpeed : speed);
                if (controller.isGrounded) move.y = 0;
                else move.y -= gravity * Time.deltaTime;
            }

            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist < 3 && player.invencibleTimer <= 0)
            {
                attackBox.gameObject.SetActive(true);
                attack = true;
                //animator.SetBool("attack", true);
                move.x = 0;
            }
            else attackBox.gameObject.SetActive(false);

            if (coolDownAttackTimer > 0)
            {
                coolDownAttackTimer -= Time.fixedDeltaTime;
                move.x = 0;
            }


            if (damaged)
            {
                damaged = false;
                move.y = jumpForce;
                move.x = transform.right.x * speed;
                damagedDir = 0;
            }

            if (stunTimer > 0)
            {
                stunTimer -= Time.fixedDeltaTime;
                move.x = 0;
                move.y = 0;
            }

            if (life == 1)
            {
                if (Mathf.Abs(-35 - transform.position.x) >= .05f)
                    move.x = speed * (transform.position.x < -35 ? 1 : -1);
                else move.x = 0;
                if (controller.isGrounded) move.y = 0;
                else move.y -= gravity * Time.deltaTime;
            }

            controller.Move(move);
            Vector3 aux = transform.position;
            aux.z = 0;
            transform.position = aux;
        }
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /* if (hit.gameObject.tag == "Moveble")// && hit.gameObject.GetComponent<Rigidbody>().velocity.y < 0)
        {
            hit.gameObject.GetComponent<LimitX>().DestroyBox();
            if (life == 1) LoseMass();
        }*/
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Trap"))
        {
            damaged = true;
            //animator.SetBool("damaged", true);
            damagedDir = (col.transform.position.x > transform.position.x) ? -1 : 1;
            LoseMass();
        }
    }

    public void CoolDownAttackTime()
    {
        coolDownAttackTimer = coolDownAttackTime;
    }

    public void LoseMass()
    {
        life -= 1;
        stunTimer = stunTime;
        if (life == 1)
        {
            //weak = true;
            lastButtonDoor.run();
            stunTimer = -1;
        }
        Debug.Log(life);
        if (life == 0)
        {
            Die();
            //animator.SetBool("Die", true);
        }
    }

    private void Die()
    {
        Debug.Log("Died");
        died = true;
        bossDefeated = true;


        Destroy(gameObject);
    }
}
