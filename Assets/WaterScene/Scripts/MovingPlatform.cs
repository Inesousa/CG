using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Transform platform;
    public Transform position1;
    public Transform position2;
    public Vector3 newPos;
    public string currentState;
    public float smooth;
    public float resetTime;
 
    private void Start() {
        ChangeTarget();        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        platform.position = Vector3.Lerp(platform.position, newPos, smooth * Time.deltaTime);
    }

    void ChangeTarget() {
        if (currentState == "1") {
            currentState = "2";
            newPos = position2.position;
        }
        else if (currentState == "2") {
            currentState = "1";
            newPos = position1.position;

        } else if (currentState == "") {
            currentState = "2";
            newPos = position2.position;
        }
        Invoke("ChangeTarget", resetTime);
    }
}
