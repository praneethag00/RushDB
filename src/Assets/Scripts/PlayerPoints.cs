using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPoints : MonoBehaviour
{

    public int playerPoints;
    private PhotonView myPV;
    // Start is called before the first frame update
    void Start()
    {
        playerPoints = 0;
        myPV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void points(int playerID, int pts)
    {
        if(playerID == myPV.ViewID)
        {
            playerPoints += pts;
        }
    }
}
