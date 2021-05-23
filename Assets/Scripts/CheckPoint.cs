using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public HealthManager theHealthMan;

    public Renderer theRend;
    public Material cpOff;
    public Material cpOn;

    // Start is called before the first frame update
    void Start()
    {
        theHealthMan = FindObjectOfType<HealthManager>();    
    }


    public void CheckPointOn() {
        CheckPoint[] checkPoints = FindObjectsOfType<CheckPoint>();

        foreach (CheckPoint cp in checkPoints) {
            cp.CheckPointOff();
        }
        theRend.material = cpOn;
    }

    public void CheckPointOff() {
        theRend.material = cpOff;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player")) {
            theHealthMan.SetSpawnPoint(transform.position);
            CheckPointOn();
        }
    }
}
