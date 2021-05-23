using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {
    public int maxHealth;
    public int currentHealth;
    public PlayerControllerWater thePlayer;
    public CharacterController theCharacter;
    public GameObject player;
    public Text healthText;
    public float invincibilityLength;
    private float invincibilityCounter;
    private float flashCounter;
    public float flashLength;

    private bool isRespawning;
    public Vector3 respawnPoint;
    public float respawnLength;

    public GameObject deathEffect;
    public Image blackScreen;
    public Text playerDeath;
    private bool isFadeToBlack;
    private bool isFadeFromBlack;
    public float fadeSpeed;
    public float waitForFade;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        healthText.text = "Health: " + currentHealth;
        respawnPoint = thePlayer.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (invincibilityCounter > 0) {
            invincibilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;

            //skin fades away
            Invincibility();
        }
        BlackScreen();
    }
    private void Invincibility() {

        SkinnedMeshRenderer[] playerRenderers = FindObjectsOfType<SkinnedMeshRenderer>();
        if (flashCounter <= 0) {
            foreach (SkinnedMeshRenderer pr in playerRenderers) {
                if ( pr.transform.parent.gameObject == player) {
                    pr.enabled = !pr.enabled;
                }
            }
            flashCounter = flashLength;
        }
        if (invincibilityCounter <= 0) {
            foreach (SkinnedMeshRenderer pr in playerRenderers) {
                pr.enabled = true;
            }
        }
    }

    private void BlackScreen() {
        if (isFadeToBlack) {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            playerDeath.color = new Color(playerDeath.color.r, playerDeath.color.g, playerDeath.color.b, Mathf.MoveTowards(playerDeath.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 1f) {
                isFadeToBlack = false;
            }
        }
        if (isFadeFromBlack) {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            playerDeath.color = new Color(playerDeath.color.r, playerDeath.color.g, playerDeath.color.b, Mathf.MoveTowards(playerDeath.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 0f) {
                isFadeFromBlack = false;
            }
        }
    }


    public void HurtPlayer(int damage, Vector3 direction) {
        if (invincibilityCounter <= 0) {
            currentHealth -= damage;
            healthText.text = "Health: " + currentHealth;

            if (currentHealth <= 0) {
                Respawn();
            } else {
                thePlayer.KnockBack(direction);
                invincibilityCounter = invincibilityLength;

                SkinnedMeshRenderer[] playerRenderers = FindObjectsOfType<SkinnedMeshRenderer>();
                foreach (SkinnedMeshRenderer pr in playerRenderers) {
                    if (pr.transform.parent.gameObject == player) 
                        pr.enabled = false;
                }

                flashCounter = flashLength;
            }
        }
    }

    public void Respawn() {
        if (!isRespawning) {
            StartCoroutine("RespawnCo");
        }
    }

    public IEnumerator RespawnCo() {
        isRespawning = true;
        thePlayer.gameObject.SetActive(false);
        Instantiate(deathEffect, thePlayer.transform.position, thePlayer.transform.rotation);

        yield return new WaitForSeconds(respawnLength);

        isFadeToBlack = true;

        yield return new WaitForSeconds(waitForFade);

        isFadeToBlack = false;
        isFadeFromBlack = true;
        
        isRespawning = false;
        thePlayer.transform.position = respawnPoint;

        thePlayer.gameObject.SetActive(true);

        thePlayer.transform.position = respawnPoint;

        currentHealth = maxHealth;
        healthText.text = "Health: " + currentHealth;

        invincibilityCounter = invincibilityLength;

        SkinnedMeshRenderer[] playerRenderers = FindObjectsOfType<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer pr in playerRenderers) {
            if (pr.transform.parent.gameObject == player)
                pr.enabled = false;
        }
        flashCounter = flashLength;
    }

    public void HealPlayer(int healAmount) {
        currentHealth += healAmount;
        healthText.text = "Health: " + currentHealth;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }
    public void RefreshHealth() {
        healthText.text = "Health: " + currentHealth;
    }

    public void SetSpawnPoint(Vector3 newPosition) {
        respawnPoint = newPosition;
    }
}
