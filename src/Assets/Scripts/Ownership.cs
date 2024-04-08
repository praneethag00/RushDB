using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Owership script for ball - in multiplayer game the owner of object is set to who logs in first so they other plaeyer can't interact with it without
 * a change in ownership.
 * Created BY: Praneetha Gobburi
 * msu-pgobburi1@mcneese.edu*/

public class Ownership : MonoBehaviourPun, IPunOwnershipCallbacks
{
    public static PhotonView PV;
    public bool equipped;

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void onDestory()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {

        this.photonView.TransferOwnership(requestingPlayer);


    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {

    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.LogError("Object transferFailed");
    }

    public void Start()
    {
        PV = this.GetComponent<PhotonView>();
    }

    private void Update()
    {
        /* if the player presses 'e' near the obejct and they are not the owner of the object change in ownership starts*/
        equipped = this.GetComponent<PickupControls>().equipped;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            Transform playerTransform = player.GetComponent<Transform>();
            Vector3 distanceToPlayer = playerTransform.position - transform.position;
            if (distanceToPlayer.magnitude <= 1 && !equipped)
            {
                if (Input.GetKeyDown(KeyCode.E) && !equipped)
                    base.photonView.RequestOwnership();
            }


        }
    }

}
