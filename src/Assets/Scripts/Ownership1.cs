using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ownership1 : MonoBehaviourPun, IPunOwnershipCallbacks
{
    public static PhotonView PV;

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

        base.photonView.TransferOwnership(requestingPlayer);


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
        bool inRange = this.GetComponent<PickupControls>().isInRange;
        if (Input.GetKeyDown(KeyCode.P))
        {
            base.photonView.RequestOwnership();
        }
    } 

}
