using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    public PhotonView playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //connect
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //check to see if we are connected
        Debug.Log("Connected to master");
        PhotonNetwork.JoinRandomOrCreateRoom();

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        //when player joins a room instantiate their character
        GameObject playerpf = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        playerpf.name = "Player";
    }
}
