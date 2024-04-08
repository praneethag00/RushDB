using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Ball controls not just pickup, who the ball hits, how to pickit up , how to throw it, and gravity
 * Created BY: Michael Boudreaux, Praneetha Gobburi
 * msu-pgobburi1@mcneese.edu*/

public class PickupControls : MonoBehaviourPun
{
    public float sec;
    public Rigidbody rb;
    public SphereCollider coll;
    private Transform player, container, cam;

    public float pickUpRange;
    public float offset;
    public float throwUpwardForce;
    public PhotonView parentfrombefore;

    public bool equipped;
    public static bool slot;
    public bool pickupAction;
    public bool isInRange;

    private GameObject playerGO;
    private Animator animator;
    private PhotonView parentPV;
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    { 
        //initialize the variables with receptive objects and values.
        isInRange = false;
        playerGO = GameObject.Find("Player(Clone)");
        if (playerGO == null)
        {
            Debug.Log("player go is not found");
        }
        else
            Debug.Log("go here");


        player = playerGO.transform.GetComponent<Transform>();

        if (player == null)
        {
            Debug.Log("can't find transform");
        }
        else
            Debug.Log("tranform here");

        string path = "mixamorig2:Hips/mixamorig2:Spine/mixamorig2:Spine1/mixamorig2:Spine2/mixamorig2:LeftShoulder/mixamorig2:LeftArm/mixamorig2:LeftForeArm/mixamorig2:LeftHand/ball pos";
        cam = playerGO.transform.Find("Camera").GetComponent<Transform>();
        container = playerGO.transform.Find(path).GetComponent<Transform>();
        parentPV = playerGO.GetComponent<PhotonView>();
        int id = parentPV.ViewID;
        Debug.Log("player id " + id);
        PV = GetComponent<PhotonView>();
        animator = playerGO.GetComponent<Animator>();
        animator.SetBool("equipped", false);
        equipped = false;
        if (!equipped)
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
        }

