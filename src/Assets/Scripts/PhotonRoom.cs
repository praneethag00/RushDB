using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

/*Room script for multiplayer system, players are connected to a room and they are instatiated there
 * Created BY:  Praneetha Gobburi
 * msu-pgobburi1@mcneese.edu*/

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room info
    public static PhotonRoom room;
    private PhotonView PV;

    //public bool isGameLoded;
    public int currentScene;
    public int multiplayerScene;
    public int players;

    Player[] photonPlayer;
    public static int playersInRoom;
    public int myNumbersInRoom;

    public static int playersInGame;


    //set up a room and destroy the old one
    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //set up singleton
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                //destory the prvious set room and set it to this one
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
    }

    // get all the scenes together
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    //take away the scenes on quit
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    //after the player joins a room create the game 
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("in room");
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        StartGame();

        photonPlayer = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayer.Length;
        myNumbersInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumbersInRoom.ToString();
    }

    void StartGame()
    {
        Debug.Log("level 1 loading");
        PhotonNetwork.LoadLevel(multiplayerScene);
    }
   
    //after scene is built create the player
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == multiplayerScene)
        {
            CreatePlayer();
        }

    }

    private static void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), Vector3.zero, Quaternion.identity, 0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {


        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("HERE you can put if a player has left the room the other player becomes the winner");
        //if one player drops of 1v1 game here put that the other player is the winner display win screen to the winner
        //GameObject.Find("GameSetup").GetComponent<GameSetup>().DisconnectPlayer(false);
        PlayerControls.gameover(false, true);
    }

}
