using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody rd;

    public float movSpeed = 10;
    public float jumpSpeed = 5;
    public float gravity = 20;
    public LayerMask platform;

    private bool canMove = true;
    public bool canJump = false;
    public bool holdingOBJ = false;

    
    private Vector3 mov = new Vector3();

    public bool crawling = false;
    public bool climing = false;


    public float dashDuration = .5f;
    public float dashTimer = 0;
    public float dashSpeedMulti = 5f;
    public float dashCooldown = 3f;
    public float dashCooldownTimer = 0;

    void Start()
    {
        rd = GetComponent<Rigidbody>();
    }

   void FixedUpdate()
   {
       canJump = false;

        mov.x = canMove ? Input.GetAxis("Horizontal") * movSpeed: 0;

        if (Input.GetButtonDown("Fire2") && dashCooldownTimer <= 0) {
            dashTimer = dashDuration;
            dashCooldownTimer = dashDuration + dashCooldown;
        }
        if (dashTimer > 0){
            dashTimer -= Time.deltaTime;
            mov.x *= dashSpeedMulti;
        }
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (!holdingOBJ){
            if(Input.GetAxis("Horizontal") > 0) transform.localRotation = new Quaternion(0,0,0,0);
            else if (Input.GetAxis("Horizontal") < 0) transform.localRotation = new Quaternion(0,180,0,0);
        }

        rd.MovePosition(transform.position + mov * Time.deltaTime);

        bool jumping = Input.GetButtonDown("Jump");
        bool grounded = Physics.Raycast(transform.position, -transform.up, 8.5f);
        bool rightRay = Physics.Raycast(transform.position, transform.right, 5f, platform);
        bool leftRay = Physics.Raycast(transform.position, -transform.right, 5f, platform);

        //if (!grounded && !jumping && rd.velocity.y > 0) rd.velocity = new Vector3(rd.velocity.x, 0, rd.velocity.z);
        
        if (!crawling && !holdingOBJ && (grounded || leftRay || rightRay))
            canJump = true;
        if ((leftRay && mov.x < 0) || (rightRay && mov.x > 0)){
            if (rd.velocity.y < 0) rd.velocity = new Vector3(rd.velocity.x, -gravity * 0.8f, rd.velocity.z);
        } 
        Debug.DrawRay(transform.position, -transform.up * 8.5f, Color.white);
        Debug.DrawRay(transform.position, -transform.right * 4.5f, Color.white);
        Debug.DrawRay(transform.position, transform.right * 4.5f, Color.white);
        
        if (climing && Input.GetButtonUp("Fire2")) climing = false;
        
        if (!climing){
            if (jumping && canJump){
                canJump = false;
                if (leftRay || rightRay){
                    rd.velocity = new Vector3((leftRay? 1.1f : -1.1f) * movSpeed,jumpSpeed * .8f,0);
                    canMove = false;
                    Invoke("EnableMovement", 0.3f);
                }else rd.velocity = new Vector3(0,1,0) * jumpSpeed;
            }
            rd.velocity -= transform.up * gravity * Time.deltaTime;
        } else
        {
            mov.x = 0;
            mov.y = Input.GetAxis("Vertical") * movSpeed * .8f;
        }


        CrawlControl();
   }

   void EnableMovement(){
       canMove = true;
   }

    void CrawlControl(){
        if(Input.GetButton("Fire1") && !holdingOBJ){
            transform.localScale = new Vector3(transform.localScale.x, 4, transform.localScale.z);
            GetComponent<CapsuleCollider>().height = 0.5f;
            crawling = true;
        }

        if(Input.GetButtonUp("Fire1")){
            transform.localScale = new Vector3(transform.localScale.x, 8, transform.localScale.z);
            GetComponent<CapsuleCollider>().height = 2f;
            crawling = false;
        }
    }


    void OnTriggerStay(Collider other){
        if(other.CompareTag("Climbable") && Input.GetButtonDown("Fire3")){
            climing = true;
        }
        if(other.CompareTag("Climbable") && Input.GetButtonUp("Fire3")){
            climing = false;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag("Climbable")){
            climing = false;
        }
    }
}
