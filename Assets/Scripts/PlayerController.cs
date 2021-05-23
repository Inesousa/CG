using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour{
    public float moveSpeed;
    public float jumpForce;
    public int jumpCount = 0;
    public CharacterController controller;
    
    private Vector3 moveDirection;
    public float gravityScale;

    public Animator anim;
    public Transform pivot;
    public float rotateSpeed;

    public GameObject playerModel;
    public GameObject enemy;

    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;

    //Water
    public GameObject WaterEffectPrefab;
    public float EffectMoveSpeed = 1;
    public Transform HeadTransform;
    private EnemyWaterController target;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if (knockBackCounter <= 0) {

            float yStore = moveDirection.y;
            //Player goes on the direction is faced
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed; //when you press W and A at the same time he doesn't change the velocity

            SprintMethod();
            
            moveDirection.y = yStore;
            
            AttackMethod();
            moveDirection.y = yStore;
            
            JumpMethod();

        } else {
            knockBackCounter -= Time.deltaTime;
        }
        SetPlayerMovement();
    }

    private void SetPlayerMovement() {
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


    private void SprintMethod() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            moveDirection = moveDirection * 2f;
            anim.SetBool("Sprint", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            anim.SetBool("Sprint", false);
        }
        if (controller.isGrounded) {
            moveDirection.y = 0f;
        }
    }
    private void AttackMethod() {
        if (Input.GetButtonDown("Fire1")) {

            if (SceneManager.GetActiveScene().name == "Water") {
                WaterAttack();
            }

            if (SceneManager.GetActiveScene().name == "Fire") {
                //FireAttack();
            }

            if (SceneManager.GetActiveScene().name == "Earth") {
                //EarthAttack();
            }

            if (SceneManager.GetActiveScene().name == "Wind") {
                //WindAttack();
            }
        }

        if (Input.GetButtonUp("Fire1")) {
            anim.SetBool("Attack", false);
        }
    }

    private void WaterAttack() {
        float rangeHit = 10f;
        anim.SetBool("Attack", true);

        //Particle System play
        GameObject thisEffect = Instantiate(WaterEffectPrefab);//, HeadTransform.transform.position , Quaternion.identity);
        thisEffect.transform.position = transform.position + playerModel.transform.forward;
        thisEffect.GetComponent<Rigidbody>().velocity = playerModel.transform.forward * 15;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rangeHit)) {
            GameObject objectTarget = hit.transform.gameObject;
            EnemyWaterController enemyComponent = objectTarget.GetComponent<EnemyWaterController>();
            if (hit.transform.tag == "Enemy") {
                //FindObjectOfType<EnemyWaterController>().
                enemyComponent.TakeDamage(1);
            }
        }
    }

    private void JumpMethod() {
        if (Input.GetButtonDown("Jump") && jumpCount == 0) {
            //theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
            anim.SetBool("Sprint", false);
            moveDirection.y = jumpForce;
            jumpCount++;
        } else if (Input.GetButtonDown("Jump") && jumpCount == 1) {
            //theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
            anim.SetBool("Sprint", false);
            moveDirection.y = jumpForce * 1.15f;
            jumpCount++;
        } else if (jumpCount == 2 && controller.isGrounded) {
            jumpCount = 0;
        }
    }

    public void KnockBack(Vector3 direction) {
        knockBackCounter = knockBackTime;

        moveDirection = direction * knockBackForce;
        moveDirection.y = knockBackForce;
    }
}
