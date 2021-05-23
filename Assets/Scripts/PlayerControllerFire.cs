using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerFire : MonoBehaviour{
    public float moveSpeed;
    //public Rigidbody theRB;
    public float jumpForce;
    public CharacterController controller;
    public GameObject bullet;
    
    private Vector3 moveDirection;
    public float gravityScale;

    public Animator anim;
    public Transform pivot;
    public float rotateSpeed;

    public GameObject playerModel;

    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;
    public float cooldown = 0f;
    // Start is called before the first frame update
    void Start() {
        //theRB = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {

        if(cooldown>0f)
            cooldown -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && cooldown<=0f)
        {
            GameObject bulletObject = Instantiate(bullet);
            bulletObject.transform.position = transform.position + transform.forward;
            bulletObject.transform.forward = transform.forward;
            cooldown = 1f;
        }

        if (knockBackCounter <= 0) {
            float yStore = moveDirection.y;
            //Player goes on the direction is faced
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed; //when you press W and A at the same time he doesn't change the velocity
            moveDirection.y = yStore;

            if (controller.isGrounded) {
                moveDirection.y = 0f;
                if (Input.GetButtonDown("Jump")) {
                    //theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
                    moveDirection.y = jumpForce;
                }
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
