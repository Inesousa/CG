using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeploySpecial : MonoBehaviour
{
    public GameObject lightSpecial;
    public GameObject lightRobot1;
    public GameObject lightRobot2;

    public GameObject enemySpecialPrefab;
    public GameObject coinSpecialPrefab;

    public GameObject cp;

    public void SpawnEnemy() {
        GameObject enemy = Instantiate(enemySpecialPrefab) as GameObject;
        enemy.transform.position = new Vector3(0f,1f,0f);

        lightSpecial.SetActive(false);
        lightRobot1.SetActive(true);
        lightRobot2.SetActive(true);

    }
    public void SpawnCoin() {
        GameObject coin = Instantiate(coinSpecialPrefab) as GameObject;
        coin.transform.position = new Vector3(0f, 3f, 0f);
    }

    public void SpawnCheckPoint() {
        GameObject cpoint = Instantiate(cp) as GameObject;

        if (SceneManager.GetActiveScene().name == "Water" ) {
            cpoint.transform.position = new Vector3(-15f, 3f, -20f);
        }

        if (SceneManager.GetActiveScene().name == "FireScene") {
            cpoint.transform.position = new Vector3(28.47f, 25.12f, -5.64f);

        }

        if (SceneManager.GetActiveScene().name == "EarthScene") {
            cpoint.transform.position = new Vector3(8.55f, 28.79f, 31.5f);

        }

        if (SceneManager.GetActiveScene().name == "AirScene") {
            cpoint.transform.position = new Vector3(-35f, 4.462f, 30.35f);

        }
    }
}
