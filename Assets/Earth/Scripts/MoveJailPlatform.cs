using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJailPlatform : MonoBehaviour
{

    private GameObject jailLevel;

    private Animator jailLevelAnim;

    // Start is called before the first frame update
    void Start()
    {
        jailLevel = GameObject.Find("Jail Level");
        jailLevelAnim = jailLevel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            jailLevelAnim.SetBool("isRotating", true);
        }
    }
}
