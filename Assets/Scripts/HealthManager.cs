using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {
    public int maxHealth;
    public int currentHealth;
    public PlayerControllerWater thePlayerWater;
    public PlayerControllerFire thePlayerFire;
    public PlayerController thePlayerEarth;
    public PlayerControllerAir thePlayerAir;


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

        if (SceneManager.GetActiveScene().name == "Water" || SceneManager.GetActiveScene().name == "MainScene") {

        respawnPoint = thePlayerWater.transform.position;
        }

        if (SceneManager.GetActiveScene().name == "FireScene") {
            respawnPoint = thePlayerFire.transform.position;
        }

        if (SceneManager.GetActiveScene().name == "EarthScene") {
            respawnPoint = thePlayerEarth.transform.position;
        }

        if (SceneManager.GetActiveScene().name == "AirScene") {
            respawnPoint = thePlayerAir.transform.position;
        }
        
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
                if (SceneManager.GetActiveScene().name == "Water") {
                    thePlayerWater.KnockBack(direction);
                }
                if (SceneManager.GetActiveScene().name == "FireScene") {
                    thePlayerFire.KnockBack(direction);

                }

                if (SceneManager.GetActiveScene().name == "EarthScene") {
                    thePlayerEarth.KnockBack(direction);
                }

                if (SceneManager.GetActiveScene().name == "AirScene") {
                    thePlayerAir.KnockBack(direction);
                }

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
        if (SceneManager.GetActiveScene().name == "Water") {
            thePlayerWater.gameObject.SetActive(false);
            Instantiate(deathEffect, thePlayerWater.transform.position, thePlayerWater.transform.rotation);
            yield return new WaitForSeconds(respawnLength);
            isFadeToBlack = true;
            yield return new WaitForSeconds(waitForFade);
            isFadeToBlack = false;
            isFadeFromBlack = true;
            isRespawning = false;
            thePlayerWater.transform.position = respawnPoint;
            thePlayerWater.gameObject.SetActive(true);
            thePlayerWater.transform.position = respawnPoint;
        }
        if (SceneManager.GetActiveScene().name == "FireScene") {
            thePlayerFire.gameObject.SetActive(false);
            Instantiate(deathEffect, thePlayerFire.transform.position, thePlayerFire.transform.rotation);
            yield return new WaitForSeconds(respawnLength);
            isFadeToBlack = true;
            yield return new WaitForSeconds(waitForFade);
            isFadeToBlack = false;
            isFadeFromBlack = true;
            isRespawning = false;
            thePlayerFire.transform.position = respawnPoint;
            thePlayerFire.gameObject.SetActive(true);
            thePlayerFire.transform.position = respawnPoint;
        }

        if (SceneManager.GetActiveScene().name == "EarthScene") {
            thePlayerEarth.gameObject.SetActive(false);
            Instantiate(deathEffect, thePlayerEarth.transform.position, thePlayerEarth.transform.rotation);
            yield return new WaitForSeconds(respawnLength);
            isFadeToBlack = true;
            yield return new WaitForSeconds(waitForFade);
            isFadeToBlack = false;
            isFadeFromBlack = true;
            isRespawning = false;
            thePlayerEarth.transform.position = respawnPoint;
            thePlayerEarth.gameObject.SetActive(true);
            thePlayerEarth.transform.position = respawnPoint;
        }

        if (SceneManager.GetActiveScene().name == "AirScene") {
            thePlayerAir.gameObject.SetActive(false);
            Instantiate(deathEffect, thePlayerAir.transform.position, thePlayerAir.transform.rotation);
            yield return new WaitForSeconds(respawnLength);
            isFadeToBlack = true;
            yield return new WaitForSeconds(waitForFade);
            isFadeToBlack = false;
            isFadeFromBlack = true;
            isRespawning = false;
            thePlayerAir.transform.position = respawnPoint;
            thePlayerAir.gameObject.SetActive(true);
            thePlayerAir.transform.position = respawnPoint;
        }

;

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
