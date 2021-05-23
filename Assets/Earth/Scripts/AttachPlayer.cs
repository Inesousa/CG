using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPlayer : MonoBehaviour
{

    public GameObject player;

    private Vector3 platPosNow;
    private Vector3 platPosBefore;

    private Vector3 platPosDiff;

    private bool isOnPlat = false;  //whether the player is on the platform or not

    private GameObject bars;

    private CharacterController controller;

    private GameObject pillar;

    private Transform parent; //empty parent

    private Vector3 dir;


    private Vector3 platRotNow;
    private Vector3 platRotBefore;

    private Vector3 platRotDiff;

    void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            isOnPlat = true;
            Debug.Log(c);
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            isOnPlat = false;
            Debug.Log(c);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bars = GameObject.Find("Jail/Bars");
        Physics.IgnoreCollision(bars.GetComponent<Collider>(), GetComponent<Collider>());

        controller = player.GetComponent<CharacterController>();

        parent = this.transform.parent;

        platRotBefore = new Vector3(parent.eulerAngles.x, parent.eulerAngles.y, parent.eulerAngles.z);
        platPosBefore = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pillar = GameObject.Find("EarthPillar(Clone)");
    }

    void FixedUpdate()
    {
        if(isOnPlat && controller.isGrounded)
        {
            platRotNow = new Vector3(parent.eulerAngles.x, parent.eulerAngles.y, parent.eulerAngles.z);
            platRotDiff = platRotNow - platRotBefore;

            player.transform.position = RotatePointAroundPivot(player.transform.position, parent.position, platRotDiff);
            Camera.main.transform.position = RotatePointAroundPivot(player.transform.position, parent.position, platRotDiff);

            platRotBefore = new Vector3(parent.eulerAngles.x, parent.eulerAngles.y, parent.eulerAngles.z);

            if (pillar != null)
            {
                pillar.transform.position = RotatePointAroundPivot(pillar.transform.position, parent.position, platRotDiff);
                pillar.transform.localEulerAngles = parent.eulerAngles;
            }
        }
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
}
