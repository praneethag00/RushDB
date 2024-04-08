using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

/*Lobby script for game - controls the buttons and texts in the game
 * Created BY: Praneetha Gobburi
 * msu-pgobburi1@mcneese.edu*/

public class PhotonLobby : MonoBehaviourPunCallbacks
{

    public static PhotonLobby lobby;

    public GameObject battleButton;//to start the search for rooms to join game
    public GameObject cancelButton; //if no the wait takes too long and the players wants to cancel after start search
    public GameObject controlButton;
    public GameObject controlText;
    public GameObject OfflineButton;
    public GameObject backButton;
    public GameObject logoText;

    private void Awake()
    {
        GameObject roomcont = GameObject.Find("GameSetup");
        Destroy(roomcont);
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //connects to master server on photon
        PhotonNetwork.ConnectUsingSettings();

    }

    //player load to the sync scene of the master
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        battleButton.SetActive(true);
        controlButton.SetActive(true);
    }

    //battle button is clicked connect to the room and create the start game
    public void onBBClicked()
    {
        //after BBbutton is clicked the BB button needs to disapear and the cancel button shows up
        battleButton.SetActive(false);
        controlButton.SetActive(false);
        cancelButton.SetActive(true);
        logoText.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }
    
    // open a text screen on how to play the game
    public void onControlBClicked()
    {
        battleButton.SetActive(false);
        controlButton.SetActive(false);
        controlText.SetActive(true);
        OfflineButton.SetActive(false);
        backButton.SetActive(true);
        logoText.SetActive(false);
    }
    
    //back button after how to play was clicked
    public void onBackBClicked()
    {
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        controlButton.SetActive(true);
        backButton.SetActive(false);
        controlText.SetActive(false);
        logoText.SetActive(true);
    }

    //player tries to join a room thats already full(so no open games ava) - so create a new room
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("no open game available");
        CreateRoom();
    }

    //create a new room
    void CreateRoom()
    {
        Debug.Log("Creating new room");
        int randomRoomName = Random.Range(0, 10000);
        // create room based on options
        RoomOptions roomsOp = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };
        PhotonNetwork.CreateRoom("Room " + randomRoomName, roomsOp);
    }

    //player joined another room


    //players fails to create a room - room name already exists
    //create a new room with a differnt name
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("create room failed");
        CreateRoom();
    }

    //joining a room took to long so player clicked on cancel button - lead the player back to the main menu screen
    public void OnCancelBClicked()
    {
        //if the cancel button is clicked the cancel button disappers and the battle button reappears.
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        controlButton.SetActive(true);
        logoText.SetActive(true);
        // and leave room
        PhotonNetwork.LeaveRoom();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
