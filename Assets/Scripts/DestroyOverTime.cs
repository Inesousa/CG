using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Destroy Effects (particle System)
public class DestroyOverTime : MonoBehaviour
{

    public float lifetime;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);
    }
}
