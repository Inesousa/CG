using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{

    public int currentGold;
    public Text goldText;

    public void Start() {
        //currentGold = 9;
    }

    public void Update() {

    }
    public void AddGold(int goldToAdd) {
        currentGold += goldToAdd;
        goldText.text = "Gold: " + currentGold;

        if (SceneManager.GetActiveScene().name == "Water" ) {
            if (currentGold == 9) {
                FindObjectOfType<DeploySpecial>().SpawnEnemy();
            }
        }
        if (currentGold == 10) {
            FindObjectOfType<DeploySpecial>().SpawnCheckPoint();
        }

    }
}
