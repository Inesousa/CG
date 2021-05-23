using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployEarthPillar : MonoBehaviour
{
    public GameObject pillarPrefab;
    
    public GameObject player;
    public CharacterController controller;

    public float raiseSpeed = 5;

    private GameObject currentPillar;
    public GameObject highlight;

    public float range;

    private bool pressedE = false;

    private bool rayCollided;

    private bool isPillarActive = false;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {

    }

    private GameObject spawnPillar()
    {
        GameObject pillar = Instantiate(pillarPrefab) as GameObject;
        pillar.transform.position = hit.point;
        pillar.SetActive(true);
        return pillar;
    }

    // Update is called once per frame
    void Update()
    {
        rayCollided = Physics.Raycast(player.transform.position, Camera.main.transform.forward, out hit, range);
        if (rayCollided)
        {
            Debug.DrawRay(player.transform.position, Camera.main.transform.forward * hit.distance, Color.yellow);
        }
        else
        {
            highlight.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isPillarActive)
            {
                Destroy(currentPillar);
                isPillarActive = false;
            }
            else
            {
                pressedE = true;
            }
        }
        if (pressedE && rayCollided)
        {
            highlight.SetActive(true);
            highlight.transform.position = hit.point;
            if (Input.GetKeyDown(KeyCode.Mouse0) && controller.isGrounded)
            {
                currentPillar = spawnPillar();
                pressedE = false;
                highlight.SetActive(false);
                isPillarActive = true;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                pressedE = false;
                highlight.SetActive(false);
            }
        }
    }
}
