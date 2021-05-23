using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    public float moveSpeed;
    public float sprintModifier;
    //public Rigidbody theRB;
    public float jumpForce;
    public int jumpCount = 0;
    public CharacterController controller;
    
    private Vector3 moveDirection;
    public float gravityScale;

    public Animator anim;
    public Transform pivot;
    public float rotateSpeed;

    public GameObject playerModel;

    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;


    // Start is called before the first frame update
    void Start() {
        //theRB = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        //theRB.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, theRB.velocity.y,Input.GetAxis("Vertical") * moveSpeed);

        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);

        if (knockBackCounter <= 0) {
            float yStore = moveDirection.y;
            //Player goes on the direction is faced
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed; //when you press W and A at the same time he doesn't change the velocity
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDirection = moveDirection * sprintModifier;
                if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    anim.SetBool("Sprint", true);
                }
                else
                {
                    anim.SetBool("Sprint", false);
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                anim.SetBool("Sprint", false);
            }
            moveDirection.y = yStore;


            if (controller.isGrounded)
            {
                moveDirection.y = 0f;
            }

            if (Input.GetButtonDown("Jump") && jumpCount == 0)
            {
                //theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
                moveDirection.y = jumpForce;
                anim.SetBool("Sprint", false);
                jumpCount++;
            }
            else if (Input.GetButtonDown("Jump") && jumpCount == 1)
            {
                //theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
                moveDirection.y = jumpForce * 1.15f;
                jumpCount++;
            }
            else if (jumpCount == 2 && controller.isGrounded)
            {
                jumpCount = 0;
            }


        } else {
            knockBackCounter -= Time.deltaTime;
        }
        // Gravity of player
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);

        //Move the player in diff directions based on camera look direction
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));

    }

    public void KnockBack(Vector3 direction) {
        knockBackCounter = knockBackTime;

        moveDirection = direction * knockBackForce;
        moveDirection.y = knockBackForce;
    }
}
