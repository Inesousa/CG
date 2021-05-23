using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    private GameObject frontWall;

    // Start is called before the first frame update
    void Start()
    {
        frontWall = GameObject.Find("Front Wall");
        Physics.IgnoreCollision(transform.GetComponent<Collider>(), frontWall.transform.GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
