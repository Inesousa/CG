using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    public float speed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyEffect());
    }

    private IEnumerator DestroyEffect() {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