        if (equipped)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
        }

        if (!PV.IsMine)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get distance but checking if the player is in range and if e was pressed
        
        Vector3 distanceToPlayer = player.position - transform.position;
        // player preesses e to pick up the ball
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E))
        {
            string path = "mixamorig2:Hips/mixamorig2:Spine/mixamorig2:Spine1/mixamorig2:Spine2/mixamorig2:LeftShoulder/mixamorig2:LeftArm/mixamorig2:LeftForeArm/mixamorig2:LeftHand/ball pos";
            if (parentPV.IsMine)
            {
                //if the player already has a ball in the box on their body they cant pick up another
                if (player.Find(path).childCount < 1)
                {
                    isInRange = true;
                    equipped = true;
                    pickupAction = true;
                    animator.SetTrigger("Pickup");
                    //string animation = "Pickup";
                    //int id = player.GetComponent<PhotonView>().ViewID;
                    //GetComponent<NetworkAnimation>().SendPlayAnimationEvent(PV.ViewID, animation, "Trigger", id);
                    StartCoroutine(Pickup());
                }
            }
        }

        if (distanceToPlayer.magnitude <= pickUpRange)
        {
            isInRange = false;
        }
        //player presses q to drop the ball
        if (equipped && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Drop!!");
            Drop();
        }
        //player presses 'left mouse cursor key' to fire/throw the ball
        if (equipped && Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Throw");
            Debug.Log("Shoot!!");
            GetComponent<AudioSource>().Play();
            StartCoroutine(Shoot());
        }
    }


    //send that the ball has been picked up by this player across the network
    [PunRPC]
    public void ObjOnPlayer( int id)
    {
        if (parentPV.IsMine)
        {
            GameObject otherGO = PhotonView.Find(id).gameObject;
            Debug.Log(id + " " + otherGO.name);
            string path = "mixamorig2:Hips/mixamorig2:Spine/mixamorig2:Spine1/mixamorig2:Spine2/mixamorig2:LeftShoulder/mixamorig2:LeftArm/mixamorig2:LeftForeArm/mixamorig2:LeftHand/ball pos";
            Transform othercont = otherGO.transform.Find(path).GetComponent<Transform>();
            transform.SetParent(othercont);
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            rb.isKinematic = true;


        }
    }

    //send that the object is no longer eqipped by the player across the network
    [PunRPC]
    public void ObjOffPlayer()
    {
        if (parentPV.IsMine)
        {
            transform.parent = null;
            rb.isKinematic = false;
        }
    }

    // when ball is picked up
    IEnumerator Pickup()
    {
        //give some time for animation to run
        if (animator.GetFloat("speed") > 0.1 && animator.GetFloat("speed") < 0.7)
        {
            sec = 0.2f;
            yield return new WaitForSeconds(sec);
        }
        else
        {
            sec = 0.6f;
            yield return new WaitForSeconds(sec);
        }
        animator.SetBool("equipped", true);
        //set the ball to be within the container on player
        transform.SetParent(container);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        //call the rpc function to sync the location of the object
        int id = parentPV.ViewID;
        PV.RPC("ObjOnPlayer", RpcTarget.AllBuffered, id);
        parentPV = playerGO.GetComponent<PhotonView>();
        parentfrombefore = parentPV;
        rb.isKinematic = true;

        animator.ResetTrigger("Pickup");
        
    }

    // player drops the ball
    void Drop()
    {
        //set the parent of this object to null so it is no longer bound in the container
        animator.SetBool("equipped", false);
        equipped = false;
        transform.parent = null;
        //send this to other clients
        PV.RPC("ObjOffPlayer", RpcTarget.AllBuffered);
        rb.isKinematic = false;
        coll.isTrigger = true;
    }

    //Player shoots the ball
    IEnumerator Shoot()
    {
        //wait for animation
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("equipped", false);
        equipped = false;
        //set parent to null so it is no loger bound and send the action to other clients 
        transform.parent = null;
        PV.RPC("ObjOffPlayer", RpcTarget.AllBuffered);
        rb.useGravity = true;
        rb.isKinematic = false;
        coll.isTrigger = true;
        //add force to the ball in the driection of the camera
        Vector3 forceDir = cam.transform.forward;
        Vector3 forceToAdd = forceDir * 20 + player.transform.up * throwUpwardForce;

        rb.AddForce(forceToAdd, ForceMode.Impulse);

        animator.ResetTrigger("Throw");

    }

    /*
    public void OnCollisionEnter(UnityEngine.Collision collider)
    {
        if (PV.IsMine)
        {
            Animator otherAnime = collider.gameObject.GetComponent<Animator>();
            if (parentfrombefore != null && collider.gameObject.CompareTag("Player"))
            {
                if ((parentfrombefore.ViewID != collider.gameObject.GetComponent<PhotonView>().ViewID))
                {
                    Debug.Log("hit Plyaer");
                    Debug.Log(parentfrombefore.ViewID + "hit" + collider.gameObject.GetComponent<PhotonView>().ViewID);
                    //otherAnime.SetTrigger("isHit");
                    //string animation = "isHit";
                    //ScoreManager.instance.points();
                    int id = collider.gameObject.GetComponent<PhotonView>().ViewID;
                    //collider.gameObject.GetComponent<PlayerPoints>().points(id, -5);
                    id = parentfrombefore.ViewID;
                    parentfrombefore.GetComponent<PlayerPoints>().points(id, 10);
                    //GetComponent<NetworkAnimation>().SendPlayAnimationEvent(PV.ViewID, animation, "isHit", id);
                }
            }
            if (parentfrombefore != null && collider.gameObject.CompareTag("Target"))
            {
                int id = parentfrombefore.ViewID;
                parentfrombefore.GetComponent<PlayerPoints>().points(id, 5);
            }
        }


    }*/
 
    //if the ball hits a player and if the ball hits a target
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in trigger");
        if (PV.IsMine)
        {
            Debug.Log("in trigger2" + parentfrombefore.ViewID);
            Animator otherAnime = other.GetComponent<Animator>();

            if (parentfrombefore != null && other.CompareTag("Player"))
            {
                if ((parentfrombefore.ViewID != other.GetComponent<PhotonView>().ViewID))
                {
                    Debug.Log("in trigger3");
                    Debug.Log("hit Plyaer");
                    Debug.Log(parentfrombefore.ViewID + "hit" + other.GetComponent<PhotonView>().ViewID);
                    other.GetComponent<AudioSource>().Play();
                    coll.isTrigger = false;
                }
            }
            if (other.CompareTag("Target"))
            {
                Debug.Log("in trigger4");
                int id = parentfrombefore.ViewID;
                Debug.Log("Hit target");
                parentfrombefore.RPC("ExtraPoints", RpcTarget.AllBuffered);
                coll.isTrigger = false;
                other.GetComponent<AudioSource>().Play();
            }
            if (other.CompareTag("Setting"))
            {
                coll.isTrigger = false;
                other.GetComponent<AudioSource>().Play();
            }
        }
        
    }

    /*
    Debug.Log("int " + parentfrombefore.ViewID);
    Debug.Log("int2 " + collider.gameObject.GetComponent<PhotonView>().ViewID);
    if(parentfrombefore.ViewID != collider.gameObject.GetComponent<PhotonView>().ViewID && collider.gameObject.CompareTag("Player"))
    {
        Debug.Log("hit plyaer");
    }
    if (!pickupAction)
    {
        Debug.Log("hit");
    }
    else
        Debug.Log("i hit somehting");

    /*
    }
/*if(parentfrombefore.veiwid != null && collider.gameboject.comapre tag)
 * if(parentfrombeforeive = collider gameobject
*/

}
