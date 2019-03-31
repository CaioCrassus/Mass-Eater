using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public CharacterController controller { private set; get; }

    //movement
    public float speed = 6;
    public float jumpSpeed = 10;
    public float gravity = 20;

    //control booleans
    private bool canMove = true;
    private bool canJump = false;
    public bool canHold = false;
    public bool holding = false;
    public bool crouching = false;
    private bool climbing = false;
    private bool dashing = false;
    private bool canDie = true;

    //dash
    public float dashDuration = .5f;
    public float dashSpeedMulti = 5f;
    public float dashCooldown = 3f;
    private float dashTimer = 0;
    private float dashCooldownTimer = 0;

    public LayerMask platformLayer;

    private Vector3 move = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        canJump = false;

        move.x = canMove && !climbing ? Input.GetAxis("Horizontal") * speed : move.x;
        if (canHold && holding)
        {
            if ((move.x < 0 && transform.rotation.y == 0) || (move.x > 0 && transform.rotation.y == -180))
                move.x = 0;
        }

        /*if (Input.GetButtonDown("Fire2") && dashCooldownTimer <= 0)
        {
            dashTimer = dashDuration;
            dashCooldownTimer = dashDuration + dashCooldown;
            move.y = 0;
            dashing = true;
        }
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            move.x = transform.right.x * speed * dashSpeedMulti;
        }
        else dashing = false;
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;*/

        if (!holding)
        {
            if (Input.GetAxis("Horizontal") > 0) transform.rotation = new Quaternion(0, 0, 0, 0);
            else if (Input.GetAxis("Horizontal") < 0) transform.localRotation = new Quaternion(0, 180, 0, 0);
        }

        if (controller.isGrounded) move.y = 0;

        bool rightRay = Physics.Raycast(transform.position + Vector3.up * .45f, Vector3.right, .45f, platformLayer);
        bool leftRay = Physics.Raycast(transform.position + Vector3.up * .45f, -Vector3.right, .45f, platformLayer);

        Debug.DrawRay(transform.position + Vector3.up * .45f, -transform.right * .45f, Color.white);
        Debug.DrawRay(transform.position + Vector3.up * .45f, transform.right * .45f, Color.white);


        if (!climbing && !dashing)
        {
            if ((leftRay && Input.GetAxis("Horizontal") < 0) || (rightRay && Input.GetAxis("Horizontal") > 0))
            {
                move.y = 0;
                move.y -= gravity * Time.deltaTime * 1.8f;
            }
            else
                move.y -= gravity * Time.deltaTime;
        }
        else if (climbing)
        {
            move.y = Input.GetAxis("Vertical") * speed * .8f;
        }

        if (Input.GetButtonDown("Jump") && !holding && !crouching)
        {
            if ((leftRay || rightRay) && !controller.isGrounded)
            {
                move.x = (leftRay ? jumpSpeed : -jumpSpeed) * 5;
                move.y = jumpSpeed;
            }
            else if (controller.isGrounded) move.y = jumpSpeed;
        }

        CrawlControl();
        //Debug.Log(move + " " + leftRay + " " + rightRay);
        controller.Move(move * Time.deltaTime);
        Vector3 aux = transform.position;
        aux.z = 0;
        transform.position = aux;

        if (Input.GetKeyDown(KeyCode.I))
        {
            canDie = !canDie;
            ImortalCheat.SetActive(!canDie);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            canHold = !canHold;
            BoxCheat.SetActive(canHold);
        }


    }

    public GameObject ImortalCheat;
    public GameObject BoxCheat;

    void CrawlControl()
    {
        if (Input.GetButton("Fire1") && controller.isGrounded && !holding)
        {
            controller.height = .1f;
            controller.radius = .1f;
            crouching = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            controller.height = .6f;
            controller.radius = .2f;
            crouching = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            SceneManager.LoadScene(0);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Climbable") && Input.GetButtonDown("Fire3"))
        {
            climbing = true;
        }
        if (other.CompareTag("Climbable") && Input.GetButtonUp("Fire3"))
        {
            climbing = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            climbing = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collision");
        if (hit.gameObject.tag == "Enemy" && canDie)
        {
            SceneManager.LoadScene(0);
        }
    }
}
