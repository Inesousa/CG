using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePlatform : MonoBehaviour
{

    public GameObject platform;

    Collider colObject = null; //collider of object that collided with the pressure plate

    Animator platAnim;

    void OnTriggerStay(Collider c)
    {
        platAnim.SetBool("isActive", true);
        colObject = c;
    }

    void OnTriggerExit(Collider c) //doesn't get activated when we take down the earth pillar that called OnTriggerEnter
    {
        platAnim.SetBool("isActive", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        platAnim = platform.transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(colObject == null)  //if we put an earth pillar on the plate and then put down a new one somewhere else (thus, the old one gets deleted and so colObject becomes null), then set isActive to false
        {
            platAnim.SetBool("isActive", false);
        }
    }
}
