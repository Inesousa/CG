
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWaterController : MonoBehaviour {
    public NavMeshAgent agent;
    public GameObject[] players;
    public GameObject player;

    public Animator anim;

    public float health;
    public float currHealth;
    public float moveSpeed;
    public GameObject deathEffect;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject playerDeathPrefab;

    //States
    public float sightRange, attackRange;

    public void Start() {
        currHealth = health;
        players = GameObject.FindGameObjectsWithTag("Player");
        player = players[0];
    }
    private void Update() {
        //Check for sight and attack range
        float distance = Vector3.Distance(player.transform.position, transform.position);
        //agent.speed = moveSpeed;
        
        if (distance <= sightRange) {
            ChasePlayer();
            //Face TArget
            //FaceTarget();
            agent.SetDestination(player.transform.position);
            if (distance <= attackRange) {
                AttackPlayer();
            }
        }else {
            Patroling();
        }
    }

    private void Patroling() {

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) {
            //anim.SetBool("Attack", false);
            anim.SetBool("Hit", false);
            anim.SetBool("Run", false);
            anim.SetBool("Walk", true);
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude <= 7f) {
            anim.SetBool("Walk", false);
            walkPointSet = false;
        }

    }
    private void SearchWalkPoint() {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f))
            walkPointSet = true;
    }

    private void ChasePlayer() {
        anim.SetBool("Hit", false);
        anim.SetBool("Walk", false);
        anim.SetBool("Run", true);

        agent.SetDestination(player.transform.position);
    }

    private void AttackPlayer() {
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
        anim.SetBool("Hit", false);

        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform);
    }


    public void TakeDamage(int damage) {

        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
        anim.SetBool("Hit", true);

        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        currHealth -= damage;
        if (currHealth < 1) {
            Instantiate(deathEffect, agent.transform.position, agent.transform.rotation);
            if (gameObject.name == "EnemySpecial(Clone)") {
                FindObjectOfType<DeploySpecial>().SpawnCoin();
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
