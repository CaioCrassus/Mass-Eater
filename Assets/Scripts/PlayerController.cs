using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public CharacterController controller { private set; get; }

    public static PlayerController instance {private set; get;}

    //movement
    public float speed = 6;
    public float jumpSpeed = 10;
    public float gravity = 20;

    public float wallJumpSpeed;

    //control booleans
    public bool canMove = true;
    private bool canJump = false;
    public bool canHold = false;
    public bool holding = false;
    public bool crouching = false;
    private bool climbing = false;
    private bool dashing = false;
    private bool canDie = true;
    public bool cameraOnPlayer = true;

    //dash
    public float dashDuration = .5f;
    public float dashSpeedMulti = 5f;
    public float dashCooldown = 3f;
    private float dashTimer = 0;
    private float dashCooldownTimer = 0;

    private int wallJumpMark = 0;

    public LayerMask platformLayer;

    private Vector3 move = Vector3.zero;

    void Start()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        canJump = false;
        if (!canMove && move.y < 0)
        {
            canMove = true;
            move.y -= gravity * Time.fixedDeltaTime * 8.5f;
        }

        move.x = canMove && !climbing && cameraOnPlayer ? Input.GetAxis("Horizontal") * speed : move.x;
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
            dashTimer -= Time.fixedDeltaTime;
            move.x = transform.right.x * speed * dashSpeedMulti;
        }
        else dashing = false;
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.fixedDeltaTime;*/

        FlipPlayer();

        if (controller.isGrounded)
        {
            wallJumpMark = 0;
            move.y = 0;
        }
        bool rightRay = Physics.Raycast(transform.position + Vector3.up * .45f, Vector3.right, .45f, platformLayer);
        bool leftRay = Physics.Raycast(transform.position + Vector3.up * .45f, -Vector3.right, .45f, platformLayer);

        Debug.DrawRay(transform.position + Vector3.up * .45f, -transform.right * .45f, Color.white);
        Debug.DrawRay(transform.position + Vector3.up * .45f, transform.right * .45f, Color.white);



        if (!climbing && !dashing)
        {
            if ((leftRay && Input.GetAxis("Horizontal") < 0) || (rightRay && Input.GetAxis("Horizontal") > 0))
            {
                if (move.y > 0 && canMove) move.y = 0;
                move.y -= gravity * Time.fixedDeltaTime * .05f;
            }
            else
                move.y -= gravity * Time.fixedDeltaTime;
        }
        else if (climbing)
        {
            move.y = Input.GetAxis("Vertical") * speed * .8f;
        }

        if (Input.GetButtonDown("Jump") && !holding && !crouching)
        {
            int wall = leftRay ? 1 : 2;
            if ((leftRay || rightRay) && !controller.isGrounded)
            {

                wallJumpMark = leftRay ? 1 : 2;
                move.x = leftRay ? wallJumpSpeed : -wallJumpSpeed;
                FlipPlayer();
                move.y = jumpSpeed;
                canMove = false;
            }
            else if (controller.isGrounded)
            {
                wallJumpMark = 0;
                move.y = jumpSpeed;
            }
        }

        //if ((controller.collisionFlags & CollisionFlags.Above) != 0) move.y -= gravity * Time.fixedDeltaTime;

        CrawlControl();
        Debug.Log(move);
        controller.Move(move * Time.fixedDeltaTime);
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
        if (other.CompareTag("Trap") && canDie)
        {
            Lose.SetActive(true);
            Time.timeScale = 0;
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


    public GameObject Lose;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collision");
        if (hit.gameObject.tag == "Enemy" && canDie)
        {
            Lose.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void FlipPlayer()
    {
        if (!holding)
        {
            if (move.x > 0) transform.rotation = new Quaternion(0, 0, 0, 0);
            else if (move.x < 0) transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
    }
}
