using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public CharacterController controller { private set; get; }

    public static PlayerController instance { private set; get; }

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
    public bool climbing = false;
    private bool dashing = false;
    private bool canDie = true;
    public bool cameraOnPlayer = true;
    public bool isMoving = false;
    private bool onWall = false;
    private bool isJumping = false;

    //dash
    public float dashDuration = .5f;
    public float dashSpeedMulti = 5f;
    public float dashCooldown = 3f;
    private float dashTimer = 0;
    private float dashCooldownTimer = 0;

    public LayerMask platformLayer;

    private Vector3 move = Vector3.zero;


    private float life = 100;
    public SkinnedMeshRenderer tank;
    private float invencibleTime = .5f;
    private float invencibleTimer;
    private bool justDamaged = false;
    private int damageDirection;


    public static Vector3 respawnPos;


    private AudioSource audioSource;
    public AudioClip walkAudio;
    public AudioClip jumpAudio;
    public AudioClip landingAudio;
    public AudioClip damageAudio;

    void Start()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        transform.position = respawnPos;
    }

    void FixedUpdate()
    {
        if (invencibleTimer > 0)
        {
            invencibleTimer -= Time.deltaTime;
        }


        canJump = false;
        if (!canMove && move.y < 0)
        {
            canMove = true;
            move.y -= gravity * Time.fixedDeltaTime * 8.5f;
        }

        move.x = canMove && !climbing ? Input.GetAxis("Horizontal") * speed : move.x;
        if (move.x != 0)
        {
            isMoving = true;
        }
        else isMoving = false;

        if (!cameraOnPlayer) move.x = 0;
        /* if (canHold && holding)
        {
            if ((move.x < 0 && transform.rotation.y == 0) || (move.x > 0 && transform.rotation.y == -180))
                move.x = 0;
        }*/

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
            move.y = 0;
            if (isJumping)
            {
                isJumping = false;
                audioSource.PlayOneShot(landingAudio);
            }
        }

        bool upRay = Physics.Raycast(transform.position + Vector3.up * .45f, Vector3.up, .2f, platformLayer);
        bool rightRay = Physics.Raycast(transform.position + Vector3.up * .45f, Vector3.right, .5f, platformLayer);
        bool leftRay = Physics.Raycast(transform.position + Vector3.up * .45f, -Vector3.right, .5f, platformLayer);

        Debug.DrawRay(transform.position + Vector3.up * .45f, -transform.right * .45f, Color.white);
        Debug.DrawRay(transform.position + Vector3.up * .45f, transform.right * .45f, Color.white);
        Debug.DrawRay(transform.position + Vector3.up * .45f, transform.up * .2f, Color.white);

        if (upRay && move.y > 0) move.y = 0;

        if (!climbing)
        {
            if (!controller.isGrounded && move.y < 0 && ((leftRay && Input.GetAxis("Horizontal") < 0) || (rightRay && Input.GetAxis("Horizontal") > 0)))
            {
                onWall = true;
                if (move.y > 0 && canMove) move.y = 0;
                move.y -= gravity * Time.fixedDeltaTime * .05f;
                move.y = Mathf.Max(move.y, -gravity * 0.05f);
            }
            else
            {
                onWall = false;
                if (move.y <= 0) move.y -= gravity * Time.fixedDeltaTime * 1.5f;
                else move.y -= gravity * Time.fixedDeltaTime;
                move.y = Mathf.Max(move.y, -gravity);
            }
        }
        else if (climbing)
        {
            move.y = Input.GetAxis("Vertical") * speed * .8f;
        }

        if (Input.GetButtonDown("Jump") && !holding && !crouching)
        {
            isJumping = true;
            Debug.Log("Jump");
            if (!controller.isGrounded && (leftRay || rightRay))
            {
                move.x = leftRay ? wallJumpSpeed : -wallJumpSpeed;
                FlipPlayer();
                move.y = jumpSpeed;
                canMove = false;
            }
            else if (controller.isGrounded)
            {
                move.y = jumpSpeed;
            }
            audioSource.PlayOneShot(jumpAudio);
        }

        if (justDamaged)
        {
            move.x = speed * damageDirection;
            move.y = jumpSpeed;
            justDamaged = false;
            audioSource.PlayOneShot(damageAudio);
        }

        //if ((controller.collisionFlags & CollisionFlags.Above) != 0) move.y -= gravity * Time.fixedDeltaTime;

        if (controller.isGrounded && move.x != 0 && audioSource.clip != walkAudio && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(walkAudio);
            audioSource.loop = true;
        }
        CrawlControl();

        if (move.x == 0/* && audioSource.isPlaying && audioSource.clip == walkAudio*/) audioSource.Stop();

        controller.Move(move * Time.fixedDeltaTime);
        //Vector3 aux = transform.position;
        //aux.z = 0;
        //transform.position = aux;

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
            controller.height = .8f;
            controller.radius = .3f;
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
        if (hit.gameObject.tag == "Enemy" && canDie && invencibleTimer <= 0 && !hit.gameObject.GetComponent<Enemy>().weakened)
        {
            invencibleTimer = invencibleTime;
            life = Mathf.Clamp(life - 33.4f, 0, 100);
            tank.SetBlendShapeWeight(0, life);
            justDamaged = true;
            if (hit.transform.position.x > transform.position.x) damageDirection = 1;
            else damageDirection = -1;
            if (life <= 0)
            {
                Lose.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else if (hit.gameObject.tag == "Enemy" && hit.gameObject.GetComponent<Enemy>().weakened && Input.GetButtonDown("Fire3"))
        {
            life += hit.gameObject.GetComponent<Enemy>().mass;
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
