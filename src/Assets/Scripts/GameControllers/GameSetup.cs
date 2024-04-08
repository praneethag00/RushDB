using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*Game script to disconnect player and keep track of the game across the scenes
 * Created BY: Praneetha Gobburi
 * msu-pgobburi1@mcneese.edu*/

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public GameObject ball;
    public bool disconnect;
    public bool win;

    public Transform[] spawnPoints;

    private void OnEnable()
    {
        if (GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    //the function is called when the player presses m to go to the main menu or at the end of the game at game over screen
    public void DisconnectPlayer(bool dis, bool winner)
    {
        disconnect = dis;
        win = winner;
        StartCoroutine(DisconnectAndLoad());
    }

    //disconnect from the room and load the main screen
    IEnumerator DisconnectAndLoad()
    {

        if(disconnect == true)
        {
            PhotonNetwork.LeaveRoom();
            //check to make sure we are in room
            while (PhotonNetwork.InRoom)
            {
                yield return null;
            }
            SceneManager.LoadScene(0);
        }
        if(disconnect == false)
        {
            PhotonNetwork.Disconnect();
            //check to make sure we are disconnected
            while (PhotonNetwork.IsConnected)
            {
                yield return null;
            }
            SceneManager.LoadScene(3);
        }
        PhotonNetwork.LeaveRoom();
        
    }


    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}

